using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using Microsoft.EntityFrameworkCore;

namespace IngSoftProyecto.CQRS.Queries
{
    public class TipoDeAsistenciaQuery
    {
        private readonly AppDbContext _context;
        public TipoDeAsistenciaQuery(AppDbContext context)
        {
            _context = context;
        }
        virtual public Task<List<TipoDeAsistencia>>GetAllTipoDeAsistencia()
        {
            return _context.TiposDeAsistencias.ToListAsync();
        }
        virtual public Task<TipoDeAsistencia?> GetTipoDeAsistenciaById(int id)
        {
            return _context.TiposDeAsistencias.FirstOrDefaultAsync(t => t.TipoDeAsistenciaId == id);
        }

    }
}
