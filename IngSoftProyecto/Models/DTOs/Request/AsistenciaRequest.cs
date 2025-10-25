namespace IngSoftProyecto.Models.DTOs.Request
{
    public class AsistenciaRequest
    {
        public int MiembroXClaseId { get; set; }
        
        public int MembresiaXMiembroId { get; set; }
        
        public int TipoDeAsistenciaId { get; set; }
        
        public DateTime Fecha { get; set; }

    }
}
