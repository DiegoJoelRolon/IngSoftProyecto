using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using Microsoft.EntityFrameworkCore;

namespace IngSoftProyecto.CQRS.Queries
{
    public class MiembroQuery
    {
        private readonly AppDbContext _context;
        public MiembroQuery(AppDbContext context)
        {
            _context = context;
        }
        virtual public async Task<List<Miembro>> GetAllMiembros()
        {
            return await _context.Miembros
                .Include(m => m.TipoDeMiembro)
                .Include(m => m.Entrenador)
                .ToListAsync();
        }
        virtual public async Task<Miembro> GetMiembroById(int id)
        {
            return await _context.Miembros
                .Include(m => m.TipoDeMiembro)
                .Include(m => m.Entrenador)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        virtual public async Task<bool> CheckIfTipoMembresiaIsAssigned(int tipoMembresiaId)
        {
            return await _context.Membresias
                .AnyAsync(m => m.TipoDeMembresiaId == tipoMembresiaId);
        }
        virtual public async Task<bool> MiembroExistsByDNI(int dni, int? excludeId = null)
        {
            // Verifica si existe algún miembro con ese DNI, excluyendo el ID actual si es una actualización
            return await _context.Miembros
                .Where(m => m.DNI == dni && m.Id != excludeId)
                .AnyAsync();
        }

        // 2. Verificar unicidad del Email
        virtual public async Task<bool> MiembroExistsByEmail(string email, int? excludeId = null)
        {
            // Verifica si existe algún miembro con ese Email, excluyendo el ID actual si es una actualización
            return await _context.Miembros
                .Where(m => m.Email == email && m.Id != excludeId)
                .AnyAsync();
        }
    }
}
