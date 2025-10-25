using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models;
using IngSoftProyecto.Exceptions;

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
            await EntrenadorExists(entrenadorId);
            var entrenador = await _entrenadorQuery.GetEntrenadorById(entrenadorId);
            return await _entrenadorMapper.GetEntrenadorResponse(entrenador);
        }
        public virtual async Task<EntrenadorResponse> AddEntrenadorAsync(EntrenadorRequest entrenadorRequest)
        {
            await CheckEntrenadorRequest(entrenadorRequest);
            var entrenador = new Entrenador
            {
                Nombre = entrenadorRequest.Nombre,
                DNI = entrenadorRequest.DNI,
                FechaNacimiento = entrenadorRequest.FechaNacimiento,
                Telefono = entrenadorRequest.Telefono,
                Direccion = entrenadorRequest.Direccion,
                Email = entrenadorRequest.Email,
                Foto = entrenadorRequest.Foto,
                Eliminado = false,
                Certificacion = entrenadorRequest.Certificacion,
                Activo = entrenadorRequest.Activo
            };
            var result = await _entrenadorCommand.AddEntrenadorAsync(entrenador);
            return await _entrenadorMapper.GetEntrenadorResponse(await _entrenadorQuery.GetEntrenadorById(result.Id));
        }
        public virtual async Task<EntrenadorResponse> UpdateEntrenadorAsync(int entrenadorId, EntrenadorRequest entrenadorRequest)
        {
            await EntrenadorExists(entrenadorId);
            await CheckEntrenadorRequest(entrenadorRequest);
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
                Certificacion = entrenadorRequest.Certificacion,
                Activo = entrenadorRequest.Activo
            };
            var result = await _entrenadorCommand.UpdateEntrenadorAsync(entrenador);
            return await _entrenadorMapper.GetEntrenadorResponse(result);
        }
        public virtual async Task<EntrenadorResponse> DeleteEntrenadorAsync(int entrenadorId)
        {
            var entrenador = await _entrenadorQuery.GetEntrenadorById(entrenadorId);
            await _entrenadorCommand.DeleteEntrenadorAsync(entrenador);
            var entrenadorDeleted = await _entrenadorQuery.GetEntrenadorById(entrenadorId);
            return await _entrenadorMapper.GetEntrenadorResponse(entrenadorDeleted);
        }
        public virtual async Task<EntrenadorResponse> RestoreEntrenadorAsync(int entrenadorId)
        {
            await EntrenadorExists(entrenadorId);
            var entrenador = await _entrenadorQuery.GetEntrenadorById(entrenadorId);
            await _entrenadorCommand.RestoreEntrenadorAsync(entrenador);
            var entrenadorRestored = await _entrenadorQuery.GetEntrenadorById(entrenadorId);
            return await _entrenadorMapper.GetEntrenadorResponse(entrenadorRestored);
        }

        private async Task<bool> EntrenadorExists(int entrenadorId)
        {
            if (entrenadorId <= 0 || await _entrenadorQuery.GetEntrenadorById(entrenadorId) == null)
                throw new NotFoundException("Id de entrenador invalido");
            return true;
        }
        private async Task<bool> CheckEntrenadorRequest(EntrenadorRequest request)
        {
            string dniString = request.DNI.ToString();
            if (string.IsNullOrEmpty(request.Nombre) || string.IsNullOrEmpty(dniString) ||
                string.IsNullOrEmpty(request.Telefono) || string.IsNullOrEmpty(request.Direccion) ||
                string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Certificacion))
            {
                throw new BadRequestException("Datos de entrenador invalidos");
            }
            return true;
        }
    }
}
