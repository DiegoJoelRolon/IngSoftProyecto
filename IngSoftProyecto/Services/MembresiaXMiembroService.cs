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

        public MembresiaXMiembroService(
            MembresiaXMiembroCommand membresiaXMiembroCommand,
            MembresiaXMiembroQuery membresiaXMiembroQuery,
            MembresiaXMiembroMapper membresiaXMiembroMapper,
            MiembroService miembroService,
            MembresiasService membresiaService,
            EstadoMembresiaQuery estadoMembresiaQuery,
            PagoService pagoService)
        {
            _membresiaXMiembroCommand = membresiaXMiembroCommand;
            _membresiaXMiembroQuery = membresiaXMiembroQuery;
            _membresiaXMiembroMapper = membresiaXMiembroMapper;
            _miembroService = miembroService;
            _membresiaService = membresiaService;
            _estadoMembresiaQuery = estadoMembresiaQuery;
            _pagoService = pagoService;
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

            if (await _miembroService.GetMiembroById(request.MiembroId)==null)
            {
                throw new BadRequestException("Id invalido en la solicitud de Miembro");
            }
            if (await _membresiaService.GetMembresiaById(request.MembresiaId)==null)
            {
                throw new BadRequestException("Id invalido en la solicitud de Membresia");
            }
            if (await _estadoMembresiaQuery.GetEstadoMembresiaById(request.EstadoMembresiaId)==null)
            {
                throw new BadRequestException("Id invalido en la solicitud de EstadoMembresia");
            }
            if (await _pagoService.GetPagoById(request.PagoId)==null)
            {
                throw new BadRequestException("Id invalido en la solicitud de Pago");
            }
            if (request.FechaFin <= request.FechaInicio)
            {
                throw new BadRequestException("Fechas invalidas en la solicitud de MembresiaXMiembro");
            }

            if (request.PagoId == 0) 
            {
                throw new BadRequestException("El PagoId es obligatorio. La membresía debe pagarse por adelantado.");
            }

            var pago = await _pagoService.GetPagoById(request.PagoId);
            if (pago == null)
            {
                throw new NotFoundException("Pago no encontrado.");
            }
            if (pago.FechaPago > request.FechaInicio)
            {
                throw new BadRequestException("El pago debe realizarse por adelantado. La fecha de pago no puede ser posterior a la fecha de inicio de la membresía.");
            }

            if (request.FechaFin <= request.FechaInicio)
            {
                throw new BadRequestException("La FechaFin debe ser posterior a la FechaInicio.");
            }

            var membresia = await _membresiaService.GetMembresiaById(request.MembresiaId);
            TimeSpan duracionReal = request.FechaFin - request.FechaInicio;

            if (duracionReal.TotalDays != membresia.DuracionEnDias)
            {
                throw new BadRequestException($"Duración incorrecta. La membresía seleccionada dura {membresia.DuracionEnDias} días.");
            }
            var montoEsperado = membresia.CostoBase * (1 - (pago.DescuentoAplicado / 100m));

            if (Math.Abs(pago.Monto - montoEsperado) > 0.01m) // Usar un margen de error para decimales
            {
                throw new BadRequestException("El monto del pago no coincide con el costo de la membresía con el descuento aplicado.");
            }

            var ultimaMembresia = await _membresiaXMiembroQuery.GetUltimaMembresiaActiva(request.MiembroId);

            if (ultimaMembresia != null && ultimaMembresia.FechaFin >= request.FechaInicio)
            {
                throw new BadRequestException("El miembro ya tiene una membresía activa o superpuesta.");
            }

            return true;
        }
    }
}
