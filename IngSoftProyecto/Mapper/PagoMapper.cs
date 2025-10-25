using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Mapper
{
    public class PagoMapper
    {
        virtual public Task<PagoResponse> GetPagoResponse(Pago pago )
        {
            PagoResponse response = new PagoResponse
            {
                PagoId = pago.PagoId,
                Monto = pago.Monto,
                FechaPago = pago.FechaPago,
                MetodoPago = pago.MetodoPago,
                DescuentoAplicado = pago.DescuentoAplicado,
            };
            return Task.FromResult(response);


        }
        virtual public Task<List<PagoResponse>> GetPagoResponseList(List<Pago> pagos)
        {
            List<PagoResponse> responseList = new List<PagoResponse>();
            foreach (var pago in pagos)
            {
                PagoResponse response = new PagoResponse
                {
                    PagoId = pago.PagoId,
                    Monto = pago.Monto,
                    FechaPago = pago.FechaPago,
                    MetodoPago = pago.MetodoPago,
                    DescuentoAplicado = pago.DescuentoAplicado,
                };
                responseList.Add(response);
            }
            return Task.FromResult(responseList);
        }
    }
}
