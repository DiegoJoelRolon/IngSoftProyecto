using IngSoftProyecto.Context;
using IngSoftProyecto.Models;

namespace IngSoftProyecto.CQRS.Commands
{
    public class MiembroXClaseCommand
    {
        private readonly AppDbContext _context;
        public MiembroXClaseCommand(AppDbContext context)
        {
            _context = context;
        }
        virtual public async Task<MiembroXClase> AddMiembroXClase(MiembroXClase request)
        {
            await _context.MiembrosXClases.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }
        virtual public async Task<MiembroXClase> UpdateMiembroXClase(MiembroXClase request)
        {
            _context.MiembrosXClases.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }

    }
}
