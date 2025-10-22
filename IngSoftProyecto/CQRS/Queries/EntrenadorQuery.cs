using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using Microsoft.EntityFrameworkCore;

namespace IngSoftProyecto.CQRS.Queries
{
    public class EntrenadorQuery
    {
        private readonly AppDbContext _context;
        public EntrenadorQuery(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task<Entrenador?> GetEntrenadorById(int entrenadorId)
        {
            return await _context.Entrenadores.FindAsync(entrenadorId);
        }       
        public virtual async Task<List<Entrenador>> GetAllEntrenadores()
        {
            return await _context.Entrenadores.ToListAsync();
        }
    }
}
