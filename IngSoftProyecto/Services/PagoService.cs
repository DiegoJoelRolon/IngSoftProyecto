using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Services
{
    public class PagoService
    {
        private readonly PagoCommand _pagoCommand;
        private readonly PagoQuery _pagoQuery;
        private readonly PagoMapper _pagoMapper;
        public PagoService(PagoCommand pagoCommand, PagoQuery pagoQuery, PagoMapper pagoMapper)
        {
            _pagoCommand = pagoCommand;
            _pagoQuery = pagoQuery;
            _pagoMapper = pagoMapper;
        }   
        virtual public async Task<PagoResponse> AddPago(PagoRequest request)
        {
            await CheckPagoRequest(request);
            var pago = new Pago
            {
               DescuentoAplicado = request.DescuentoAplicado,
                FechaPago = request.FechaPago,
                MetodoPago = request.MetodoPago,
                Monto = request.Monto
            };
            var result = await _pagoCommand.AddPago(pago);
            return await _pagoMapper.GetPagoResponse(await _pagoQuery.GetPagoById(result.PagoId));
        }

        virtual public async Task<List<PagoResponse>> GetAllPagos()
        {
            var pagos = await _pagoQuery.GetAllPagos();
            return await _pagoMapper.GetPagoResponseList(pagos);
        }
        virtual public async Task<PagoResponse> GetPagoById(int pagoId)
        {
            await PagoExists(pagoId);
            var pago = await _pagoQuery.GetPagoById(pagoId);
            return await _pagoMapper.GetPagoResponse(pago);
        }
        virtual public async Task<PagoResponse> UpdatePago(int pagoId, PagoRequest request)
        {
            await PagoExists(pagoId);
            await CheckPagoRequest(request);
            var pago = await _pagoQuery.GetPagoById(pagoId);
            pago.DescuentoAplicado = request.DescuentoAplicado;
            pago.FechaPago = request.FechaPago;
            pago.MetodoPago = request.MetodoPago;
            pago.Monto = request.Monto;
            var result = await _pagoCommand.UpdatePago(pago);
            return await _pagoMapper.GetPagoResponse(await _pagoQuery.GetPagoById(result.PagoId));
        }
        
        private async Task PagoExists(int id)
        {
            var pago = await _pagoQuery.GetPagoById(id);
            if (pago == null)
            {
                throw new Exception($"El pago con ID {id} no existe.");
            }
        }
        private async Task<bool> CheckPagoRequest(PagoRequest request)
        {
            if (request.Monto <= 0)
            {
                throw new BadRequestException("El monto del pago debe ser mayor que cero.");
            }
            if (request.DescuentoAplicado>100 || request.DescuentoAplicado<0)
            {
                throw new BadRequestException("El descuento aplicado debe estar entre 0 y 100.");
            }
            return true;
        }
    }
}
