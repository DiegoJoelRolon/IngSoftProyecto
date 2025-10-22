using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Response;
namespace IngSoftProyecto.Mapper
{
    public class MembresiaMapper
    {
        private readonly GenericMapper _genericMapper;
        public MembresiaMapper(GenericMapper genericMapper)
        {
            _genericMapper = genericMapper;
        }

        public virtual async Task<MembresiaResponse> GetMembresiaResponse(Membresia membresia)
        {
            MembresiaResponse response = new MembresiaResponse
            {
                CostoBase = membresia.CostoBase,
                DuracionEnDias = membresia.DuracionEnDias,
                MembresiaId = membresia.MembresiaId,
                TipoDeMembresia = await _genericMapper.GetGenericResponse(membresia.TipoDeMembresia)
            };
            return response;
        }
        public virtual async Task<List<MembresiaResponse>> GetAllMembresiasResponse(List<Membresia> membresias)
        {
            List<MembresiaResponse> responses = new List<MembresiaResponse>();
            foreach (var membresia in membresias)
            {
                MembresiaResponse response = new MembresiaResponse
                {
                    CostoBase = membresia.CostoBase,
                    DuracionEnDias = membresia.DuracionEnDias,
                    MembresiaId = membresia.MembresiaId,
                    TipoDeMembresia = await _genericMapper.GetGenericResponse(membresia.TipoDeMembresia)
                };
                responses.Add(response);
            }
            return responses;
        }


    }
}
