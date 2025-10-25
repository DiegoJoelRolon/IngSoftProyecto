using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Models;
using IngSoftProyecto.Exceptions;

namespace IngSoftProyecto.Services
{
    public class MembresiaXMiembroService
    {
        private readonly MembresiaXMiembroCommand _membresiaXMiembroCommand;
        private readonly MembresiaXMiembroQuery _membresiaXMiembroQuery;
        private readonly MembresiaXMiembroMapper _membresiaXMiembroMapper;
        private readonly MiembroService _miembroService;
        private readonly MembresiasService _membresiaService;
        private readonly EstadoMembresiaQuery _estadoMembresiaQuery;
        private readonly PagoService _pagoService;

        public MembresiaXMiembroService(MembresiaXMiembroCommand membresiaXMiembroCommand, MembresiaXMiembroQuery membresiaXMiembroQuery, MembresiaXMiembroMapper membresiaXMiembroMapper)
        {
            _membresiaXMiembroCommand = membresiaXMiembroCommand;
            _membresiaXMiembroQuery = membresiaXMiembroQuery;
            _membresiaXMiembroMapper = membresiaXMiembroMapper;
        }
        virtual public async Task<MembresiaXMiembroResponse> AddMembresiaXMiembro(MembresiaXMiembroRequest request)
        {
            await CheckMembresiaXMiembroRequest(request);
            var membresiaXMiembro = new MembresiaXMiembro
            {
                MiembroId = request.MiembroId,
                MembresiaId = request.MembresiaId,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                EstadoMembresiaId = request.EstadoMembresiaId,
                PagoId = request.PagoId,
            };
            var result = await _membresiaXMiembroCommand.AddMembresiaXMiembro(membresiaXMiembro);
            return await _membresiaXMiembroMapper.GetMembresiaXMiembroResponse(await _membresiaXMiembroQuery.GetMembresiaXMiembroById(result.MembresiaXMiembroId));
        }
        virtual public async Task<List<MembresiaXMiembroResponse>> GetAllMembresiasXMiembros()
        {
            var membresiasXMiembros = await _membresiaXMiembroQuery.GetAllMembresiasXMiembros();
            return await _membresiaXMiembroMapper.GetMembresiaXMiembroResponseList(membresiasXMiembros);
        }
        virtual public async Task<MembresiaXMiembroResponse> GetMembresiaXMiembroById(int membresiaXMiembroId)
        {
            await MembresiaXMiembroExists(membresiaXMiembroId);
            var membresiaXMiembro = await _membresiaXMiembroQuery.GetMembresiaXMiembroById(membresiaXMiembroId);
            return await _membresiaXMiembroMapper.GetMembresiaXMiembroResponse(membresiaXMiembro);
        }
        virtual public async Task<MembresiaXMiembroResponse> UpdateMembresiaXMiembro(int membresiaXMiembroId, MembresiaXMiembroRequest request)
        {
            await MembresiaXMiembroExists(membresiaXMiembroId);
            await CheckMembresiaXMiembroRequest(request);
            var membresiaXMiembro = await _membresiaXMiembroQuery.GetMembresiaXMiembroById(membresiaXMiembroId);
            membresiaXMiembro.MiembroId = request.MiembroId;
            membresiaXMiembro.MembresiaId = request.MembresiaId;
            membresiaXMiembro.FechaInicio = request.FechaInicio;
            membresiaXMiembro.FechaFin = request.FechaFin;
            membresiaXMiembro.EstadoMembresiaId = request.EstadoMembresiaId;
            membresiaXMiembro.PagoId = request.PagoId;
            var result = await _membresiaXMiembroCommand.UpdateMembresiaXMiembro(membresiaXMiembro);
            return await _membresiaXMiembroMapper.GetMembresiaXMiembroResponse(await _membresiaXMiembroQuery.GetMembresiaXMiembroById(result.MembresiaXMiembroId));
        }

        private async Task<bool> MembresiaXMiembroExists(int id)
        {
            if (id <= 0 || await _membresiaXMiembroQuery.GetMembresiaXMiembroById(id) == null)
                throw new NotFoundException("Id de MembresiaXMiembro invalido");
            return true;
        }
        private async Task<bool> CheckMembresiaXMiembroRequest(MembresiaXMiembroRequest request)
        {
            if (_miembroService.GetMiembroById(request.MiembroId)==null)
            {
                throw new BadRequestException("Id invalido en la solicitud de Miembro");
            }
            if (_membresiaService.GetMembresiaById(request.MembresiaId)==null)
            {
                throw new BadRequestException("Id invalido en la solicitud de Membresia");
            }
            if (await _estadoMembresiaQuery.GetEstadoMembresiaById(request.EstadoMembresiaId)==null)
            {
                throw new BadRequestException("Id invalido en la solicitud de EstadoMembresia");
            }
            if (_pagoService.GetPagoById(request.PagoId)==null)
            {
                throw new BadRequestException("Id invalido en la solicitud de Pago");
            }
            if (request.FechaFin <= request.FechaInicio)
            {
                throw new BadRequestException("Fechas invalidas en la solicitud de MembresiaXMiembro");
            }

            return true;
        }
    }
}
