using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lab10_GonzaloDiaz.Domain.Ports.Repositories.Base;
using Lab10_GonzaloDiaz.Models;

namespace Lab10_GonzaloDiaz.Domain.Ports;

public interface ITicketRepository : IGenericRepository.IGenericRepository<Ticket>
{
    Task<List<Ticket>> ObtenerPorUsuarioAsync(Guid userId);
}