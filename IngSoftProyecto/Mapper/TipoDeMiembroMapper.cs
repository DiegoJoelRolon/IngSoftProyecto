using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Mapper
{
    public class TipoDeMiembroMapper
    {
        public Task<TipoDeMiembroResponse> GetTipoDeMiembroResponse(TipoDeMiembro tipoDeMiembro)
        {
            TipoDeMiembroResponse response = new TipoDeMiembroResponse
            {
                Descripcion = tipoDeMiembro.Descripcion,
                PorcentajeDescuento = tipoDeMiembro.PorcentajeDescuento
            };
            return Task.FromResult(response);
        }
    }
}
