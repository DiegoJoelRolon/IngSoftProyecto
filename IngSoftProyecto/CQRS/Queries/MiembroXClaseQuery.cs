using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using Microsoft.EntityFrameworkCore;

namespace IngSoftProyecto.CQRS.Queries
{
    public class MiembroXClaseQuery
    {
        private readonly AppDbContext _context;
        public MiembroXClaseQuery(AppDbContext context)
        {
            _context = context;
        }
        virtual public async Task<MiembroXClase> GetMiembroXClaseById(int id)
        {
            return await _context.MiembrosXClases
                .Include(mxc => mxc.Miembro)
                .Include(mxc => mxc.Clase)
                .FirstAsync(mxc => mxc.MiembroXClaseId == id);
            
        }
        virtual public async Task<List<MiembroXClase>> GetAllMiembrosXClases()
        {
            return await _context.MiembrosXClases
                .Include(mxc => mxc.Miembro)
                .Include(mxc => mxc.Clase)
                .ToListAsync();
        }


    }
}
