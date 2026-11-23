using Lab10_GonzaloDiaz.Application.Dtos;
using Lab10_GonzaloDiaz.Domain.Ports;
using Lab10_GonzaloDiaz.Models;

namespace Lab10_GonzaloDiaz.Application.UserCase.ResponseUseCases;

public class CreateResponseUseCase
{
    private readonly IResponseRepository _responseRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;

    public CreateResponseUseCase(
        IResponseRepository responseRepository,
        ITicketRepository ticketRepository,
        IUserRepository userRepository)
    {
        _responseRepository = responseRepository;
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
    }

    public async Task<ResponseDto> Execute(
        CreateResponseDto dto,
        Guid responderId)
    {
        if (string.IsNullOrWhiteSpace(dto.Message))
        {
            throw new ArgumentException("El mensaje es obligatorio.");
        }

        var ticket = await _ticketRepository
            .GetByIdAsync(dto.TicketId);

        if (ticket == null)
        {
            throw new KeyNotFoundException("El ticket no existe.");
        }

        var user = await _userRepository.GetByIdAsync(responderId);

        if (user == null)
        {
            throw new KeyNotFoundException("El usuario no existe.");
        }

        var response = new Response
        {
            ResponseId = Guid.NewGuid(),
            TicketId = dto.TicketId,
            ResponderId = responderId,
            Message = dto.Message.Trim(),
            CreatedAt = DateTime.SpecifyKind(
                DateTime.UtcNow,
                DateTimeKind.Unspecified)
        };

        await _responseRepository.AddAndSaveAsync(response);

        return new ResponseDto
        {
            ResponseId = response.ResponseId,
            TicketId = response.TicketId,
            ResponderId = response.ResponderId,
            Message = response.Message,
            CreatedAt = response.CreatedAt
        };
    }
}