namespace IngSoftProyecto.Models
{
    public class MiembroXClase
    {
        public int MiembroXClaseId { get; set; }
        public int MiembroId { get; set; }
        public Miembro Miembro { get; set; }
        public int ClaseId { get; set; }
        public Clase Clase { get; set; }
        public DateTime FechaInscripcion { get; set; }

        public List<Asistencia> Asistencias { get; set; }
    }
}
