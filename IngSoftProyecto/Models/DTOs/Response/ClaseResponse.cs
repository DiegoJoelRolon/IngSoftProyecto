namespace IngSoftProyecto.Models.DTOs.Response
{
    public class ClaseResponse
    {
        public int ClaseId { get; set; }
        public ActividadResponse Actividad { get; set; }
        public EntrenadorResponse Entrenador { get; set; }
        public DateTime Fecha { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public int Cupo { get; set; }

    }
}
