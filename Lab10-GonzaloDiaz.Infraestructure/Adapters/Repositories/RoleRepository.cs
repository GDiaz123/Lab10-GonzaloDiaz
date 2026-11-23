using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lab10_GonzaloDiaz.Domain.Ports;
using Lab10_GonzaloDiaz.Infraestructure.Adapters.Repositories.Base;
using Lab10_GonzaloDiaz.Models;

namespace Lab10_GonzaloDiaz.Infrastructure.Adapters.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    private readonly ApplicationDbContext _db;

    public RoleRepository(ApplicationDbContext context)
        : base(context)
    {
        _db = context;
    }

    public async Task<Role?> ObtenerPorNombreAsync(string roleName)
    {
        return await _db.Set<Role>()
            .AsNoTracking()
            .FirstOrDefaultAsync(role => role.RoleName == roleName);
    }

    public async Task<bool> TieneRolAsync(Guid userId, Guid roleId)
    {
        return await _db.Set<UserRole>()
            .AnyAsync(userRole =>
                userRole.UserId == userId &&
                userRole.RoleId == roleId);
    }

    public async Task AsignarRolAsync(UserRole userRole)
    {
        await _db.Set<UserRole>().AddAsync(userRole);
        await _db.SaveChangesAsync();
    }
}