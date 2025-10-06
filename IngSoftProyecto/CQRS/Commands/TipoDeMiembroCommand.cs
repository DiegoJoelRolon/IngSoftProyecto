using IngSoftProyecto.Context;
using IngSoftProyecto.Models;

namespace IngSoftProyecto.CQRS.Commands
{
    public class TipoDeMiembroCommand
    {
        private readonly AppDbContext _context;
        public TipoDeMiembroCommand(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddTipoDeMiembro(TipoDeMiembro tipoDeMiembro)
        {
            _context.TiposDeMiembros.Add(tipoDeMiembro);
            await _context.SaveChangesAsync();
        }
    }
}
