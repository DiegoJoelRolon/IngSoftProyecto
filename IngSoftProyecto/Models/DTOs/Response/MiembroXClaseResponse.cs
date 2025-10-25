namespace IngSoftProyecto.Models.DTOs.Response
{
    public class MiembroXClaseResponse
    {
        public int MiembroXClaseId { get; set; }
        public MiembroResponse Miembro { get; set; }
        public ClaseResponse Clase { get; set; }
        public DateTime FechaInscripcion { get; set; }
    }
}
