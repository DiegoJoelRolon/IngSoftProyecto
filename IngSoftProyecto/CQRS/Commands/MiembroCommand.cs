using IngSoftProyecto.Context;
using IngSoftProyecto.Models;

namespace IngSoftProyecto.CQRS.Commands
{
    public class MiembroCommand
    {
        private readonly AppDbContext _context;
        public MiembroCommand(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Miembro> AddMiembro(Miembro miembro)
        {
            _context.Miembros.Add(miembro);
            await _context.SaveChangesAsync();
            return miembro;
        }
        public async Task UpdateMiembro(Miembro miembro)
        {
            _context.Miembros.Update(miembro);
            await _context.SaveChangesAsync();
        }
    }
}
