namespace IngSoftProyecto.Models
{
    public class Miembro
    {
        public int MiembroId { get; set; }
        public required int TipoDeMiembroId { get; set; }
        public TipoDeMiembro TipoDeMiembro { get; set; }
        public int? EntrenadorId { get; set; }
        public Entrenador? Entrenador { get; set; }
        public required string Nombre { get; set; }
        public required string Direccion { get; set; }
        public required string Telefono { get; set; }
        public required DateTime FechaNacimiento { get; set; }
        public required string Email { get; set; }
        public required string Foto { get; set; }
        public bool Eliminado { get; set; } = false;

        public List<MembresiaXMiembro> MembresiasXMiembros { get; set; }
        public List<MiembroXClase> MiembrosXClases { get; set; }
    }
}
