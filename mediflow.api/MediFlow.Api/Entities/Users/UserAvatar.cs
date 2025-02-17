﻿using MediFlow.Api.Entities.Users.Values;

namespace MediFlow.Api.Entities.Users;

public sealed class UserAvatar
{
    public required UserId UserId { get; init; }
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    public required byte[] Data { get; set; }

#nullable disable
    public User User { get; init; }
#nullable restore
}
