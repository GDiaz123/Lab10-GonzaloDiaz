using Lab10_GonzaloDiaz.Application.Dtos;
using Lab10_GonzaloDiaz.Domain.Ports;
using Lab10_GonzaloDiaz.Models;

namespace Lab10_GonzaloDiaz.Application.UserCase.RoleUseCases;

public class CreateRoleUseCase
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleUseCase(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleDto> Execute(CreateRoleDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RoleName))
        {
            throw new ArgumentException("El nombre del rol es obligatorio.");
        }

        var roleName = dto.RoleName.Trim();

        var existingRole = await _roleRepository
            .ObtenerPorNombreAsync(roleName);

        if (existingRole != null)
        {
            throw new InvalidOperationException("El rol ya existe.");
        }

        var role = new Role
        {
            RoleId = Guid.NewGuid(),
            RoleName = roleName
        };

        await _roleRepository.AddAndSaveAsync(role);

        return new RoleDto
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName
        };
    }
}