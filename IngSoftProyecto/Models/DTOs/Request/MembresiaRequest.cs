namespace IngSoftProyecto.Models.DTOs.Request
{
    public class MembresiaRequest
    {
        public int TipoDeMembresiaId { get; set; }
        public int DuracionEnDias { get; set; }
        public decimal CostoBase { get; set; }

    }
}
