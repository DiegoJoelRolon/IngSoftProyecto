namespace IngSoftProyecto.Models.DTOs.Response
{
    public class EntrenadorResponse
    {
        public required int Id { get; set; }
        public required string Nombre { get; set; }
        public required int DNI { get; set; }
        public required DateTime FechaNacimiento { get; set; }
        public required string Telefono { get; set; }
        public required string Direccion { get; set; }
        public required string Email { get; set; }
        public required string Foto { get; set; }
        public bool Eliminado { get; set; } = false;

        public required string Certificacion { get; set; }
        public required bool Activo { get; set; }
    }
}
