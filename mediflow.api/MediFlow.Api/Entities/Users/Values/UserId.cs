using System.Diagnostics.CodeAnalysis;

namespace MediFlow.Api.Entities.Users.Values;

public readonly record struct UserId : IParsable<UserId>
{
    public required readonly Guid Value { get; init; }
    public static UserId Create(Guid value) => new() { Value = value };
    public static UserId Generate() => new() { Value = Guid.NewGuid() };
    public override string ToString() => Value.ToString();

    public static UserId Parse(string s, IFormatProvider? provider) => Create(Guid.Parse(s, provider));
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out UserId result)
    {
        var ok = Guid.TryParse(s, provider, out var value);
        result = ok ? Create(value) : default;
        return ok;
    }
}
