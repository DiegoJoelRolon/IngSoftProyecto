using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using Microsoft.EntityFrameworkCore;

namespace IngSoftProyecto.CQRS.Queries
{
    public class EstadoMembresiaQuery
    {
        private readonly AppDbContext _context;
        public EstadoMembresiaQuery(AppDbContext context)
        {
            _context = context;
        }

        virtual public async Task<EstadoMembresia> GetEstadoMembresiaById(int id)
        {
            return await _context.EstadosMembresias.FindAsync(id);
        }
        virtual public async Task<List<EstadoMembresia>> GetAllEstadosMembresias()
        {
            return await _context.EstadosMembresias.ToListAsync();
        }

    }
}
