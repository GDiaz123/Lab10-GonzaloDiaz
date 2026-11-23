using System;
using System.ComponentModel.DataAnnotations;

namespace Lab10_GonzaloDiaz.Application.Dtos;

public class CreateResponseDto
{
    public Guid TicketId { get; set; }

    [Required]
    public string Message { get; set; } = string.Empty;
}