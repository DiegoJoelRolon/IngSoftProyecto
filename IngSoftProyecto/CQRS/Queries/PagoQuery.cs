using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using Microsoft.EntityFrameworkCore;

namespace IngSoftProyecto.CQRS.Queries
{
    public class PagoQuery
    {
        private readonly AppDbContext _context;
        public PagoQuery(AppDbContext context)
        {
            _context = context;
        }
        virtual public async Task<Pago> GetPagoById(int id)
        {
            return await _context.Pagos.FindAsync(id);
        }
        virtual public async Task<List<Pago>> GetAllPagos()
        {
            return await _context.Pagos.ToListAsync();
        }

    }
}
