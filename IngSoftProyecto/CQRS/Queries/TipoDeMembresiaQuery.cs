using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using Microsoft.EntityFrameworkCore;
namespace IngSoftProyecto.CQRS.Queries
{
    public class TipoDeMembresiaQuery
    {
        private readonly AppDbContext _context;
        public TipoDeMembresiaQuery(AppDbContext context)
        {
            _context = context;
        }
        public virtual async Task<List<TipoDeMembresia>> GetAllTiposDeMembresias()
        {
            return await _context.TiposDeMembresias.ToListAsync();
        }
        public virtual async Task<TipoDeMembresia?> GetTipoDeMembresiaById(int id)
        {
            return await _context.TiposDeMembresias.FirstOrDefaultAsync(t => t.TipoDeMembresiaId == id);
        }
    }
}
