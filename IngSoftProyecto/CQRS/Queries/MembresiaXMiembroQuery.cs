using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using Microsoft.EntityFrameworkCore;

namespace IngSoftProyecto.CQRS.Queries
{
    public class MembresiaXMiembroQuery
    {
        private readonly AppDbContext _context;
        public MembresiaXMiembroQuery(AppDbContext context)
        {
            _context = context;
        }
        virtual public async Task<MembresiaXMiembro> GetMembresiaXMiembroById(int id)
        {
            return await _context.MembresiasXMiembros
                .Include(mxm => mxm.Miembro)
                .Include(mxm => mxm.Membresia)
                .Include(mxm => mxm.EstadoMembresia)
                .Include(mxm => mxm.Pago)
                .FirstOrDefaultAsync(mxm=>mxm.MembresiaXMiembroId==id);
        }
        virtual public async Task<List<MembresiaXMiembro>> GetAllMembresiasXMiembros()
        {
            return await _context.MembresiasXMiembros
                .Include(mxm => mxm.Miembro)
                .Include(mxm => mxm.Membresia)
                .Include(mxm => mxm.EstadoMembresia)
                .Include(mxm => mxm.Pago)
                .ToListAsync();
        }
        virtual public async Task<MembresiaXMiembro> GetUltimaMembresiaActiva(int miembroId)
        {
            // 1. Filtrar por el ID del miembro.
            // 2. Filtrar donde la FechaFin sea igual o posterior a la fecha y hora actuales (membresía vigente).
            return await _context.MembresiasXMiembros
                .Include(mxm => mxm.Membresia) // Necesitamos incluir Membresia para saber su duración y costo
                .Where(mxm => mxm.MiembroId == miembroId &&
                              mxm.FechaFin >= DateTime.Now)
                .OrderByDescending(mxm => mxm.FechaFin) // Ordenamos para asegurar que traemos la que finaliza más tarde
                .FirstOrDefaultAsync();
        }
    }
}
