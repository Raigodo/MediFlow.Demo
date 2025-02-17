using System.Diagnostics.CodeAnalysis;

namespace MediFlow.Api.Entities.Employees.Values;

public readonly record struct InvitationId : IParsable<InvitationId>
{
    public required readonly Guid Value { get; init; }
    public static InvitationId Create(Guid value) => new() { Value = value };
    public static InvitationId Generate() => new() { Value = Guid.NewGuid() };
    public override string ToString() => Value.ToString();

    public static InvitationId Parse(string s, IFormatProvider? provider) => Create(Guid.Parse(s, provider));
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out InvitationId result)
    {
        var ok = Guid.TryParse(s, provider, out var value);
        result = ok ? Create(value) : default;
        return ok;
    }
}
