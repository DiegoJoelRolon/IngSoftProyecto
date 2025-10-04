namespace IngSoftProyecto.Models
{
    public class Actividad
    {
        public int ActividadId { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }

        public List<Clase>Clases { get; set; }
    }
}
