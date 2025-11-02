using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Services
{
    public class MembresiasService
    {
        private readonly MembresiaQuery _query;
        private readonly MembresiaCommand _command;
        private readonly TipoDeMembresiaService _tipoDeMembresiaService;
        private readonly MembresiaMapper _membresiaMapper;
        private readonly MiembroQuery _miembroQuery;

        public MembresiasService(
            MembresiaQuery query,
            MembresiaCommand command,
            TipoDeMembresiaService tipoDeMembresiaService,
            MembresiaMapper membresiaMapper,
            MiembroQuery miembroQuery) 
        {
            _query = query;
            _command = command;
            _tipoDeMembresiaService = tipoDeMembresiaService;
            _membresiaMapper = membresiaMapper;
            _miembroQuery = miembroQuery;
        }

        
        public virtual async Task<List<MembresiaResponse>> GetAllMembresias()
        {
            var membresias = await _query.GetAllMembresias();
            return await _membresiaMapper.GetAllMembresiasResponse(membresias);
        }

        public virtual async Task<MembresiaResponse?> GetMembresiaById(int id)
        {
            await MembresiaExists(id);
            var membresia = await _query.GetMembresiaById(id);
            if (membresia == null)
            {
                return null;
            }
            return await _membresiaMapper.GetMembresiaResponse(membresia);
        }

        
        public virtual async Task<MembresiaResponse> AddMembresia(MembresiaRequest request)
        {
            await CheckMembresiaRequest(request);
            var membresia = new Membresia
            {
                TipoDeMembresiaId = request.TipoDeMembresiaId,
                DuracionEnDias = request.DuracionEnDias,
                CostoBase = request.CostoBase
            };

            var result = await _command.AddMembresia(membresia);
            return await _membresiaMapper.GetMembresiaResponse(membresia);
        }

        public virtual async Task<MembresiaResponse> UpdateMembresia(int id, MembresiaRequest request)
        {
        
            await MembresiaExists(id);
            await CheckMembresiaRequest(request);

        
            var membresia = await _query.GetMembresiaById(id);
            
            // Actualizar propiedades
            membresia.TipoDeMembresiaId = request.TipoDeMembresiaId;
            membresia.DuracionEnDias = request.DuracionEnDias;
            membresia.CostoBase = request.CostoBase;

            // 3. Ejecutar el comando
            var result = await _command.UpdateMembresia(membresia);
            return await _membresiaMapper.GetMembresiaResponse(result);
        }

        // 🆕 Método de Eliminación (Delete Command)
        public virtual async Task<bool> DeleteMembresia(int id)
        {
            // 1. Validar existencia
            await MembresiaExists(id);

            // 2. Validar integridad referencial: No eliminar si hay miembros usándola
            await CheckIfMembresiaIsUsed(id);

            // 3. Obtener el objeto (o solo el ID, dependiendo de tu Command)
            var membresiaToDelete = await _query.GetMembresiaById(id);

            // 4. Ejecutar el comando de eliminación
            await _command.DeleteMembresia(membresiaToDelete);
            
            return true;
        }

        // --- Validaciones (Privadas) ---

        // 🔒 Valida que la membresía (el tipo) exista por su ID.
        private async Task<bool> MembresiaExists(int id)
        {
            if (id <= 0 || await _query.GetMembresiaById(id) == null)
                throw new NotFoundException("Id de membresia invalido.");
            return true;
        }

        // 🎯 Valida los datos del Request antes de agregar/actualizar.
        private async Task<bool> CheckMembresiaRequest(MembresiaRequest request)
        {
            if (request == null)
                throw new BadRequestException("Datos de membresia invalidos.");

            // 1. Validar existencia del Tipo de Membresía
            var tipo = await _tipoDeMembresiaService.GetTipoDeMembresiaById(request.TipoDeMembresiaId);
            if (tipo == null)
                throw new NotFoundException("Tipo de membresia no encontrado.");

            // 2. Validación de lógica de negocio para Duración
            if (request.DuracionEnDias <= 0)
                throw new BadRequestException("DuracionEnDias invalida: debe ser mayor a 0.");

            // 3. Validación de lógica de negocio para Costo
            if (request.CostoBase <= 0)
                throw new BadRequestException("CostoBase invalido: debe ser mayor a 0.");

            return true;
        }

        // 🔑 🆕 Validación de Integridad Referencial
        private async Task CheckIfMembresiaIsUsed(int membresiaId)
        {
            // La implementación real debe llamar a tu capa de datos (MiembroQuery) 
            // para verificar si algún miembro tiene este TipoDeMembresiaId asignado.

            // Ejemplo conceptual:
            bool isUsed = await _miembroQuery.CheckIfTipoMembresiaIsAssigned(membresiaId);

            if (isUsed)
            {
                throw new BadRequestException("No se puede eliminar este tipo de membresía porque está siendo utilizada por miembros activos o históricos.");
            }
        }
    }
}