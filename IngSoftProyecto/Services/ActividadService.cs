using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Services
{
    public class ActividadService
    {
        private readonly ActividadQuery _actividadQuery;
        private readonly ActividadCommand _actividadCommand;
        private readonly ActividadMapper _actividadMapper = new ActividadMapper();
        public ActividadService(ActividadQuery actividadQuery, ActividadCommand actividadCommand, ActividadMapper actividadMapper)
        {
            _actividadQuery = actividadQuery;
            _actividadMapper = actividadMapper;
            _actividadCommand = actividadCommand;
        }

        public virtual async Task<List<ActividadResponse>> GetAllActividades()
        {
            return await _actividadMapper.GetAllActividadesResponse((await _actividadQuery.GetAllActividades()));
        }
        public virtual async Task<ActividadResponse> AddActividad(ActividadRequest request)
        {
            CheckActividadRequest(request);
            var actividad = new Actividad
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion
            };
            await _actividadCommand.AddActividad(actividad);
            return await _actividadMapper.GetActividadResponse(actividad);
        }

        public virtual async Task<ActividadResponse> GetActividadById(int id)
        {
            await ActividadExists(id);
            var actividad = await _actividadQuery.GetActividadById(id);
            return await _actividadMapper.GetActividadResponse(actividad);
        }

        private async Task<bool> ActividadExists(int id)
        {
            if (id <= 0 || await _actividadQuery.GetActividadById(id) == null)
                throw new NotFoundException("Id de miembro invalido");
            return true;
        }
        private bool CheckActividadRequest(ActividadRequest request)
        {
            if (string.IsNullOrEmpty(request.Nombre) || string.IsNullOrEmpty(request.Descripcion))
            {
                throw new BadRequestException("Nombre o descripcion no pueden estar vacios");
            }
            return true;
        }
    }
}
