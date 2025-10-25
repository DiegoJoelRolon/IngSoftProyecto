using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace IngSoftProyecto.CQRS.Queries
{
    public class AsistenciaQuery
    {
        private readonly AppDbContext _context;
        public AsistenciaQuery(AppDbContext context)
        {
            _context = context;
        }
        virtual public async Task<Asistencia> GetAsistenciaById(int id)
        {
            return await _context.Asistencias
                .Include(a => a.MiembroXClase)
                .Include(a => a.MembresiaXMiembro)
                .Include(a => a.TipoDeAsistencia)
                .FirstOrDefaultAsync(a => a.AsistenciaId == id);
        }
        virtual public async Task<List<Asistencia>> GetAllAsistencias()
        {
            return await _context.Asistencias
                .Include(a => a.MiembroXClase)
                .Include(a => a.MembresiaXMiembro)
                .Include(a => a.TipoDeAsistencia)
                .ToListAsync();
        }

    }
}
