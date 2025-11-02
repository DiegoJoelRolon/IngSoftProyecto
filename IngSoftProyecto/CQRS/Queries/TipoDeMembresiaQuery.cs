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
        virtual public async Task<bool> DescriptionExists(string descripcion, int? excludeId = null)
        {
            return await _context.TiposDeMembresias
                .AnyAsync(t => t.Descripcion.ToLower() == descripcion.ToLower() && t.TipoDeMembresiaId != excludeId);
        }
        virtual public async Task<bool> HasActiveMembresia(int miembroId, int estadoActivoId)
        {
            // Asumiendo que 'EstadoActivoId' es el ID que representa el estado "ACTIVO"
            return await _context.MembresiasXMiembros
                .AnyAsync(m => m.MiembroId == miembroId && m.EstadoMembresiaId == estadoActivoId && m.FechaFin > DateTime.Today);
        }

    }
}
