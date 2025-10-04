namespace IngSoftProyecto.Models
{
    public class TipoDeAsistencia
    {
        public int TipoDeAsistenciaId { get; set; }
        public required string Descripcion { get; set; }

        public List<Asistencia> Asistencias { get; set; }
    }
}
