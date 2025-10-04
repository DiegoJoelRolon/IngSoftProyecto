using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using Microsoft.EntityFrameworkCore;

namespace IngSoftProyecto.CQRS.Queries
{
    public class ActividadQuery
    {
        private readonly AppDbContext _context;
        public ActividadQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Actividad>> GetAllActividades()
        {
            return await _context.Actividades.ToListAsync();
        }
        public async Task<Actividad?> GetActividadById(int id)
        {
            return await _context.Actividades.FindAsync(id);
        }
    }
}
