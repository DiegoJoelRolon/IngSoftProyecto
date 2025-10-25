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
    }
}
