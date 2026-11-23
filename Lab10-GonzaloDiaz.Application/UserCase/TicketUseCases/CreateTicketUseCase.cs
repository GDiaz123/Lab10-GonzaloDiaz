using Lab10_GonzaloDiaz.Application.Dtos;
using Lab10_GonzaloDiaz.Domain.Ports;
using Lab10_GonzaloDiaz.Models;

namespace Lab10_GonzaloDiaz.Application.UserCase.TicketUseCase;

public class CreateTicketUseCase
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;

    public CreateTicketUseCase(
        ITicketRepository ticketRepository,
        IUserRepository userRepository)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
    }

    public async Task<TicketDto> Execute(
        CreateTicketDto dto,
        Guid userId)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new ArgumentException(
                "El título del ticket es obligatorio.");
        }

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new KeyNotFoundException("El usuario no existe.");
        }

        var ticket = new Ticket
        {
            TicketId = Guid.NewGuid(),
            UserId = userId,
            Title = dto.Title.Trim(),
            Description = dto.Description,
            CreatedAt = DateTime.SpecifyKind(
                DateTime.UtcNow,
                DateTimeKind.Unspecified),
            ClosedAt = null
        };

        await _ticketRepository.AddAndSaveAsync(ticket);

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