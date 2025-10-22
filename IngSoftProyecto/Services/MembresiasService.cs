using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Services
{
    public class MembresiasService
    {
        private readonly MembresiaQuery _query;
        private readonly MembresiaCommand _command;
        private readonly TipoDeMembresiaService _tipoDeMembresiaService;
        private readonly MembresiaMapper _membresiaMapper;

        public MembresiasService(MembresiaQuery query, MembresiaCommand command, TipoDeMembresiaService tipoDeMembresiaService, MembresiaMapper membresiaMapper)
        {
            _query = query;
            _command = command;
            _tipoDeMembresiaService = tipoDeMembresiaService;
            _membresiaMapper = membresiaMapper;
        }
        public virtual async Task<List<MembresiaResponse>> GetAllMembresias()
        {
            var membresias = await _query.GetAllMembresias();
            return await _membresiaMapper.GetAllMembresiasResponse(membresias);
        }
        public virtual async Task<MembresiaResponse?> GetMembresiaById(int id)
        {
            var membresia =  await _query.GetMembresiaById(id);
            if (membresia == null)
            {
                return null;
            }
            return await _membresiaMapper.GetMembresiaResponse(membresia);
        }
        public virtual async Task<MembresiaResponse> AddMembresia(MembresiaRequest request)
        {
            var membresia = new Membresia
            {
                TipoDeMembresiaId = request.TipoDeMembresiaId,
                DuracionEnDias = request.DuracionEnDias,
                CostoBase = request.CostoBase
            };

            var result = await _command.AddMembresia(membresia);
            return await _membresiaMapper.GetMembresiaResponse(membresia);
        }
        public virtual async Task<MembresiaResponse> UpdateMembresia(int id, MembresiaRequest request)
        {
            var membresia = new Membresia
            {
                TipoDeMembresiaId = request.TipoDeMembresiaId,
                DuracionEnDias = request.DuracionEnDias,
                CostoBase = request.CostoBase
            };
            
            var result = await _command.UpdateMembresia(membresia);
            return await _membresiaMapper.GetMembresiaResponse(result);
        }
    }
}
