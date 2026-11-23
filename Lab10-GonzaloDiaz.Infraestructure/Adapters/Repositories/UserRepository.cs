using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lab10_GonzaloDiaz.Domain.Ports;
using Lab10_GonzaloDiaz.Infraestructure.Adapters.Repositories.Base;
using Lab10_GonzaloDiaz.Models;

namespace Lab10_GonzaloDiaz.Infrastructure.Adapters.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext context)
        : base(context)
    {
        _db = context;
    }

    public async Task<User?> ObtenerPorUsernameAsync(string username)
    {
        return await _db.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Username == username);
    }

    public async Task<User?> ObtenerPorEmailAsync(string email)
    {
        return await _db.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<List<string>> ObtenerRolesAsync(Guid userId)
    {
        return await _db.Set<UserRole>()
            .Where(userRole => userRole.UserId == userId)
            .Join(
                _db.Set<Role>(),
                userRole => userRole.RoleId,
                role => role.RoleId,
                (userRole, role) => role.RoleName
            )
            .ToListAsync();
    }
}