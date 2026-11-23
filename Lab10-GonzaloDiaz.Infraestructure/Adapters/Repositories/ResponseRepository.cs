using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lab10_GonzaloDiaz.Domain.Ports;
using Lab10_GonzaloDiaz.Infraestructure.Adapters.Repositories.Base;
using Lab10_GonzaloDiaz.Models;

namespace Lab10_GonzaloDiaz.Infrastructure.Adapters.Repositories;

public class ResponseRepository
    : GenericRepository<Response>, IResponseRepository
{
    private readonly ApplicationDbContext _db;

    public ResponseRepository(ApplicationDbContext context)
        : base(context)
    {
        _db = context;
    }

    public async Task<List<Response>> ObtenerPorTicketAsync(Guid ticketId)
    {
        return await _db.Set<Response>()
            .AsNoTracking()
            .Where(response => response.TicketId == ticketId)
            .OrderBy(response => response.CreatedAt)
            .ToListAsync();
    }
}