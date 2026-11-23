using Lab10_GonzaloDiaz.Application.Dtos;
using Lab10_GonzaloDiaz.Application.UserCase.RoleUseCases;
using Lab10_GonzaloDiaz.Application.UserCase.UserRoleUseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_GonzaloDiaz.Api.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize(Roles = "administrador")]
public class RolesController : ControllerBase
{
    private readonly CreateRoleUseCase _createRoleUseCase;
    private readonly GetRolesUseCase _getRolesUseCase;
    private readonly AssignRoleUseCase _assignRoleUseCase;

    public RolesController(
        CreateRoleUseCase createRoleUseCase,
        GetRolesUseCase getRolesUseCase,
        AssignRoleUseCase assignRoleUseCase)
    {
        _createRoleUseCase = createRoleUseCase;
        _getRolesUseCase = getRolesUseCase;
        _assignRoleUseCase = assignRoleUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<List<RoleDto>>> GetAll()
    {
        var roles = await _getRolesUseCase.Execute();

        return Ok(roles);
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> Create(
        [FromBody] CreateRoleDto dto)
    {
        try
        {
            var role = await _createRoleUseCase.Execute(dto);

            return StatusCode(StatusCodes.Status201Created, role);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    [HttpPost("assign")]
    public async Task<IActionResult> Assign(
        [FromBody] AssignRoleDto dto)
    {
        try
        {
            await _assignRoleUseCase.Execute(dto);

            return Ok(new { message = "Rol asignado correctamente." });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }
}