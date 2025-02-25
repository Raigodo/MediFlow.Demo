using MediFlow.Functions.Entities.Employees;
using MediFlow.Functions.Entities.Notes;

namespace MediFlow.Functions.Modules.Notes.Functions.Responses;

public static class ConverterExtensions
{
    public static IEnumerable<NoteResponseDto> ToResponseDto(this IEnumerable<Note> notes)
    {
        return notes.Select(e => e.ToResponseDto());
    }

    public static NoteResponseDto ToResponseDto(this Note note)
    {
        return new NoteResponseDto()
        {
            Id = note.Id,
            ClientId = note.ClientId,
            Content = note.Content,
            CreatedOn = note.CreatedOn,
            IsFlagged = note.IsFlagged,
            Creator = note.Creator.ToCreatorDto(),
        };
    }

    private static NoteResponseDto.NoteCreatorDto ToCreatorDto(this Employee creator)
    {
        return new()
        {
            Id = creator.Id,
            Role = creator.Role,
            Name = creator.User.Name,
            Surname = creator.User.Surname,
        };
    }
}
