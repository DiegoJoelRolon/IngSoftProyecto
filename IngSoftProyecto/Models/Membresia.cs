namespace IngSoftProyecto.Models
{
    public class Membresia
    {
        public int MembresiaId { get; set; }
        public int TipoDeMembresiaId { get; set; }
        public TipoDeMembresia TipoDeMembresia { get; set; }
        public int DuracionEnDias { get; set; }
        public decimal CostoBase { get; set; }

        public List<MembresiaXMiembro> MembresiasXMiembro { get; set; }

    }
}
