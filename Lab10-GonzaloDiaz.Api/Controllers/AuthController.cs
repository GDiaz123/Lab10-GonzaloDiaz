using Lab10_GonzaloDiaz.Application.Dtos;
using Lab10_GonzaloDiaz.Application.UserCase.AuthUseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_GonzaloDiaz.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly RegisterUseCase _registerUseCase;
    private readonly LoginUseCase _loginUseCase;

    public AuthController(
        RegisterUseCase registerUseCase,
        LoginUseCase loginUseCase)
    {
        _registerUseCase = registerUseCase;
        _loginUseCase = loginUseCase;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(
        [FromBody] RegisterDto dto)
    {
        try
        {
            var user = await _registerUseCase.Execute(dto);

            return CreatedAtAction(
                nameof(UsersController.GetById),
                "Users",
                new { id = user.UserId },
                user);
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

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginDto dto)
    {
        try
        {
            var result = await _loginUseCase.Execute(dto);

            return Ok(result);
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(new { message = exception.Message });
        }
    }
}