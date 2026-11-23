using System.ComponentModel.DataAnnotations;

namespace Lab10_GonzaloDiaz.Application.Dtos;

public class LoginDto
{
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}