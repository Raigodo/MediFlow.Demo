using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MediFlow.Api.Application.Converters;

public class TypedIdConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.Name.EndsWith("Id"))
        {
            return false;
        }

        var valueProperty = typeToConvert.GetProperty("Value");
        if (valueProperty == null || valueProperty.GetGetMethod() == null)
        {
            return false;
        }

        var iParsableInterface = typeToConvert.GetInterfaces()
        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IParsable<>));

        if (iParsableInterface == null)
        {
            return false;
        }

        var genericArgument = iParsableInterface.GetGenericArguments()[0];
        if (genericArgument != typeToConvert)
        {
            return false;
        }

        return true;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return (JsonConverter)Activator.CreateInstance(typeof(TypedIdConverter))!;
    }
}

public class TypedIdConverter : JsonConverter<object>
{

    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var valueString = reader.GetString();
        var targetType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;

        var parseMethod = targetType.GetMethod("Parse", [typeof(string), typeof(IFormatProvider)]);
        if (parseMethod == null)
        {
            throw new InvalidOperationException($"No Parse method found on type {targetType.FullName}.");
        }

        var result = parseMethod.Invoke(null, [valueString, CultureInfo.InvariantCulture]);

        return result!;
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        var valueProperty = value.GetType().GetProperty("Value");
        var valueToSerialize = valueProperty?.GetValue(value);
        if (valueToSerialize is Guid guid && guid == Guid.Empty
            || valueToSerialize is string str && str == string.Empty)
        {
            writer.WriteNullValue();
        }
        else
        {
            JsonSerializer.Serialize(writer, valueToSerialize, options);
        }
    }
}