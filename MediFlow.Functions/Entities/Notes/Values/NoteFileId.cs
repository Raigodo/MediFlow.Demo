using System.Diagnostics.CodeAnalysis;

namespace MediFlow.Functions.Entities.Notes.Values;

public readonly record struct NoteFileId : IParsable<NoteFileId>
{
    public required readonly Guid Value { get; init; }
    public static NoteFileId Create(Guid value) => new() { Value = value };
    public static NoteFileId Generate() => new() { Value = Guid.NewGuid() };
    public override string ToString() => Value.ToString();

    public static NoteFileId Parse(string s, IFormatProvider? provider) => Create(Guid.Parse(s, provider));
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out NoteFileId result)
    {
        var ok = Guid.TryParse(s, provider, out var value);
        result = ok ? Create(value) : default;
        return ok;
    }
}
