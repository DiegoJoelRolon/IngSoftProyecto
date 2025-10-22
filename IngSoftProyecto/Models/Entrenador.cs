namespace IngSoftProyecto.Models
{
    public class Entrenador:Persona
    {
        public required string Certificacion { get; set; }
        public required bool Activo { get; set; }
        public List<Clase> Clases { get; set; }
        public List<Miembro> Miembros { get; set; }
    }
}
