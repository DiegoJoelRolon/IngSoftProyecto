namespace IngSoftProyecto.Models.DTOs.Response
{
    public class PagoResponse
    {
        public int PagoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public required string MetodoPago { get; set; }
        public required int DescuentoAplicado { get; set; }

    }
}
