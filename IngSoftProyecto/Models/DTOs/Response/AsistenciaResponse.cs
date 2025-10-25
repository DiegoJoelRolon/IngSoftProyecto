namespace IngSoftProyecto.Models.DTOs.Response
{
    public class AsistenciaResponse
    {
        public int AsistenciaId { get; set; }
        public MiembroXClaseResponse MiembroXClase { get; set; }
        public MembresiaXMiembroResponse MembresiaXMiembro { get; set; }
        public GenericResponse TipoDeAsistencia { get; set; }
        public DateTime Fecha { get; set; }
    }
}
