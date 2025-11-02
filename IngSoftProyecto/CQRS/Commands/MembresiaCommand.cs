using IngSoftProyecto.Context;
using IngSoftProyecto.Models;

namespace IngSoftProyecto.CQRS.Commands
{
    public class MembresiaCommand
    {
        private readonly AppDbContext _context;
        public MembresiaCommand(AppDbContext context)
        {
            _context = context;
        }
        virtual public async Task<Membresia> AddMembresia(Membresia request)
        {
            await _context.Membresias.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }
        virtual public async Task<Membresia> UpdateMembresia(Membresia request)
        {
            _context.Membresias.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }
        virtual public async Task<Membresia> DeleteMembresia(Membresia request)
        {
            _context.Membresias.Remove(request);
            await _context.SaveChangesAsync();
            return request;
        }

    }
}
