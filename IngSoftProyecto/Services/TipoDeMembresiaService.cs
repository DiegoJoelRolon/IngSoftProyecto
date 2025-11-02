using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Exceptions;
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
            await TipoDeMembresiaExists(id);
            return await _query.GetTipoDeMembresiaById(id);
        }
        public virtual async Task<TipoDeMembresia> AddTipoDeMembresia(TipoDeMembresia request)
        {
            await CheckTipoDeMembresiaRequest(request);
            return await _command.AddTipoDeMembresia(request);
        }
        public virtual async Task UpdateTipoDeMembresia(TipoDeMembresia request)
        {
            await TipoDeMembresiaExists(request.TipoDeMembresiaId);
            await _command.UpdateTipoDeMembresia(request);
        }
        public virtual async Task DeleteTipoDeMembresia(TipoDeMembresia request)
        {
            await TipoDeMembresiaExists(request.TipoDeMembresiaId);
            await _command.DeleteTipoDeMembresia(request);
        }

        private async Task<bool> TipoDeMembresiaExists(int id)
        {
            if (id <= 0 || await _query.GetTipoDeMembresiaById(id) == null)
                throw new NotFoundException("Id de tipo de membresia invalido");
            return true;
        }
        private async Task<bool> CheckTipoDeMembresiaRequest(TipoDeMembresia request)
        {
            if (string.IsNullOrEmpty(request.Descripcion))
            {
                throw new BadRequestException("Descripcion no puede estar vacio");
            }

            if (await _query.DescriptionExists(request.Descripcion, request.TipoDeMembresiaId))
            {
                throw new BadRequestException("Ya existe un tipo de membresía con esa descripción.");
            }

            return true;
        }
    }
}
