using Lab10_GonzaloDiaz.Application.Dtos;
using Lab10_GonzaloDiaz.Domain.Ports;

namespace Lab10_GonzaloDiaz.Application.UserCase.RoleUseCases;

public class GetRolesUseCase
{
    private readonly IRoleRepository _roleRepository;

    public GetRolesUseCase(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<List<RoleDto>> Execute()
    {
        var roles = await _roleRepository.GetAllAsync();

        return roles.Select(role => new RoleDto
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName
        }).ToList();
    }
}