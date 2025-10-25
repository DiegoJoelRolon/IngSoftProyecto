namespace IngSoftProyecto.Models.DTOs.Request
{
    public class PagoRequest
    {
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public required string MetodoPago { get; set; }
        public required int DescuentoAplicado { get; set; }
    }
}
