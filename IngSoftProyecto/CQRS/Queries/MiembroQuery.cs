using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using Microsoft.EntityFrameworkCore;

namespace IngSoftProyecto.CQRS.Queries
{
    public class MiembroQuery
    {
        private readonly AppDbContext _context;
        public MiembroQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Miembro>> GetAllMiembros()
        {
            return await _context.Miembros.ToListAsync();
        }
        public async Task<Miembro> GetMiembroById(int id)
        {
            return await _context.Miembros
                .Include(m => m.TipoDeMiembro)
                .Include(m=>m.Entrenador)
                .FirstOrDefaultAsync(m => m.MiembroId == id);
        }
    }
}
