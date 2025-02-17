﻿using MediFlow.Api.Entities.Employees;
using MediFlow.Api.Entities.Employees.Values;
using MediFlow.Api.Entities.Journal.Values;

namespace MediFlow.Api.Entities.Journal;

public sealed class NoteFile
{
    public NoteFileId Id { get; init; } = NoteFileId.Generate();
    public required NoteId NoteId { get; init; }
    public required EmployeeId CreatorId { get; init; }
    public required string ContentType { get; set; } //application/pdf or image/jpeg etc.
    public required string FileName { get; set; }
    public required byte[] Bytes { get; set; } = [];
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;


#nullable disable
    public Note Note { get; init; } = null;
    public Employee Creator { get; init; } = null;
#nullable restore
}
