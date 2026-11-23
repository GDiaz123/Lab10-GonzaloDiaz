using Lab10_GonzaloDiaz.Application.Dtos;
using Lab10_GonzaloDiaz.Domain.Ports;

namespace Lab10_GonzaloDiaz.Application.UserCase.ResponseUseCases;

public class GetResponseByIdUseCase
{
    private readonly IResponseRepository _responseRepository;

    public GetResponseByIdUseCase(
        IResponseRepository responseRepository)
    {
        _responseRepository = responseRepository;
    }

    public async Task<ResponseDto?> Execute(Guid id)
    {
        var response = await _responseRepository.GetByIdAsync(id);

        if (response == null)
        {
            return null;
        }

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