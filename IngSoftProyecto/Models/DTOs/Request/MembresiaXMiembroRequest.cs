namespace IngSoftProyecto.Models.DTOs.Request
{
    public class MembresiaXMiembroRequest
    {
        
        public required int MiembroId { get; set; }
        
        public required int MembresiaId { get; set; }
        
        public required int EstadoMembresiaId { get; set; }
        
        public required int PagoId { get; set; }
        
        public required DateTime FechaInicio { get; set; }
        public required DateTime FechaFin { get; set; }
    }
}
