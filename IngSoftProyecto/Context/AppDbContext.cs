using IngSoftProyecto.Models;
using Microsoft.EntityFrameworkCore;

namespace IngSoftProyecto.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Clase> Clases { get; set; }
        public DbSet<Entrenador> Entrenadores { get; set; }
        public DbSet<EstadoMembresia> EstadosMembresias { get; set; }
        public DbSet<Membresia> Membresias { get; set; }
        public DbSet<MembresiaXMiembro> MembresiasXMiembros { get; set; }
        public DbSet<Miembro> Miembros { get; set; }
        public DbSet<MiembroXClase> MiembrosXClases { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<TipoDeAsistencia> TiposDeAsistencias { get; set; }
        public DbSet<TipoDeMembresia> TiposDeMembresias { get; set; }
        public DbSet<TipoDeMiembro> TiposDeMiembros { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            modelBuilder.Entity<Actividad>(builder =>
            {
                builder.ToTable("Actividad");
                builder.HasKey(a => a.ActividadId);
                builder.Property(a => a.ActividadId).ValueGeneratedOnAdd();

                builder.Property(a => a.Nombre).IsRequired().HasMaxLength(100);
                builder.Property(a => a.Descripcion).HasMaxLength(500);

                builder.HasMany(a => a.Clases).WithOne(c => c.Actividad);

            });
            modelBuilder.Entity<Asistencia>(builder =>
            {
                builder.ToTable("Asistencia");
                builder.HasKey(a => a.AsistenciaId);
                builder.Property(a => a.AsistenciaId).ValueGeneratedOnAdd();
                builder.Property(a => a.Fecha).IsRequired();

                builder.HasOne(a => a.TipoDeAsistencia).WithMany(t => t.Asistencias).HasForeignKey(a => a.TipoDeAsistenciaId);
                builder.HasOne(a => a.MembresiaXMiembro).WithMany(m => m.Asistencias).HasForeignKey(a => a.MembresiaXMiembroId);
                builder.HasOne(a => a.MiembroXClase).WithMany(c => c.Asistencias).HasForeignKey(a => a.MiembroXClaseId);
            });
            modelBuilder.Entity<Clase>(builder =>
            {
                builder.ToTable("Clase");
                builder.HasKey(c => c.ClaseId);
                builder.Property(c => c.ClaseId).ValueGeneratedOnAdd();
                builder.HasOne(c => c.Actividad).WithMany(a => a.Clases).HasForeignKey(c => c.ActividadId);
                builder.HasOne(c => c.Entrenador).WithMany(e => e.Clases).HasForeignKey(c => c.EntrenadorId);
                builder.HasMany(c => c.MiembrosXClases).WithOne(mc => mc.Clase);
            });
            modelBuilder.Entity<Entrenador>(builder =>
            {
                builder.ToTable("Entrenador");
                builder.HasKey(e => e.EntrenadorId);
                builder.Property(e => e.EntrenadorId).ValueGeneratedOnAdd();
                builder.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                builder.Property(e => e.Telefono).HasMaxLength(15);
                builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
                builder.HasMany(e => e.Clases).WithOne(c => c.Entrenador);
                builder.HasMany(e => e.Miembros).WithOne(m => m.Entrenador);
            });
            modelBuilder.Entity<EstadoMembresia>(builder =>
            {
                builder.ToTable("EstadoMembresia");
                builder.HasKey(em => em.EstadoMembresiaId);
                builder.Property(em => em.EstadoMembresiaId).ValueGeneratedOnAdd();
                builder.HasMany(em => em.MembresiasXMiembro).WithOne(m => m.EstadoMembresia);
            });
            modelBuilder.Entity<Membresia>(builder =>
            {
                builder.ToTable("Membresia");
                builder.HasKey(m => m.MembresiaId);
                builder.Property(m => m.MembresiaId).ValueGeneratedOnAdd();
                builder.Property(m => m.DuracionEnDias).IsRequired();
                builder.Property(m => m.CostoBase).IsRequired().HasColumnType("decimal(18,2)");
                builder.HasOne(m => m.TipoDeMembresia).WithMany(t => t.Membresias).HasForeignKey(m => m.TipoDeMembresiaId);
                builder.HasMany(m => m.MembresiasXMiembro).WithOne(mxm => mxm.Membresia);
            });
            modelBuilder.Entity<MembresiaXMiembro>(builder =>
            {
                builder.ToTable("MembresiaXMiembro");
                builder.HasKey(mxm => mxm.MembresiaXMiembroId);
                builder.Property(mxm => mxm.MembresiaXMiembroId).ValueGeneratedOnAdd();
                builder.Property(mxm => mxm.FechaInicio).IsRequired();
                builder.Property(mxm => mxm.FechaFin).IsRequired();

                builder.HasOne(mxm => mxm.Miembro).WithMany(m => m.MembresiasXMiembros).HasForeignKey(mxm => mxm.MiembroId);
                builder.HasOne(mxm => mxm.Membresia).WithMany(m => m.MembresiasXMiembro).HasForeignKey(mxm => mxm.MembresiaId);
                builder.HasOne(mxm => mxm.EstadoMembresia).WithMany(em => em.MembresiasXMiembro).HasForeignKey(mxm => mxm.EstadoMembresiaId);
                builder.HasOne(mxm => mxm.Pago).WithMany(p => p.MembresiasXMiembro).HasForeignKey(mxm => mxm.PagoId);
                builder.HasMany(mxm => mxm.Asistencias).WithOne(a => a.MembresiaXMiembro);
            });
            modelBuilder.Entity<Miembro>(builder =>
            {
                builder.ToTable("Miembro");
                builder.HasKey(m => m.MiembroId);
                builder.Property(m => m.MiembroId).ValueGeneratedOnAdd();
                builder.Property(m => m.Nombre).IsRequired().HasMaxLength(100);
                builder.Property(m => m.Direccion).HasMaxLength(200);
                builder.Property(m => m.Telefono).HasMaxLength(15);
                builder.Property(m => m.Email).IsRequired().HasMaxLength(100);
                builder.Property(m => m.Foto).HasMaxLength(200);
                builder.HasOne(m => m.TipoDeMiembro).WithMany(t => t.Miembros).HasForeignKey(m => m.TipoDeMiembroId);
                builder.HasOne(m => m.Entrenador).WithMany(e => e.Miembros).HasForeignKey(m => m.EntrenadorId);
                builder.HasMany(m => m.MembresiasXMiembros).WithOne(mxm => mxm.Miembro);

            });
            modelBuilder.Entity<MiembroXClase>(builder =>
            {
                builder.ToTable("MiembroXClase");
                builder.HasKey(mc => mc.MiembroXClaseId);
                builder.Property(mc => mc.MiembroXClaseId).ValueGeneratedOnAdd();
                builder.HasOne(mc => mc.Miembro).WithMany(m => m.MiembrosXClases).HasForeignKey(mc => mc.MiembroId);
                builder.HasOne(mc => mc.Clase).WithMany(c => c.MiembrosXClases).HasForeignKey(mc => mc.ClaseId);
                builder.HasMany(mc => mc.Asistencias).WithOne(a => a.MiembroXClase);
            });
            modelBuilder.Entity<Pago>(builder =>
            {
                builder.ToTable("Pago");
                builder.HasKey(p => p.PagoId);
                builder.Property(p => p.PagoId).ValueGeneratedOnAdd();
                builder.Property(p => p.Monto).IsRequired().HasColumnType("decimal(18,2)");
                builder.Property(p => p.FechaPago).IsRequired();

                builder.HasMany(p => p.MembresiasXMiembro).WithOne(mxm => mxm.Pago);

            });
            modelBuilder.Entity<TipoDeAsistencia>(builder =>
            {
                builder.ToTable("TipoDeAsistencia");
                builder.HasKey(ta => ta.TipoDeAsistenciaId);
                builder.Property(ta => ta.TipoDeAsistenciaId).ValueGeneratedOnAdd();

                builder.HasMany(ta => ta.Asistencias).WithOne(a => a.TipoDeAsistencia);
            });
            modelBuilder.Entity<TipoDeMembresia>(builder =>
            {
                builder.ToTable("TipoDeMembresia");
                builder.HasKey(tm => tm.TipoDeMembresiaId);
                builder.Property(tm => tm.TipoDeMembresiaId).ValueGeneratedOnAdd();

                builder.HasMany(tm => tm.Membresias).WithOne(m => m.TipoDeMembresia);
            });
            modelBuilder.Entity<TipoDeMiembro>(builder =>
            {
                builder.ToTable("TipoDeMiembro");
                builder.HasKey(tm => tm.TipoDeMiembroId);
                builder.Property(tm => tm.TipoDeMiembroId).ValueGeneratedOnAdd();

                builder.HasMany(tm => tm.Miembros).WithOne(m => m.TipoDeMiembro);
            });
            */

            modelBuilder.Entity<TipoDeMiembro>().HasData(
                new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "Regular", PorcentajeDescuento = 0 },
                new TipoDeMiembro { TipoDeMiembroId = 2, Descripcion = "Estudiante", PorcentajeDescuento = 10 },
                new TipoDeMiembro { TipoDeMiembroId = 3, Descripcion = "Mayor", PorcentajeDescuento = 20 }
            );
            modelBuilder.Entity<EstadoMembresia>().HasData(
                new EstadoMembresia { EstadoMembresiaId = 1, Descripcion = "Activa" },
                new EstadoMembresia { EstadoMembresiaId = 2, Descripcion = "Expirada" },
                new EstadoMembresia { EstadoMembresiaId = 3, Descripcion = "Cancelada" }
            );
            modelBuilder.Entity<TipoDeAsistencia>().HasData(
                new TipoDeAsistencia { TipoDeAsistenciaId = 1, Descripcion = "Gimnasio" },
                new TipoDeAsistencia { TipoDeAsistenciaId = 2, Descripcion = "Clase" }
            );
        }
    }
}
