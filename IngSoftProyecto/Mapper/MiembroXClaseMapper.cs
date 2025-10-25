using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Mapper
{
    public class MiembroXClaseMapper
    {
        private readonly MiembroMapper _miembroMapper;
        private readonly ClaseMapper _claseMapper;
        public MiembroXClaseMapper(MiembroMapper miembroMapper, ClaseMapper claseMapper)
        {
            _miembroMapper = miembroMapper;
            _claseMapper = claseMapper;
        }
        public virtual async Task<MiembroXClaseResponse> GetMiembroXClaseResponse(MiembroXClase miembroXClase)
        {
           
            return new MiembroXClaseResponse
            {
                Miembro = await _miembroMapper.GetMiembroResponse(miembroXClase.Miembro),
                Clase = await _claseMapper.GetClaseResponse(miembroXClase.Clase),
                FechaInscripcion = miembroXClase.FechaInscripcion,
                MiembroXClaseId=miembroXClase.MiembroXClaseId

            };
        }
        public virtual async Task<List<MiembroXClaseResponse>> GetMiembroXClaseResponseList(List<MiembroXClase> miembrosXClases)
        {
            var miembroXClaseResponses = new List<MiembroXClaseResponse>();
            foreach (var miembroXClase in miembrosXClases)
            {
                miembroXClaseResponses.Add(await GetMiembroXClaseResponse(miembroXClase));
            }
            return miembroXClaseResponses;
        }

    }
}
