using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lab10_GonzaloDiaz.Domain.Ports.Repositories.Base;
using Lab10_GonzaloDiaz.Models;

namespace Lab10_GonzaloDiaz.Domain.Ports;

public interface IUserRepository : IGenericRepository.IGenericRepository<User>
{
    Task<User?> ObtenerPorUsernameAsync(string username);
    Task<User?> ObtenerPorEmailAsync(string email);
    Task<List<string>> ObtenerRolesAsync(Guid userId);
}