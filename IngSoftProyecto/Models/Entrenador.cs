namespace IngSoftProyecto.Models
{
    public class Entrenador
    {
        public int EntrenadorId { get; set; }
        public required string Nombre { get; set; }
        public required string Telefono { get; set; }
        public required string Email { get; set; }
        public required string Certificacion { get; set; }
        public required bool Activo { get; set; }

        public List<Clase> Clases { get; set; }
        public List<Miembro> Miembros { get; set; }
    }
}
