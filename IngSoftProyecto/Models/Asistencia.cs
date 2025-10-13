namespace IngSoftProyecto.Models
{
    public class Asistencia
    {
        public int AsistenciaId { get; set; }

        public int MiembroXClaseId { get; set; }
        public MiembroXClase MiembroXClase { get; set; }
        public int MembresiaXMiembroId { get; set; }
        public MembresiaXMiembro MembresiaXMiembro { get; set; }
        public int TipoDeAsistenciaId { get; set; }
        public TipoDeAsistencia TipoDeAsistencia { get; set; }
        public DateTime Fecha { get; set; }
    }
}
