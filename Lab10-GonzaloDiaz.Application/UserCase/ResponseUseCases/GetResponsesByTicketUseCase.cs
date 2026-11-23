using Lab10_GonzaloDiaz.Application.Dtos;
using Lab10_GonzaloDiaz.Domain.Ports;

namespace Lab10_GonzaloDiaz.Application.UserCase.ResponseUseCases;

public class GetResponsesByTicketUseCase
{
    private readonly IResponseRepository _responseRepository;
    private readonly ITicketRepository _ticketRepository;

    public GetResponsesByTicketUseCase(
        IResponseRepository responseRepository,
        ITicketRepository ticketRepository)
    {
        _responseRepository = responseRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task<List<ResponseDto>> Execute(Guid ticketId)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);

        if (ticket == null)
        {
            throw new KeyNotFoundException("El ticket no existe.");
        }

        var responses = await _responseRepository
            .ObtenerPorTicketAsync(ticketId);

        return responses.Select(response => new ResponseDto
        {
            ResponseId = response.ResponseId,
            TicketId = response.TicketId,
            ResponderId = response.ResponderId,
            Message = response.Message,
            CreatedAt = response.CreatedAt
        }).ToList();
    }
}