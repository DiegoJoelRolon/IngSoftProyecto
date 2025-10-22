using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Models;

namespace IngSoftProyecto.Services
{
    public class TipoDeMembresiaService
    {
        private readonly TipoDeMembresiaCommand _command;
        private readonly TipoDeMembresiaQuery _query;
        public TipoDeMembresiaService(TipoDeMembresiaCommand command, TipoDeMembresiaQuery query)
        {
            _command = command;
            _query = query;
        }
        public virtual async Task<List<TipoDeMembresia>> GetAllTiposDeMembresias()
        {
            return await _query.GetAllTiposDeMembresias();
        }
        public virtual async Task<TipoDeMembresia?> GetTipoDeMembresiaById(int id)
        {
            return await _query.GetTipoDeMembresiaById(id);
        }
        public virtual async Task<TipoDeMembresia> AddTipoDeMembresia(TipoDeMembresia request)
        {
            return await _command.AddTipoDeMembresia(request);
        }
        public virtual async Task UpdateTipoDeMembresia(TipoDeMembresia request)
        {
            await _command.UpdateTipoDeMembresia(request);
        }
        public virtual async Task DeleteTipoDeMembresia(TipoDeMembresia request)
        {
            await _command.DeleteTipoDeMembresia(request);
        }
    }
}
