using System.Security.Claims;
using Lab10_GonzaloDiaz.Application.Dtos;
using Lab10_GonzaloDiaz.Application.UserCase.UserUseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_GonzaloDiaz.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly GetUserByIdUseCase _getUserByIdUseCase;

    public UsersController(
        GetUserByIdUseCase getUserByIdUseCase)
    {
        _getUserByIdUseCase = getUserByIdUseCase;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(claim, out var userId))
        {
            return Unauthorized();
        }

        if (id != userId && !User.IsInRole("administrador"))
        {
            return Forbid();
        }

        var user = await _getUserByIdUseCase.Execute(id);

        if (user == null)
        {
            return NotFound(new { message = "El usuario no existe." });
        }

        return Ok(user);
    }
}