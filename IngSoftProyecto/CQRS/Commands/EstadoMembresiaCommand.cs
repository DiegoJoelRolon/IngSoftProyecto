using IngSoftProyecto.Context;
using IngSoftProyecto.Models;

namespace IngSoftProyecto.CQRS.Commands
{
    public class EstadoMembresiaCommand
    {
        private readonly AppDbContext _context;
        public EstadoMembresiaCommand(AppDbContext context)
        {
            _context = context;
        }
        virtual public async Task<EstadoMembresia> AddEstadoMembresia(EstadoMembresia request)
        {
            await _context.EstadosMembresias.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }
        virtual public async Task<EstadoMembresia> UpdateEstadoMembresia(EstadoMembresia request)
        {
            _context.EstadosMembresias.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }
        virtual public async Task DeleteEstadoMembresia(EstadoMembresia request)
        {
            _context.EstadosMembresias.Remove(request);
            await _context.SaveChangesAsync();
        }

    }
}
