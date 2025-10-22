using IngSoftProyecto.Context;
using IngSoftProyecto.Models;

namespace IngSoftProyecto.CQRS.Commands
{
    public class TipoDeAsistenciaCommand
    {
        private readonly AppDbContext _context;
        public TipoDeAsistenciaCommand(AppDbContext context)
        {
            _context = context;
        }
        public virtual async Task<TipoDeAsistencia> AddTipoDeAsistencia(TipoDeAsistencia tipoDeAsistencia)
        {
            _context.TiposDeAsistencias.Add(tipoDeAsistencia);
            await _context.SaveChangesAsync();
            return tipoDeAsistencia;
        }
        virtual public async Task<TipoDeAsistencia?> UpdateTipoDeAsistencia(int id, TipoDeAsistencia tipoDeAsistencia)
        {
            var existingTipoDeAsistencia = await _context.TiposDeAsistencias.FindAsync(id);         
            existingTipoDeAsistencia.Descripcion = tipoDeAsistencia.Descripcion;
            await _context.SaveChangesAsync();
            return existingTipoDeAsistencia;
        }
    }
}
