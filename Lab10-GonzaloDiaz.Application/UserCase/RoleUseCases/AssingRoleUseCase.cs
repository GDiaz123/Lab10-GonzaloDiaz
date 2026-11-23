using Lab10_GonzaloDiaz.Application.Dtos;
using Lab10_GonzaloDiaz.Domain.Ports;
using Lab10_GonzaloDiaz.Models;

namespace Lab10_GonzaloDiaz.Application.UserCase.UserRoleUseCase;

public class AssignRoleUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public AssignRoleUseCase(
        IUserRepository userRepository,
        IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task Execute(AssignRoleDto dto)
    {
        if (dto.UserId == Guid.Empty || dto.RoleId == Guid.Empty)
        {
            throw new ArgumentException(
                "Los identificadores del usuario y del rol son obligatorios.");
        }

        var user = await _userRepository.GetByIdAsync(dto.UserId);

        if (user == null)
        {
            throw new KeyNotFoundException("El usuario no existe.");
        }

        var role = await _roleRepository.GetByIdAsync(dto.RoleId);

        if (role == null)
        {
            throw new KeyNotFoundException("El rol no existe.");
        }

        var hasRole = await _roleRepository
            .TieneRolAsync(dto.UserId, dto.RoleId);

        if (hasRole)
        {
            throw new InvalidOperationException(
                "El usuario ya tiene ese rol.");
        }

        var userRole = new UserRole
        {
            UserId = dto.UserId,
            RoleId = dto.RoleId,
            AssignedAt = DateTime.SpecifyKind(
                DateTime.UtcNow,
                DateTimeKind.Unspecified)
        };

        await _roleRepository.AsignarRolAsync(userRole);
    }
}