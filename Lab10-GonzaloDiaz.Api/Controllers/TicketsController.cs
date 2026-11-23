using System.Security.Claims;
using Lab10_GonzaloDiaz.Application.Dtos;
using Lab10_GonzaloDiaz.Application.UserCase.TicketUseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_GonzaloDiaz.Api.Controllers;

[ApiController]
[Route("api/tickets")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly CreateTicketUseCase _createTicketUseCase;
    private readonly GetTicketsUseCase _getTicketsUseCase;
    private readonly GetTicketByIdUseCase _getTicketByIdUseCase;

    public TicketsController(
        CreateTicketUseCase createTicketUseCase,
        GetTicketsUseCase getTicketsUseCase,
        GetTicketByIdUseCase getTicketByIdUseCase)
    {
        _createTicketUseCase = createTicketUseCase;
        _getTicketsUseCase = getTicketsUseCase;
        _getTicketByIdUseCase = getTicketByIdUseCase;
    }

    [Authorize(Roles = "administrador")]
    [HttpGet]
    public async Task<ActionResult<List<TicketDto>>> GetAll()
    {
        var tickets = await _getTicketsUseCase.Execute();

        return Ok(tickets);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TicketDto>> GetById(Guid id)
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(claim, out var userId))
        {
            return Unauthorized();
        }

        var ticket = await _getTicketByIdUseCase.Execute(id);

        if (ticket == null)
        {
            return NotFound(new { message = "El ticket no existe." });
        }

        if (ticket.UserId != userId &&
            !User.IsInRole("administrador"))
        {
            return Forbid();
        }

        return Ok(ticket);
    }

    [HttpPost]
    public async Task<ActionResult<TicketDto>> Create(
        [FromBody] CreateTicketDto dto)
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(claim, out var userId))
        {
            return Unauthorized();
        }

        try
        {
            var ticket = await _createTicketUseCase.Execute(dto, userId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = ticket.TicketId },
                ticket);
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