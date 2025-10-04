namespace IngSoftProyecto.Models
{
    public class TipoDeMiembro
    {
        public int TipoDeMiembroId { get; set; }
        public required string Descripcion { get; set; }
        public required int PorcentajeDescuento { get; set; }

        public List<Miembro> Miembros { get; set; }
    }
}
