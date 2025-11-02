using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Models;
namespace IngSoftProyecto.Mapper
{
    public class ClaseMapper
    {
        private readonly EntrenadorMapper _entrenadorMapper;
        private readonly ActividadMapper _actividadMapper;

        public ClaseMapper(EntrenadorMapper entrenadorMapper, ActividadMapper actividadMapper)
        {
            _entrenadorMapper = entrenadorMapper;
            _actividadMapper = actividadMapper;

        }

        public virtual async Task<List<ClaseResponse>> GetAllClasesResponse(List<Clase> clases)
        {
            List<ClaseResponse> clasesResponse = new List<ClaseResponse>();
            foreach (var clase in clases)
            {
                ClaseResponse claseResponse = new ClaseResponse
                {
                    ClaseId =  clase.ClaseId,
                    Actividad = await _actividadMapper.GetActividadResponse(clase.Actividad),
                    Entrenador = await _entrenadorMapper.GetEntrenadorResponse(clase.Entrenador),
                    Fecha = clase.Fecha,
                    HoraInicio = clase.HoraInicio,
                    HoraFin = clase.HoraFin,
                    Cupo = clase.Cupo
                };
                clasesResponse.Add(claseResponse);
            }
            return clasesResponse;
        }
        public virtual async Task<ClaseResponse> GetClaseResponse(Clase clase)
        {
            ClaseResponse claseResponse = new ClaseResponse
            {
                ClaseId = clase.ClaseId,
                Actividad = await _actividadMapper.GetActividadResponse(clase.Actividad),
                Entrenador = await _entrenadorMapper.GetEntrenadorResponse(clase.Entrenador),
                Fecha = clase.Fecha,
                HoraInicio = clase.HoraInicio,
                HoraFin = clase.HoraFin,
                Cupo = clase.Cupo
            };
            return claseResponse;
        }
    }
}
