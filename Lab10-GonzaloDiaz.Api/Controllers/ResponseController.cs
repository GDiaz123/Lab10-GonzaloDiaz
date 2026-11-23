using System.Security.Claims;
using Lab10_GonzaloDiaz.Application.Dtos;
using Lab10_GonzaloDiaz.Application.UserCase.ResponseUseCases;
using Lab10_GonzaloDiaz.Application.UserCase.TicketUseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_GonzaloDiaz.Api.Controllers;

[ApiController]
[Route("api/responses")]
[Authorize]
public class ResponsesController : ControllerBase
{
    private readonly CreateResponseUseCase _createResponseUseCase;
    private readonly GetResponseByIdUseCase _getResponseByIdUseCase;
    private readonly GetResponsesByTicketUseCase _getResponsesByTicketUseCase;
    private readonly GetTicketByIdUseCase _getTicketByIdUseCase;

    public ResponsesController(
        CreateResponseUseCase createResponseUseCase,
        GetResponseByIdUseCase getResponseByIdUseCase,
        GetResponsesByTicketUseCase getResponsesByTicketUseCase,
        GetTicketByIdUseCase getTicketByIdUseCase)
    {
        _createResponseUseCase = createResponseUseCase;
        _getResponseByIdUseCase = getResponseByIdUseCase;
        _getResponsesByTicketUseCase = getResponsesByTicketUseCase;
        _getTicketByIdUseCase = getTicketByIdUseCase;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ResponseDto>> GetById(Guid id)
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(claim, out var userId))
        {
            return Unauthorized();
        }

        var response = await _getResponseByIdUseCase.Execute(id);

        if (response == null)
        {
            return NotFound(new { message = "La respuesta no existe." });
        }

        var ticket = await _getTicketByIdUseCase
            .Execute(response.TicketId);

        if (ticket == null)
        {
            return NotFound(new { message = "El ticket no existe." });
        }

        if (ticket.UserId != userId &&
            !User.IsInRole("administrador"))
        {
            return Forbid();
        }

        return Ok(response);
    }

    [HttpGet("ticket/{ticketId:guid}")]
    public async Task<ActionResult<List<ResponseDto>>> GetByTicket(
        Guid ticketId)
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(claim, out var userId))
        {
            return Unauthorized();
        }

        var ticket = await _getTicketByIdUseCase.Execute(ticketId);

        if (ticket == null)
        {
            return NotFound(new { message = "El ticket no existe." });
        }

        if (ticket.UserId != userId &&
            !User.IsInRole("administrador"))
        {
            return Forbid();
        }

        try
        {
            var responses = await _getResponsesByTicketUseCase
                .Execute(ticketId);

            return Ok(responses);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ResponseDto>> Create(
        [FromBody] CreateResponseDto dto)
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(claim, out var userId))
        {
            return Unauthorized();
        }

        if (dto.TicketId == Guid.Empty)
        {
            return BadRequest(
                new { message = "El identificador del ticket es obligatorio." });
        }

        var ticket = await _getTicketByIdUseCase.Execute(dto.TicketId);

        if (ticket == null)
        {
            return NotFound(new { message = "El ticket no existe." });
        }

        if (ticket.UserId != userId &&
            !User.IsInRole("administrador"))
        {
            return Forbid();
        }

        try
        {
            var response = await _createResponseUseCase
                .Execute(dto, userId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.ResponseId },
                response);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }
}