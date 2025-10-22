using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Request;
namespace IngSoftProyecto.CQRS.Commands
{
    public class TipoDeMembresiaCommand
    {
        private readonly AppDbContext _context;
        public TipoDeMembresiaCommand(AppDbContext context)
        {
            _context = context;
        }
        virtual public async Task<TipoDeMembresia> AddTipoDeMembresia(TipoDeMembresia request)
        {
            await _context.TiposDeMembresias.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }
        virtual public async Task UpdateTipoDeMembresia(TipoDeMembresia request)
        {
            _context.TiposDeMembresias.Update(request);
            await _context.SaveChangesAsync();
        }
        virtual public async Task DeleteTipoDeMembresia(TipoDeMembresia request)
        {
            _context.TiposDeMembresias.Remove(request);
            await _context.SaveChangesAsync();
        }
    }
}
