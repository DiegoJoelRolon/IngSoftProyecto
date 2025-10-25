namespace IngSoftProyecto.Models.DTOs.Request
{
    public class MiembroXClaseRequest
    {
        public int MiembroId { get; set; }
        public int ClaseId { get; set; }
        public DateTime FechaInscripcion { get; set; }
    }
}
