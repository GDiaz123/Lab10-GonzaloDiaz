using System;

namespace Lab10_GonzaloDiaz.Application.Dtos;

public class UserDto
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = string.Empty;

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }
}