namespace IngSoftProyecto.Models.DTOs.Response
{
    public class MembresiaXMiembroResponse
    {
        public int MembresiaXMiembroId { get; set; }
        public MiembroResponse Miembro { get; set; }
        
        public MembresiaResponse Membresia { get; set; }
        
        public GenericResponse EstadoMembresia { get; set; }
        
        public PagoResponse Pago { get; set; }
        public required DateTime FechaInicio { get; set; }
        public required DateTime FechaFin { get; set; }
    }
}
