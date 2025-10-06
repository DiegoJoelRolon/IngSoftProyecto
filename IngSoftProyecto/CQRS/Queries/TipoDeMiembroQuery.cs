using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using Microsoft.EntityFrameworkCore;

namespace IngSoftProyecto.CQRS.Queries
{
    public class TipoDeMiembroQuery
    {
        private readonly AppDbContext _context;
        public TipoDeMiembroQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<TipoDeMiembro>> GetAllTiposDeMiembros()
        {
            return await _context.TiposDeMiembros.ToListAsync();
        }
        public async Task<TipoDeMiembro?> GetTipoDeMiembroById(int id)
        {
            return await _context.TiposDeMiembros.FirstOrDefaultAsync(t => t.TipoDeMiembroId == id);
        }
    }
}
