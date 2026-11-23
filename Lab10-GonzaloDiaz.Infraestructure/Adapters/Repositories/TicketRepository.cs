using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lab10_GonzaloDiaz.Domain.Ports;
using Lab10_GonzaloDiaz.Infraestructure.Adapters.Repositories.Base;

using Lab10_GonzaloDiaz.Models;

namespace Lab10_GonzaloDiaz.Infrastructure.Adapters.Repositories;

public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
{
    private readonly ApplicationDbContext _db;

    public TicketRepository(ApplicationDbContext context)
        : base(context)
    {
        _db = context;
    }

    public async Task<List<Ticket>> ObtenerPorUsuarioAsync(Guid userId)
    {
        return await _db.Set<Ticket>()
            .AsNoTracking()
            .Where(ticket => ticket.UserId == userId)
            .OrderByDescending(ticket => ticket.CreatedAt)
            .ToListAsync();
    }
}