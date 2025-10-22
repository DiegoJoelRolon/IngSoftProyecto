using IngSoftProyecto.Context;
using IngSoftProyecto.Models;

namespace IngSoftProyecto.CQRS.Commands
{
    public class ClaseCommand
    {
        private readonly AppDbContext _context;
        public ClaseCommand(AppDbContext context)
        {
            _context = context;
        }
        public virtual async Task<Clase> AddClase(Clase clase)
        {
            _context.Clases.Add(clase);
            await _context.SaveChangesAsync();
            return clase;
        }
        public virtual async Task<Clase> UpdateClase(Clase clase)
        {
            _context.Clases.Update(clase);
            await _context.SaveChangesAsync();
            return clase;
        }
        public virtual async Task DeleteClase(Clase clase)
        {
            _context.Clases.Remove(clase);
            await _context.SaveChangesAsync();
        }
    }
}
