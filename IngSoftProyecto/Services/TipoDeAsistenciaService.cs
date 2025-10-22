using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Services
{
    public class TipoDeAsistenciaService
    {
        private readonly TipoDeAsistenciaCommand _tipoDeAsistenciaCommand;
        private readonly TipoDeAsistenciaQuery _tipoDeAsistenciaQuery;
        private readonly GenericMapper _genericMapper;
        public TipoDeAsistenciaService(TipoDeAsistenciaCommand tipoDeAsistenciaCommand, TipoDeAsistenciaQuery tipoDeAsistenciaQuery, GenericMapper genericMapper)
        {
            _tipoDeAsistenciaCommand = tipoDeAsistenciaCommand;
            _tipoDeAsistenciaQuery = tipoDeAsistenciaQuery;
            _genericMapper = genericMapper;
            
        }
        virtual public async Task<List<GenericResponse>> GetAllTipoDeAsistencia()
        {
            var tiposDeAsistencias =  await _tipoDeAsistenciaQuery.GetAllTipoDeAsistencia();
            return await _genericMapper.GetAllGenericResponse(tiposDeAsistencias);
        }
        virtual public async Task<GenericResponse?> GetTipoDeAsistenciaById(int id)
        {
            var tipoDeAsistencia = await _tipoDeAsistenciaQuery.GetTipoDeAsistenciaById(id);
            return  await _genericMapper.GetGenericResponse(tipoDeAsistencia);
        }
        virtual public async Task<GenericResponse> AddTipoDeAsistencia(TipoDeAsistencia tipoDeAsistencia)
        {
           var response = await _tipoDeAsistenciaCommand.AddTipoDeAsistencia(tipoDeAsistencia);
           return await _genericMapper.GetGenericResponse(response);
        }
        virtual public async Task<GenericResponse?> UpdateTipoDeAsistencia(int id, TipoDeAsistencia tipoDeAsistencia)
        {
            var response = await _tipoDeAsistenciaCommand.UpdateTipoDeAsistencia(id, tipoDeAsistencia);
            if (response == null)
            {
                return null;
            }
            return await _genericMapper.GetGenericResponse(response);
        }

    }
}
