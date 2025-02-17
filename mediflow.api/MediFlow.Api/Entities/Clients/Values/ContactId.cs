using System.Diagnostics.CodeAnalysis;

namespace MediFlow.Api.Entities.Clients.Values;

public readonly record struct ContactId : IParsable<ContactId>
{
    public required readonly Guid Value { get; init; }
    public static ContactId Create(Guid value) => new() { Value = value };
    public static ContactId Generate() => new() { Value = Guid.NewGuid() };
    public override string ToString() => Value.ToString();

    public static ContactId Parse(string s, IFormatProvider? provider) => Create(Guid.Parse(s, provider));
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ContactId result)
    {
        var ok = Guid.TryParse(s, provider, out var value);
        result = ok ? Create(value) : default;
        return ok;
    }
}
