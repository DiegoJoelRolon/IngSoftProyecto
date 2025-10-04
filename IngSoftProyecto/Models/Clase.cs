namespace IngSoftProyecto.Models
{
    public class Clase
    {
        public int ClaseId { get; set; }

        public int ActividadId { get; set; }
        public required Actividad Actividad { get; set; }
        public int EntrenadorId { get; set; }
        public required Entrenador Entrenador { get; set; }
        public DateTime Fecha { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public int Cupo { get; set; }

        public List<MiembroXClase>MiembrosXClases { get; set; }
    }
}
