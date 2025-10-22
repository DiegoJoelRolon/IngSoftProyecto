namespace IngSoftProyecto.Models
{
    public class Miembro:Persona
    {
        public int MiembroId { get; set; }
        public required int TipoDeMiembroId { get; set; }
        public TipoDeMiembro TipoDeMiembro { get; set; }
        public int? EntrenadorId { get; set; }
        public Entrenador? Entrenador { get; set; }
        public List<MembresiaXMiembro> MembresiasXMiembros { get; set; }
        public List<MiembroXClase> MiembrosXClases { get; set; }
    }
}
