namespace IngSoftProyecto.Models.DTOs.Response
{
    public class MiembroResponse
    {
        public required int TipoDeMiembroId { get; set; }
        public int? EntrenadorId { get; set; }
        public required string Nombre { get; set; }
        public required string Direccion { get; set; }
        public required string Telefono { get; set; }
        public required DateTime FechaNacimiento { get; set; }
        public required TipoDeMiembroResponse TipoDeMiembro { get; set; }
        public required string Email { get; set; }
        public required string Foto { get; set; }
        public bool Eliminado { get; set; }
    }
}
