using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models;
namespace IngSoftProyecto.Services
{
    public class ClaseService
    {
        private readonly ClaseQuery _claseQuery;
        private readonly ClaseCommand _claseCommand;
        private readonly ClaseMapper _claseMapper;
        public ClaseService(ClaseQuery claseQuery, ClaseCommand claseCommand, ClaseMapper claseMapper)
        {
            _claseQuery = claseQuery;
            _claseMapper = claseMapper;
            _claseCommand = claseCommand;
        }

        public virtual async Task<List<ClaseResponse>> GetAllClases()
        {
            return await _claseMapper.GetAllClasesResponse((await _claseQuery.GetAllClases()));
        }
        public virtual async Task<ClaseResponse> GetClaseById(int claseId)
        {
            var clase = await _claseQuery.GetClaseById(claseId);
            return await _claseMapper.GetClaseResponse(clase);
        }
        public virtual async Task<ClaseResponse> AddClaseAsync(ClaseRequest claseRequest)
        {
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
        public virtual async Task<ClaseResponse>UpdateClase(int claseId, ClaseRequest claseRequest)
        {
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
    }
}
