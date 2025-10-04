namespace IngSoftProyecto.Models
{
    public class EstadoMembresia
    {
        public int EstadoMembresiaId { get; set; }
        public string Descripcion { get; set; }
        public List<MembresiaXMiembro> MembresiasXMiembro { get; set; }
    }
}
