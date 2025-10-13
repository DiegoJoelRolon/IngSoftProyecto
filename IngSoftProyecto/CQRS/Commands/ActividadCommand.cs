using IngSoftProyecto.Context;
using IngSoftProyecto.Models;

namespace IngSoftProyecto.CQRS.Commands
{
    public class ActividadCommand
    {
        private readonly AppDbContext _context;

        public ActividadCommand(AppDbContext context)
        {
            _context = context;
        }
        public virtual async Task AddActividad(Actividad actividad)
        {
            _context.Actividades.Add(actividad);
            await _context.SaveChangesAsync();
        }
    }
}
