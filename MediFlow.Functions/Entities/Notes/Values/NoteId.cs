using System.Diagnostics.CodeAnalysis;

namespace MediFlow.Functions.Entities.Notes.Values;

public readonly record struct NoteId : IParsable<NoteId>
{
    public required readonly Guid Value { get; init; }
    public static NoteId Create(Guid value) => new() { Value = value };
    public static NoteId Generate() => new() { Value = Guid.NewGuid() };
    public override string ToString() => Value.ToString();

    public static NoteId Parse(string s, IFormatProvider? provider) => Create(Guid.Parse(s, provider));
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out NoteId result)
    {
        var ok = Guid.TryParse(s, provider, out var value);
        result = ok ? Create(value) : default;
        return ok;
    }
}
