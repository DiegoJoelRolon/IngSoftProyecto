using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models;

namespace IngSoftProyecto.Services
{
    public class EntrenadorService
    {
        private readonly EntrenadorQuery _entrenadorQuery;
        private readonly EntrenadorCommand _entrenadorCommand;
        private readonly EntrenadorMapper _entrenadorMapper;

        public EntrenadorService(EntrenadorQuery entrenadorQuery, EntrenadorCommand entrenadorCommand, EntrenadorMapper entrenadorMapper)
        {
            _entrenadorQuery = entrenadorQuery;
            _entrenadorCommand = entrenadorCommand;
            _entrenadorMapper = entrenadorMapper;
        }   
        public virtual async Task<List<EntrenadorResponse>> GetAllEntrenadores()
        {
            return await _entrenadorMapper.GetAllEntrenadoresResponse((await _entrenadorQuery.GetAllEntrenadores()));
        }
        public virtual async Task<EntrenadorResponse?> GetEntrenadorById(int entrenadorId)
        {
            var entrenador = await _entrenadorQuery.GetEntrenadorById(entrenadorId);
            return await _entrenadorMapper.GetEntrenadorResponse(entrenador);
        }
        public virtual async Task<EntrenadorResponse> AddEntrenadorAsync(EntrenadorRequest entrenadorRequest)
        {
            var entrenador = new Entrenador
            {
                Nombre = entrenadorRequest.Nombre,
                DNI = entrenadorRequest.DNI,
                FechaNacimiento = entrenadorRequest.FechaNacimiento,
                Telefono = entrenadorRequest.Telefono,
                Direccion = entrenadorRequest.Direccion,
                Email = entrenadorRequest.Email,
                Foto = entrenadorRequest.Foto,
                Eliminado = entrenadorRequest.Eliminado,
                Certificacion = entrenadorRequest.Certificacion,
                Activo = entrenadorRequest.Activo
            };
            var result = await _entrenadorCommand.AddEntrenadorAsync(entrenador);
            return await _entrenadorMapper.GetEntrenadorResponse(await _entrenadorQuery.GetEntrenadorById(result.Id));
        }
        public virtual async Task<EntrenadorResponse> UpdateEntrenadorAsync(int entrenadorId, EntrenadorRequest entrenadorRequest)
        {
            var entrenador = new Entrenador
            {
                Id = entrenadorId,
                Nombre = entrenadorRequest.Nombre,
                DNI = entrenadorRequest.DNI,
                FechaNacimiento = entrenadorRequest.FechaNacimiento,
                Telefono = entrenadorRequest.Telefono,
                Direccion = entrenadorRequest.Direccion,
                Email = entrenadorRequest.Email,
                Foto = entrenadorRequest.Foto,
                Eliminado = entrenadorRequest.Eliminado,
                Certificacion = entrenadorRequest.Certificacion,
                Activo = entrenadorRequest.Activo
            };
            var result = await _entrenadorCommand.UpdateEntrenadorAsync(entrenador);
            return await _entrenadorMapper.GetEntrenadorResponse(result);
        }

    }
}
