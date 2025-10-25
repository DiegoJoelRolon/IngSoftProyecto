using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;
namespace IngSoftProyecto.Services
{
    public class ClaseService
    {
        private readonly ClaseQuery _claseQuery;
        private readonly ClaseCommand _claseCommand;
        private readonly ClaseMapper _claseMapper;
        private readonly ActividadService _actividadService;
        private readonly EntrenadorService _entrenadorService;
        public ClaseService(ClaseQuery claseQuery, ClaseCommand claseCommand, ClaseMapper claseMapper, ActividadService actividadService,EntrenadorService entrenadorService)
        {
            _claseQuery = claseQuery;
            _claseMapper = claseMapper;
            _claseCommand = claseCommand;
            _actividadService = actividadService;
            _entrenadorService = entrenadorService;
        }

        public virtual async Task<List<ClaseResponse>> GetAllClases()
        {
            return await _claseMapper.GetAllClasesResponse((await _claseQuery.GetAllClases()));
        }
        public virtual async Task<ClaseResponse> GetClaseById(int claseId)
        {
            await ClaseExists(claseId);
            var clase = await _claseQuery.GetClaseById(claseId);
            return await _claseMapper.GetClaseResponse(clase);
        }
        public virtual async Task<ClaseResponse> AddClaseAsync(ClaseRequest claseRequest)
        {
            await CheckClaseRequest(claseRequest);
            var clase = new Clase
            {
                ActividadId = claseRequest.ActividadId,
                EntrenadorId = claseRequest.EntrenadorId,
                Fecha = claseRequest.Fecha,
                HoraInicio = claseRequest.HoraInicio,
                HoraFin = claseRequest.HoraFin,
                Cupo = claseRequest.Cupo
            };
            var result = await _claseCommand.AddClase(clase);
            return await _claseMapper.GetClaseResponse(await _claseQuery.GetClaseById(result.ClaseId));

        }
        public virtual async Task<ClaseResponse> UpdateClase(int claseId, ClaseRequest claseRequest)
        {
            await ClaseExists(claseId);
            await CheckClaseRequest(claseRequest);

            var clase = await _claseQuery.GetClaseById(claseId);
            clase.ActividadId = claseRequest.ActividadId;
            clase.EntrenadorId = claseRequest.EntrenadorId;
            clase.Fecha = claseRequest.Fecha;
            clase.HoraInicio = claseRequest.HoraInicio;
            clase.HoraFin = claseRequest.HoraFin;
            clase.Cupo = claseRequest.Cupo;
            var result = await _claseCommand.UpdateClase(clase);
            return await _claseMapper.GetClaseResponse(await _claseQuery.GetClaseById(result.ClaseId));
        }
        public virtual async Task<ClaseResponse> DeleteClase(int claseId)
        {
            var clase = await _claseQuery.GetClaseById(claseId);
            await _claseCommand.DeleteClase(clase);
            return await _claseMapper.GetClaseResponse(clase);
        }

        private async Task<bool> ClaseExists(int claseId)
        {
            if (claseId <= 0 || await _claseQuery.GetClaseById(claseId) == null)
                throw new NotFoundException("Id de clase invalido");
            return true;
        }
        private async Task<bool> CheckClaseRequest(ClaseRequest request)
        {
            if (await _entrenadorService.GetEntrenadorById(request.EntrenadorId)==null)
            {
                throw new BadRequestException("Datos de Entrenador invalidos");
            }
            if (await _actividadService.GetActividadById(request.ActividadId)==null)
            {
                throw new BadRequestException("Datos de Actividad invalidos");
            }
            return true;
        }
    }
}
