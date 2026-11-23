using Lab10_GonzaloDiaz.Application.Dtos;

using Lab10_GonzaloDiaz.Domain.Ports;

namespace Lab10_GonzaloDiaz.Application.UserCase.TicketUseCase;

public class GetTicketsUseCase
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketsUseCase(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<List<TicketDto>> Execute()
    {
        var tickets = await _ticketRepository.GetAllAsync();

        return tickets.Select(ticket => new TicketDto
        {
            TicketId = ticket.TicketId,
            UserId = ticket.UserId,
            Title = ticket.Title,
            Description = ticket.Description,
            CreatedAt = ticket.CreatedAt,
            ClosedAt = ticket.ClosedAt
        }).ToList();
    }
}