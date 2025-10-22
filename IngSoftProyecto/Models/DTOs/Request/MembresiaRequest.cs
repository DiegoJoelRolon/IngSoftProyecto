namespace IngSoftProyecto.Models.DTOs.Request
{
    public class MembresiaRequest
    {
        public int TipoDeMembresiaId { get; set; }
        public string DuracionEnDias { get; set; }
        public decimal CostoBase { get; set; }

    }
}
