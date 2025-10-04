using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Models;

namespace IngSoftProyecto.Services
{
    public class ActividadService
    {
        private readonly ActividadQuery _actividadQuery;
        private readonly ActividadCommand _actividadCommand;
        private readonly ActividadMapper _actividadMapper = new ActividadMapper();
        public ActividadService(ActividadQuery actividadQuery,ActividadCommand actividadCommand,ActividadMapper actividadMapper)
        {
            _actividadQuery = actividadQuery;
            _actividadMapper = actividadMapper;
            _actividadCommand = actividadCommand;
        }

        public async Task<List<ActividadResponse>> GetAllActividades()
        {
            return await _actividadMapper.GetAllActividadesResponse((await _actividadQuery.GetAllActividades()));
        }
        public async Task<ActividadResponse> AddActividad(ActividadRequest request)
        {
            var actividad = new Actividad
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion
            };
           await _actividadCommand.AddActividad(actividad);
           return await _actividadMapper.GetActividadResponse(actividad);
        }
    }
}
