using System;

namespace Lab10_GonzaloDiaz.Application.Dtos;

public class ResponseDto
{
    public Guid ResponseId { get; set; }

    public Guid TicketId { get; set; }

    public Guid ResponderId { get; set; }

    public string Message { get; set; } = string.Empty;

    public DateTime? CreatedAt { get; set; }
}