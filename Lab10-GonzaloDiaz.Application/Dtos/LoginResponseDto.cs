using System;
using System.Collections.Generic;

namespace Lab10_GonzaloDiaz.Application.Dtos;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public string Username { get; set; } = string.Empty;

    public List<string> Roles { get; set; } = new List<string>();
}