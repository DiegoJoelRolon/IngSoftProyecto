namespace IngSoftProyecto.Models.DTOs.Request
{
    public class ClaseRequest
    {
        public int ActividadId { get; set; }
        public int EntrenadorId { get; set; }
        public DateTime Fecha { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public int Cupo { get; set; }

    }
}
