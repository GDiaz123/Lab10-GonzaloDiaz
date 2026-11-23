using System.ComponentModel.DataAnnotations;

namespace Lab10_GonzaloDiaz.Application.Dtos;

public class CreateRoleDto
{
    [Required]
    [MaxLength(50)]
    public string RoleName { get; set; } = string.Empty;
}