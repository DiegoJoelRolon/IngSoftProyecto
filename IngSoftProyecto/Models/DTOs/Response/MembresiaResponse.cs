namespace IngSoftProyecto.Models.DTOs.Response
{
    public class MembresiaResponse
    {
        public int MembresiaId { get; set; }
        public GenericResponse TipoDeMembresia{ get; set; }
        public int DuracionEnDias { get; set; }
        public decimal CostoBase { get; set; }

    }
}
