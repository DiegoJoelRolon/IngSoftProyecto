using IngSoftProyecto.Context;
using IngSoftProyecto.Models;

namespace IngSoftProyecto.CQRS.Commands
{
    public class MembresiaXMiembroCommand
    {
        private readonly AppDbContext _context;

        public MembresiaXMiembroCommand(AppDbContext context)
        {
            _context = context;
            
        }

        virtual public async Task<MembresiaXMiembro> AddMembresiaXMiembro(MembresiaXMiembro request)
        {
            await _context.MembresiasXMiembros.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }
        virtual public async Task<MembresiaXMiembro> UpdateMembresiaXMiembro(MembresiaXMiembro request)
        {
            _context.MembresiasXMiembros.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }
        

    }
}
