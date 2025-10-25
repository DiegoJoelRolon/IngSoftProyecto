using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Mapper
{
    public class AsistenciaMaper
    {
        private readonly MiembroXClaseMapper _miembroXClaseMapper;
        private readonly MembresiaXMiembroMapper _membresiaXMiembroMapper;
        private readonly GenericMapper _genericMapper;
        public AsistenciaMaper(MiembroXClaseMapper miembroXClaseMapper, MembresiaXMiembroMapper membresiaXMiembroMapper, GenericMapper genericMapper)
        {
            _miembroXClaseMapper = miembroXClaseMapper;
            _membresiaXMiembroMapper = membresiaXMiembroMapper;
            _genericMapper = genericMapper;
        }
        virtual public async Task<AsistenciaResponse> GetAsistenciaResponse(Asistencia asistencia)
        {

            return new AsistenciaResponse
            {
                AsistenciaId = asistencia.AsistenciaId,
                Fecha = asistencia.Fecha,
                MembresiaXMiembro = await _membresiaXMiembroMapper.GetMembresiaXMiembroResponse(asistencia.MembresiaXMiembro),
                MiembroXClase = await _miembroXClaseMapper.GetMiembroXClaseResponse(asistencia.MiembroXClase),
                TipoDeAsistencia = await _genericMapper.GetGenericResponse(asistencia.TipoDeAsistencia)
            };

        }

        virtual public async Task<List<AsistenciaResponse>> GetAsistenciaResponseList(List<Asistencia> asistencias)
        {
            var asistenciaResponses = new List<AsistenciaResponse>();
            foreach (var asistencia in asistencias)
            {
                asistenciaResponses.Add(await GetAsistenciaResponse(asistencia));
            }
            return asistenciaResponses;
        }
    }
}
