using IngSoftProyecto.Context;
using IngSoftProyecto.Models;

namespace IngSoftProyecto.CQRS.Commands
{
    public class AsistenciaCommand
    {
        private readonly AppDbContext _context;
        public AsistenciaCommand(AppDbContext context)
        {
            _context = context;
        }
        virtual public async Task<Asistencia> AddAsistencia(Asistencia request)
        {
            await _context.Asistencias.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }
        virtual public async Task<Asistencia> UpdateAsistencia(Asistencia request)
        {
            _context.Asistencias.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }

    }
}
