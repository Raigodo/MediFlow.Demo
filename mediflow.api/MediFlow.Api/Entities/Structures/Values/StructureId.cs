using System.Diagnostics.CodeAnalysis;

namespace MediFlow.Api.Entities.Structures.Values;

public readonly record struct StructureId : IParsable<StructureId>
{
    public required readonly Guid Value { get; init; }
    public static StructureId Create(Guid value) => new() { Value = value };
    public static StructureId Generate() => new() { Value = Guid.NewGuid() };
    public override string ToString() => Value.ToString();

    public static StructureId Parse(string s, IFormatProvider? provider) => Create(Guid.Parse(s, provider));
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out StructureId result)
    {
        var ok = Guid.TryParse(s, provider, out var value);
        result = ok ? Create(value) : default;
        return ok;
    }
}
