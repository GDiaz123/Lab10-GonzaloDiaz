using System;
using System.Threading.Tasks;
using Lab10_GonzaloDiaz.Domain.Ports.Repositories.Base;
using Lab10_GonzaloDiaz.Models;

namespace Lab10_GonzaloDiaz.Domain.Ports;

public interface IRoleRepository : IGenericRepository.IGenericRepository<Role>
{
    Task<Role?> ObtenerPorNombreAsync(string roleName);
    Task<bool> TieneRolAsync(Guid userId, Guid roleId);
    Task AsignarRolAsync(UserRole userRole);
}