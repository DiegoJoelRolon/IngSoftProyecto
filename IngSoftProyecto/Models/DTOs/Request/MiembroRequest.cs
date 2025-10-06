namespace IngSoftProyecto.Models.DTOs.Request
{
    public class MiembroRequest
    {
        public required int TipoDeMiembroId { get; set; }
        public int? EntrenadorId { get; set; }
        public required string Nombre { get; set; }
        public required string Direccion { get; set; }
        public required string Telefono { get; set; }
        public required DateTime FechaNacimiento { get; set; }
        public required string Email { get; set; }
        public required string Foto { get; set; }
    }
}
