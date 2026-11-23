using System.ComponentModel.DataAnnotations;

namespace Lab10_GonzaloDiaz.Application.Dtos;

public class ChangeTicketStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}