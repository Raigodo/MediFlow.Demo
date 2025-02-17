using System.Diagnostics.CodeAnalysis;

namespace MediFlow.Api.Entities.Clients.Values;

public readonly record struct ClientId : IParsable<ClientId>
{
    public required readonly string Value { get; init; }
    public static ClientId Create(string value) => new() { Value = value };
    public override string ToString() => Value.ToString();

    public static ClientId Parse(string s, IFormatProvider? provider) => Create(s);
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ClientId result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }
        result = Create(s);
        return true;
    }
}
