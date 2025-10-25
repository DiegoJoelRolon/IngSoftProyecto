using IngSoftProyecto.Context;
using IngSoftProyecto.Models;

namespace IngSoftProyecto.CQRS.Commands
{
    public class PagoCommand
    {
        private readonly AppDbContext _context;
        public PagoCommand(AppDbContext context)
        {
            _context = context;
        }
        virtual public async Task<Pago> AddPago(Pago request)
        {
            await _context.Pagos.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }
        virtual public async Task<Pago> UpdatePago(Pago request)
        {
            _context.Pagos.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }

    }
}
