using Lab10_GonzaloDiaz.Application.Dtos;
using Lab10_GonzaloDiaz.Domain.Ports;

namespace Lab10_GonzaloDiaz.Application.UserCase.TicketUseCase;

public class GetTicketByIdUseCase
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketByIdUseCase(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<TicketDto?> Execute(Guid id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);

        if (ticket == null)
        {
            return null;
        }

        return new TicketDto
        {
            TicketId = ticket.TicketId,
            UserId = ticket.UserId,
            Title = ticket.Title,
            Description = ticket.Description,
            CreatedAt = ticket.CreatedAt,
            ClosedAt = ticket.ClosedAt
        };
    }
}