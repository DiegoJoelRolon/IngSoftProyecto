namespace IngSoftProyecto.Models
{
    public class MembresiaXMiembro
    {
        public int MembresiaXMiembroId { get; set; }
        public required int MiembroId { get; set; }
        public required Miembro Miembro { get; set; }
        public required int MembresiaId { get; set; }
        public required Membresia Membresia { get; set; }
        public required int EstadoMembresiaId { get; set; }
        public required EstadoMembresia EstadoMembresia { get; set; }
        public required int PagoId { get; set; }
        public required Pago Pago { get; set; }
        public required DateTime FechaInicio { get; set; }
        public required DateTime FechaFin { get; set; }

        public List<Asistencia> Asistencias { get; set; }


    }
}
