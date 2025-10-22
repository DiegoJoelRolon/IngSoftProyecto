using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using Microsoft.EntityFrameworkCore;

namespace IngSoftProyecto.CQRS.Queries
{
    public class MembresiaQuery
    {
        private readonly AppDbContext _context;
        public MembresiaQuery(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task<List<Membresia>> GetAllMembresias()
        {
            return await _context.Membresias
                .Include(m => m.TipoDeMembresia)
                .ToListAsync();
        }
        public virtual async Task<Membresia?> GetMembresiaById(int id)
        {
            return await _context.Membresias
                .Include(m => m.TipoDeMembresia)
                .FirstOrDefaultAsync(m => m.MembresiaId == id);
        }

    }
}
