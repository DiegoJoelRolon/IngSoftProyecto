using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IngSoftProyecto.Migrations
{
    /// <inheritdoc />
    public partial class casifinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Actividad",
                columns: table => new
                {
                    ActividadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actividad", x => x.ActividadId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EstadoMembresia",
                columns: table => new
                {
                    EstadoMembresiaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoMembresia", x => x.EstadoMembresiaId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pago",
                columns: table => new
                {
                    PagoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MetodoPago = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DescuentoAplicado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pago", x => x.PagoId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Persona",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DNI = table.Column<int>(type: "int", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Direccion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Foto = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Eliminado = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipoDeAsistencia",
                columns: table => new
                {
                    TipoDeAsistenciaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDeAsistencia", x => x.TipoDeAsistenciaId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipoDeMembresia",
                columns: table => new
                {
                    TipoDeMembresiaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDeMembresia", x => x.TipoDeMembresiaId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipoDeMiembro",
                columns: table => new
                {
                    TipoDeMiembroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PorcentajeDescuento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDeMiembro", x => x.TipoDeMiembroId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Entrenador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Certificacion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrenador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entrenador_Persona_Id",
                        column: x => x.Id,
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Membresia",
                columns: table => new
                {
                    MembresiaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TipoDeMembresiaId = table.Column<int>(type: "int", nullable: false),
                    DuracionEnDias = table.Column<int>(type: "int", nullable: false),
                    CostoBase = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membresia", x => x.MembresiaId);
                    table.ForeignKey(
                        name: "FK_Membresia_TipoDeMembresia_TipoDeMembresiaId",
                        column: x => x.TipoDeMembresiaId,
                        principalTable: "TipoDeMembresia",
                        principalColumn: "TipoDeMembresiaId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Clase",
                columns: table => new
                {
                    ClaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActividadId = table.Column<int>(type: "int", nullable: false),
                    EntrenadorId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    HoraFin = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    Cupo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clase", x => x.ClaseId);
                    table.ForeignKey(
                        name: "FK_Clase_Actividad_ActividadId",
                        column: x => x.ActividadId,
                        principalTable: "Actividad",
                        principalColumn: "ActividadId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clase_Entrenador_EntrenadorId",
                        column: x => x.EntrenadorId,
                        principalTable: "Entrenador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Miembro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    TipoDeMiembroId = table.Column<int>(type: "int", nullable: false),
                    EntrenadorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Miembro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Miembro_Entrenador_EntrenadorId",
                        column: x => x.EntrenadorId,
                        principalTable: "Entrenador",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Miembro_Persona_Id",
                        column: x => x.Id,
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Miembro_TipoDeMiembro_TipoDeMiembroId",
                        column: x => x.TipoDeMiembroId,
                        principalTable: "TipoDeMiembro",
                        principalColumn: "TipoDeMiembroId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MembresiaXMiembro",
                columns: table => new
                {
                    MembresiaXMiembroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MiembroId = table.Column<int>(type: "int", nullable: false),
                    MembresiaId = table.Column<int>(type: "int", nullable: false),
                    EstadoMembresiaId = table.Column<int>(type: "int", nullable: false),
                    PagoId = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembresiaXMiembro", x => x.MembresiaXMiembroId);
                    table.ForeignKey(
                        name: "FK_MembresiaXMiembro_EstadoMembresia_EstadoMembresiaId",
                        column: x => x.EstadoMembresiaId,
                        principalTable: "EstadoMembresia",
                        principalColumn: "EstadoMembresiaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MembresiaXMiembro_Membresia_MembresiaId",
                        column: x => x.MembresiaId,
                        principalTable: "Membresia",
                        principalColumn: "MembresiaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MembresiaXMiembro_Miembro_MiembroId",
                        column: x => x.MiembroId,
                        principalTable: "Miembro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MembresiaXMiembro_Pago_PagoId",
                        column: x => x.PagoId,
                        principalTable: "Pago",
                        principalColumn: "PagoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MiembroXClase",
                columns: table => new
                {
                    MiembroXClaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MiembroId = table.Column<int>(type: "int", nullable: false),
                    ClaseId = table.Column<int>(type: "int", nullable: false),
                    FechaInscripcion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiembroXClase", x => x.MiembroXClaseId);
                    table.ForeignKey(
                        name: "FK_MiembroXClase_Clase_ClaseId",
                        column: x => x.ClaseId,
                        principalTable: "Clase",
                        principalColumn: "ClaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MiembroXClase_Miembro_MiembroId",
                        column: x => x.MiembroId,
                        principalTable: "Miembro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Asistencia",
                columns: table => new
                {
                    AsistenciaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MiembroXClaseId = table.Column<int>(type: "int", nullable: false),
                    MembresiaXMiembroId = table.Column<int>(type: "int", nullable: false),
                    TipoDeAsistenciaId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistencia", x => x.AsistenciaId);
                    table.ForeignKey(
                        name: "FK_Asistencia_MembresiaXMiembro_MembresiaXMiembroId",
                        column: x => x.MembresiaXMiembroId,
                        principalTable: "MembresiaXMiembro",
                        principalColumn: "MembresiaXMiembroId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asistencia_MiembroXClase_MiembroXClaseId",
                        column: x => x.MiembroXClaseId,
                        principalTable: "MiembroXClase",
                        principalColumn: "MiembroXClaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asistencia_TipoDeAsistencia_TipoDeAsistenciaId",
                        column: x => x.TipoDeAsistenciaId,
                        principalTable: "TipoDeAsistencia",
                        principalColumn: "TipoDeAsistenciaId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "EstadoMembresia",
                columns: new[] { "EstadoMembresiaId", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Activa" },
                    { 2, "Expirada" },
                    { 3, "Cancelada" }
                });

            migrationBuilder.InsertData(
                table: "TipoDeAsistencia",
                columns: new[] { "TipoDeAsistenciaId", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Gimnasio" },
                    { 2, "Clase" }
                });

            migrationBuilder.InsertData(
                table: "TipoDeMiembro",
                columns: new[] { "TipoDeMiembroId", "Descripcion", "PorcentajeDescuento" },
                values: new object[,]
                {
                    { 1, "Regular", 0 },
                    { 2, "Estudiante", 10 },
                    { 3, "Mayor", 20 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asistencia_MembresiaXMiembroId",
                table: "Asistencia",
                column: "MembresiaXMiembroId");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencia_MiembroXClaseId",
                table: "Asistencia",
                column: "MiembroXClaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencia_TipoDeAsistenciaId",
                table: "Asistencia",
                column: "TipoDeAsistenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Clase_ActividadId",
                table: "Clase",
                column: "ActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_Clase_EntrenadorId",
                table: "Clase",
                column: "EntrenadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Membresia_TipoDeMembresiaId",
                table: "Membresia",
                column: "TipoDeMembresiaId");

            migrationBuilder.CreateIndex(
                name: "IX_MembresiaXMiembro_EstadoMembresiaId",
                table: "MembresiaXMiembro",
                column: "EstadoMembresiaId");

            migrationBuilder.CreateIndex(
                name: "IX_MembresiaXMiembro_MembresiaId",
                table: "MembresiaXMiembro",
                column: "MembresiaId");

            migrationBuilder.CreateIndex(
                name: "IX_MembresiaXMiembro_MiembroId",
                table: "MembresiaXMiembro",
                column: "MiembroId");

            migrationBuilder.CreateIndex(
                name: "IX_MembresiaXMiembro_PagoId",
                table: "MembresiaXMiembro",
                column: "PagoId");

            migrationBuilder.CreateIndex(
                name: "IX_Miembro_EntrenadorId",
                table: "Miembro",
                column: "EntrenadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Miembro_TipoDeMiembroId",
                table: "Miembro",
                column: "TipoDeMiembroId");

            migrationBuilder.CreateIndex(
                name: "IX_MiembroXClase_ClaseId",
                table: "MiembroXClase",
                column: "ClaseId");

            migrationBuilder.CreateIndex(
                name: "IX_MiembroXClase_MiembroId",
                table: "MiembroXClase",
                column: "MiembroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asistencia");

            migrationBuilder.DropTable(
                name: "MembresiaXMiembro");

            migrationBuilder.DropTable(
                name: "MiembroXClase");

            migrationBuilder.DropTable(
                name: "TipoDeAsistencia");

            migrationBuilder.DropTable(
                name: "EstadoMembresia");

            migrationBuilder.DropTable(
                name: "Membresia");

            migrationBuilder.DropTable(
                name: "Pago");

            migrationBuilder.DropTable(
                name: "Clase");

            migrationBuilder.DropTable(
                name: "Miembro");

            migrationBuilder.DropTable(
                name: "TipoDeMembresia");

            migrationBuilder.DropTable(
                name: "Actividad");

            migrationBuilder.DropTable(
                name: "Entrenador");

            migrationBuilder.DropTable(
                name: "TipoDeMiembro");

            migrationBuilder.DropTable(
                name: "Persona");
        }
    }
}
