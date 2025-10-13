namespace IngSoftProyecto.Models
{
    public class TipoDeMembresia
    {
        public int TipoDeMembresiaId { get; set; }
        public required string Descripcion { get; set; }

        public List<Membresia> Membresias { get; set; }
    }
}
