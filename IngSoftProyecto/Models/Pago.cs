namespace IngSoftProyecto.Models
{
    public class Pago
    {
        public int PagoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public required string MetodoPago { get; set; }
        public required string DescuentoAplicado { get; set; }
        
        public List<MembresiaXMiembro> MembresiasXMiembro { get; set; }
    }
}
