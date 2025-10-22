using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using Microsoft.EntityFrameworkCore;

namespace IngSoftProyecto.CQRS.Queries
{
    public class ClaseQuery
    {
        private readonly AppDbContext _context;

        public ClaseQuery(AppDbContext context)
        {
            _context = context;
        }
        public virtual async Task<List<Clase>> GetAllClases()
        {
            return await _context.Clases.ToListAsync();
        }
        public virtual async Task<Clase?> GetClaseById(int id)
        {
            return await _context.Clases.FindAsync(id);
        }

    }
}
