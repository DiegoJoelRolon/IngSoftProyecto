namespace IngSoftProyecto.Models
{
    public class Persona
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required int DNI { get; set; }
        public required DateTime FechaNacimiento { get; set; }
        public required string Telefono { get; set; }
        public required string Direccion { get; set; }
        public required string Email { get; set; }
        public required string Foto { get; set; }
        public bool Eliminado { get; set; } = false;
    }
}
