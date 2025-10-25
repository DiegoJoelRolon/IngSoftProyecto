using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
namespace IngSoftProyecto.CQRS.Commands
{
    public class EntrenadorCommand
    {
        private readonly AppDbContext _context;
        public EntrenadorCommand(AppDbContext context)
        {
            _context = context;
        }
        public virtual async Task<Entrenador> AddEntrenadorAsync(Entrenador entrenador)
        {
            _context.Entrenadores.Add(entrenador);
            await _context.SaveChangesAsync();
            return entrenador;
        }
        public virtual async Task<Entrenador> UpdateEntrenadorAsync(Entrenador entrenador)
        {
            _context.Entrenadores.Update(entrenador);
            await _context.SaveChangesAsync();
            return entrenador;
        }
        public virtual async Task DeleteEntrenadorAsync(Entrenador entrenador)
        {
            entrenador.Eliminado = true;
            _context.Entrenadores.Update(entrenador);
            await _context.SaveChangesAsync();
        }
        public virtual async Task<Entrenador> RestoreEntrenadorAsync(Entrenador entrenador)
        {
            entrenador.Eliminado = false;
            _context.Entrenadores.Update(entrenador);
            await _context.SaveChangesAsync();
            return entrenador;
        }

    }
}
