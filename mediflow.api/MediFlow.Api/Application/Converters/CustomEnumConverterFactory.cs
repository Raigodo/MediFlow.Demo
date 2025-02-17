using System.Text.Json;
using System.Text.Json.Serialization;

namespace MediFlow.Api.Application.Converters;

public class CustomEnumConverterFactory : JsonConverterFactory
{
    public sealed override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum || (Nullable.GetUnderlyingType(typeToConvert)?.IsEnum ?? false);
    }

    public sealed override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type underlyingType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;

        if (Nullable.GetUnderlyingType(typeToConvert) != null)
        {
            var nullableConverterType = typeof(NullableEnumConverter<>).MakeGenericType(underlyingType);
            return (JsonConverter)Activator.CreateInstance(nullableConverterType)!;
        }

        var converterType = typeof(CustomEnumConverter<>).MakeGenericType(underlyingType);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }

    /// <summary>
    /// Handles non-nullable enums.
    /// </summary>
    public class CustomEnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();
                if (Enum.TryParse(value, true, out T result))
                {
                    return result;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                var value = reader.GetInt32();
                if (Enum.IsDefined(typeof(T), value))
                {
                    return (T)Enum.ToObject(typeof(T), value);
                }
            }

            throw new JsonException($"Unable to convert value to {typeof(T)}.");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    /// <summary>
    /// Handles nullable enums (T?).
    /// </summary>
    public class NullableEnumConverter<T> : JsonConverter<T?> where T : struct, Enum
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();
                if (Enum.TryParse(value, true, out T result))
                {
                    return result;
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                var value = reader.GetInt32();
                if (Enum.IsDefined(typeof(T), value))
                {
                    return (T)Enum.ToObject(typeof(T), value);
                }
            }

            throw new JsonException($"Unable to convert value to {typeof(T)}.");
        }

        public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString());
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}