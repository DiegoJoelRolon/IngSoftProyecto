using System;
using System.Collections.Generic;
using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Services;
using Moq;
using Xunit;

namespace Tests
{
    public class AsistenciaServiceTests
    {
        [Fact]
        public async Task GetAllAsistencias_ReturnsList()
        {
            var cmd = new Mock<AsistenciaCommand>(null);
            var qry = new Mock<AsistenciaQuery>(null);
            var tipoSvc = new Mock<TipoDeAsistenciaService>(null, null, null);
            var miembroXClaseSvc = new Mock<MiembroXClaseService>(null, null, null, null, null);
            var membresiaXMiembroSvc = new Mock<MembresiaXMiembroService>(null, null, null, null, null, null, null);
            var mapper = new Mock<AsistenciaMaper>(null, null, null);

            qry.Setup(q => q.GetAllAsistencias()).ReturnsAsync(new List<Asistencia>());
            mapper.Setup(m => m.GetAsistenciaResponseList(It.IsAny<List<Asistencia>>()))
                .ReturnsAsync(new List<AsistenciaResponse>());

            var service = new AsistenciaService(cmd.Object, qry.Object, tipoSvc.Object, miembroXClaseSvc.Object, membresiaXMiembroSvc.Object, mapper.Object);

            var result = await service.GetAllAsistencias();

            Assert.NotNull(result);
            Assert.IsType<List<AsistenciaResponse>>(result);
        }

        [Fact]
        public async Task GetAsistenciaById_ReturnsResponse_WhenIdIsValid()
        {
            var cmd = new Mock<AsistenciaCommand>(null);
            var qry = new Mock<AsistenciaQuery>(null);
            var tipoSvc = new Mock<TipoDeAsistenciaService>(null, null, null);
            var miembroXClaseSvc = new Mock<MiembroXClaseService>(null, null, null, null, null);
            var membresiaXMiembroSvc = new Mock<MembresiaXMiembroService>(null, null, null, null, null, null, null);
            var mapper = new Mock<AsistenciaMaper>(null, null, null);

            var fecha = DateTime.Today;

            // Configuración completa de Miembro y MembresiaXMiembro (propiedades required)
            var miembroModel = new Miembro
            {
                Id = 1,
                Direccion = "Calle Falsa 123",
                DNI = 12345678,
                Email = "email@email.com",
                FechaNacimiento = new DateTime(1999, 1, 1),
                Nombre = "Juan Perez",
                Telefono = "555-1234",
                Foto = "Foto",
                TipoDeMiembroId = 1,
                TipoDeMiembro = new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "Regular", PorcentajeDescuento = 0 },
                EntrenadorId = null,
                Entrenador = null,
                MembresiasXMiembros = new List<MembresiaXMiembro>(),
                MiembrosXClases = new List<MiembroXClase>()
            };

            var membresiaModel = new MembresiaXMiembro
            {
                MembresiaXMiembroId = 1,
                MiembroId = miembroModel.Id,
                Miembro = miembroModel,
                MembresiaId = 1,
                Membresia = new Membresia { MembresiaId = 1, CostoBase=100,DuracionEnDias=30,TipoDeMembresiaId=1},
                EstadoMembresiaId = 1,
                EstadoMembresia = new EstadoMembresia { EstadoMembresiaId = 1, Descripcion = "Activa" },
                PagoId = 1,
                Pago = new Pago { PagoId = 1, Monto = 0m,MetodoPago="Tarjeta",DescuentoAplicado=0 },
                FechaInicio = DateTime.Today.AddDays(-30),
                FechaFin = DateTime.Today.AddDays(30),
                Asistencias = new List<Asistencia>()
            };

            var entity = new Asistencia
            {
                AsistenciaId = 1,
                Fecha = fecha,
                MiembroXClaseId = 1,
                MembresiaXMiembroId = membresiaModel.MembresiaXMiembroId,
                TipoDeAsistenciaId = 1,
                MiembroXClase = new MiembroXClase
                {
                    MiembroXClaseId = 1,
                    FechaInscripcion = DateTime.Today.AddDays(-10),
                    Miembro = miembroModel
                },
                MembresiaXMiembro = membresiaModel,
                TipoDeAsistencia = new TipoDeAsistencia { TipoDeAsistenciaId = 1, Descripcion = "Presencial" }
            };

            var response = new AsistenciaResponse
            {
                AsistenciaId = 1,
                Fecha = fecha,
                MiembroXClase = new MiembroXClaseResponse
                {
                    MiembroXClaseId = 1,
                    FechaInscripcion = DateTime.Today.AddDays(-10),
                    Miembro = new MiembroResponse
                    {
                        Id = 1,
                        TipoDeMiembroId = 1,
                        Entrenador = null,
                        Nombre = "Ana",
                        Direccion = "Calle Falsa 123",
                        Telefono = "555-1234",
                        FechaNacimiento = new DateTime(1990, 1, 1),
                        TipoDeMiembro = new TipoDeMiembroResponse { Descripcion = "Regular",PorcentajeDescuento = 0 },
                        Email = "ana@example.com",
                        Foto = string.Empty,
                        Eliminado = false
                    }
                },
                MembresiaXMiembro = new MembresiaXMiembroResponse
                {
                    MembresiaXMiembroId = 1,
                    FechaInicio = DateTime.Today.AddDays(-30),
                    FechaFin = DateTime.Today.AddDays(30)
                },
                TipoDeAsistencia = new GenericResponse { Id = 1, Descripcion = "Presencial" }
            };

            qry.Setup(q => q.GetAsistenciaById(1)).ReturnsAsync(entity);
            mapper.Setup(m => m.GetAsistenciaResponse(entity)).ReturnsAsync(response);

            var service = new AsistenciaService(cmd.Object, qry.Object, tipoSvc.Object, miembroXClaseSvc.Object, membresiaXMiembroSvc.Object, mapper.Object);

            var result = await service.GetAsistenciaById(1);

            Assert.NotNull(result);
            Assert.IsType<AsistenciaResponse>(result);
            Assert.Equal(fecha, result.Fecha);
        }

        [Fact]
        public async Task GetAsistenciaById_ThrowsNotFoundException_WhenIdInvalid()
        {
            var cmd = new Mock<AsistenciaCommand>(null);
            var qry = new Mock<AsistenciaQuery>(null);
            var tipoSvc = new Mock<TipoDeAsistenciaService>(null, null, null);
            var miembroXClaseSvc = new Mock<MiembroXClaseService>(null, null, null, null, null);
            var membresiaXMiembroSvc = new Mock<MembresiaXMiembroService>(null, null, null, null, null, null, null);
            var mapper = new Mock<AsistenciaMaper>(null, null, null);

            qry.Setup(q => q.GetAsistenciaById(It.IsAny<int>())).ReturnsAsync((Asistencia?)null);

            var service = new AsistenciaService(cmd.Object, qry.Object, tipoSvc.Object, miembroXClaseSvc.Object, membresiaXMiembroSvc.Object, mapper.Object);

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetAsistenciaById(0));
            Assert.Equal("Id de Asistencia invalido", ex.Message);
        }

        [Fact]
        public async Task AddAsistencia_ReturnsResponse_WhenRequestIsValid()
        {
            var cmd = new Mock<AsistenciaCommand>(null);
            var qry = new Mock<AsistenciaQuery>(null);
            var tipoSvc = new Mock<TipoDeAsistenciaService>(null, null, null);
            var miembroXClaseSvc = new Mock<MiembroXClaseService>(null, null, null, null, null);
            var membresiaXMiembroSvc = new Mock<MembresiaXMiembroService>(null, null, null, null, null, null, null);
            var mapper = new Mock<AsistenciaMaper>(null, null, null);

            var fecha = DateTime.Today;
            var added = new Asistencia { AsistenciaId = 1, Fecha = fecha };

            // Reutilizamos modelo completo para consistencia
            var miembroModel = new Miembro
            {
                Id = 1,
                Direccion = "Calle Falsa 123",
                DNI = 12345678,
                Email = "email@email.com",
                FechaNacimiento = new DateTime(1999, 1, 1),
                Nombre = "Juan Perez",
                Telefono = "555-1234",
                Foto = "Foto",
                TipoDeMiembroId = 1,
                TipoDeMiembro = new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "Regular",PorcentajeDescuento = 0 },
                EntrenadorId = null,
                Entrenador = null,
                MembresiasXMiembros = new List<MembresiaXMiembro>(),
                MiembrosXClases = new List<MiembroXClase>()
            };

            var membresiaModel = new MembresiaXMiembro
            {
                MembresiaXMiembroId = 1,
                MiembroId = miembroModel.Id,
                Miembro = miembroModel,
                MembresiaId = 1,
                Membresia = new Membresia { MembresiaId = 1, CostoBase = 100, DuracionEnDias = 30, TipoDeMembresiaId = 1 },
                EstadoMembresiaId = 1,
                EstadoMembresia = new EstadoMembresia { EstadoMembresiaId = 1, Descripcion = "Activa" },
                PagoId = 1,
                Pago = new Pago { PagoId = 1, Monto = 0m, MetodoPago = "Tarjeta", DescuentoAplicado = 0 },
                FechaInicio = DateTime.Today.AddDays(-30),
                FechaFin = DateTime.Today.AddDays(30),
                Asistencias = new List<Asistencia>()
            };

            var fetched = new Asistencia
            {
                AsistenciaId = 1,
                Fecha = fecha,
                MiembroXClaseId = 1,
                MembresiaXMiembroId = membresiaModel.MembresiaXMiembroId,
                TipoDeAsistenciaId = 1,
                MiembroXClase = new MiembroXClase { MiembroXClaseId = 1, FechaInscripcion = DateTime.Today.AddDays(-5), Miembro = miembroModel },
                MembresiaXMiembro = membresiaModel,
                TipoDeAsistencia = new TipoDeAsistencia { TipoDeAsistenciaId = 1, Descripcion = "Presencial" }
            };
            var response = new AsistenciaResponse
            {
                AsistenciaId = 1,
                Fecha = fecha,
                MiembroXClase = new MiembroXClaseResponse { MiembroXClaseId = 1, FechaInscripcion = DateTime.Today.AddDays(-5) },
                MembresiaXMiembro = new MembresiaXMiembroResponse { MembresiaXMiembroId = 1, FechaInicio = DateTime.Today.AddDays(-30), FechaFin = DateTime.Today.AddDays(30) },
                TipoDeAsistencia = new GenericResponse { Id = 1, Descripcion = "Presencial" }
            };

            // Validaciones
            membresiaXMiembroSvc.Setup(s => s.GetMembresiaXMiembroById(It.IsAny<int>())).ReturnsAsync(new MembresiaXMiembroResponse { MembresiaXMiembroId = 1, FechaInicio = DateTime.Today.AddDays(-30), FechaFin = DateTime.Today.AddDays(30) });
            miembroXClaseSvc.Setup(s => s.GetMiembroXClaseById(It.IsAny<int>())).ReturnsAsync(new MiembroXClaseResponse { MiembroXClaseId = 1, FechaInscripcion = DateTime.Today.AddDays(-5) });
            tipoSvc.Setup(s => s.GetTipoDeAsistenciaById(It.IsAny<int>())).ReturnsAsync(new GenericResponse { Id = 1, Descripcion = "Presencial" });

            cmd.Setup(c => c.AddAsistencia(It.IsAny<Asistencia>())).ReturnsAsync(added);
            qry.Setup(q => q.GetAsistenciaById(1)).ReturnsAsync(fetched);
            mapper.Setup(m => m.GetAsistenciaResponse(fetched)).ReturnsAsync(response);

            var service = new AsistenciaService(cmd.Object, qry.Object, tipoSvc.Object, miembroXClaseSvc.Object, membresiaXMiembroSvc.Object, mapper.Object);

            var request = new AsistenciaRequest
            {
                MiembroXClaseId = 1,
                MembresiaXMiembroId = 1,
                TipoDeAsistenciaId = 1,
                Fecha = fecha
            };

            var result = await service.AddAsistencia(request);

            Assert.NotNull(result);
            Assert.IsType<AsistenciaResponse>(result);
            Assert.Equal(fecha, result.Fecha);
        }

        [Fact]
        public async Task AddAsistencia_ThrowsBadRequestException_WhenMembresiaXMiembroInvalid()
        {
            var cmd = new Mock<AsistenciaCommand>(null);
            var qry = new Mock<AsistenciaQuery>(null);
            var tipoSvc = new Mock<TipoDeAsistenciaService>(null, null, null);
            var miembroXClaseSvc = new Mock<MiembroXClaseService>(null, null, null, null, null);
            var membresiaXMiembroSvc = new Mock<MembresiaXMiembroService>(null, null, null, null, null, null, null);
            var mapper = new Mock<AsistenciaMaper>(null, null, null);

            // MembresiaXMiembro invalido
            membresiaXMiembroSvc.Setup(s => s.GetMembresiaXMiembroById(It.IsAny<int>())).ReturnsAsync((MembresiaXMiembroResponse?)null);
            miembroXClaseSvc.Setup(s => s.GetMiembroXClaseById(It.IsAny<int>())).ReturnsAsync(new MiembroXClaseResponse { MiembroXClaseId = 1, FechaInscripcion = DateTime.Today.AddDays(-5) });
            tipoSvc.Setup(s => s.GetTipoDeAsistenciaById(It.IsAny<int>())).ReturnsAsync(new GenericResponse { Id = 1, Descripcion = "Presencial" });

            var service = new AsistenciaService(cmd.Object, qry.Object, tipoSvc.Object, miembroXClaseSvc.Object, membresiaXMiembroSvc.Object, mapper.Object);

            var request = new AsistenciaRequest
            {
                MiembroXClaseId = 1,
                MembresiaXMiembroId = 999,
                TipoDeAsistenciaId = 1,
                Fecha = DateTime.Today
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.AddAsistencia(request));
            Assert.Contains("MembresiaXMiembro", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AddAsistencia_ThrowsBadRequestException_WhenMiembroXClaseInvalid()
        {
            var cmd = new Mock<AsistenciaCommand>(null);
            var qry = new Mock<AsistenciaQuery>(null);
            var tipoSvc = new Mock<TipoDeAsistenciaService>(null, null, null);
            var miembroXClaseSvc = new Mock<MiembroXClaseService>(null, null, null, null, null);
            var membresiaXMiembroSvc = new Mock<MembresiaXMiembroService>(null, null, null, null, null, null, null);
            var mapper = new Mock<AsistenciaMaper>(null, null, null);

            membresiaXMiembroSvc.Setup(s => s.GetMembresiaXMiembroById(It.IsAny<int>())).ReturnsAsync(new MembresiaXMiembroResponse { MembresiaXMiembroId = 1, FechaInicio = DateTime.Today.AddDays(-30), FechaFin = DateTime.Today.AddDays(30) });
            miembroXClaseSvc.Setup(s => s.GetMiembroXClaseById(It.IsAny<int>())).ReturnsAsync((MiembroXClaseResponse?)null);
            tipoSvc.Setup(s => s.GetTipoDeAsistenciaById(It.IsAny<int>())).ReturnsAsync(new GenericResponse { Id = 1, Descripcion = "Presencial" });

            var service = new AsistenciaService(cmd.Object, qry.Object, tipoSvc.Object, miembroXClaseSvc.Object, membresiaXMiembroSvc.Object, mapper.Object);

            var request = new AsistenciaRequest
            {
                MiembroXClaseId = 999,
                MembresiaXMiembroId = 1,
                TipoDeAsistenciaId = 1,
                Fecha = DateTime.Today
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.AddAsistencia(request));
            Assert.Contains("MiembroXClase", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AddAsistencia_ThrowsBadRequestException_WhenTipoDeAsistenciaInvalid()
        {
            var cmd = new Mock<AsistenciaCommand>(null);
            var qry = new Mock<AsistenciaQuery>(null);
            var tipoSvc = new Mock<TipoDeAsistenciaService>(null, null, null);
            var miembroXClaseSvc = new Mock<MiembroXClaseService>(null, null, null, null, null);
            var membresiaXMiembroSvc = new Mock<MembresiaXMiembroService>(null, null, null, null, null, null, null);
            var mapper = new Mock<AsistenciaMaper>(null, null, null);

            membresiaXMiembroSvc.Setup(s => s.GetMembresiaXMiembroById(It.IsAny<int>())).ReturnsAsync(new MembresiaXMiembroResponse { MembresiaXMiembroId = 1, FechaInicio = DateTime.Today.AddDays(-30), FechaFin = DateTime.Today.AddDays(30) });
            miembroXClaseSvc.Setup(s => s.GetMiembroXClaseById(It.IsAny<int>())).ReturnsAsync(new MiembroXClaseResponse { MiembroXClaseId = 1, FechaInscripcion = DateTime.Today.AddDays(-5) });
            tipoSvc.Setup(s => s.GetTipoDeAsistenciaById(It.IsAny<int>())).ReturnsAsync((GenericResponse?)null);

            var service = new AsistenciaService(cmd.Object, qry.Object, tipoSvc.Object, miembroXClaseSvc.Object, membresiaXMiembroSvc.Object, mapper.Object);

            var request = new AsistenciaRequest
            {
                MiembroXClaseId = 1,
                MembresiaXMiembroId = 1,
                TipoDeAsistenciaId = 999,
                Fecha = DateTime.Today
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.AddAsistencia(request));
            Assert.Contains("TipoDeAsistencia", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task UpdateAsistencia_ReturnsResponse_WhenRequestIsValid()
        {
            var cmd = new Mock<AsistenciaCommand>(null);
            var qry = new Mock<AsistenciaQuery>(null);
            var tipoSvc = new Mock<TipoDeAsistenciaService>(null, null, null);
            var miembroXClaseSvc = new Mock<MiembroXClaseService>(null, null, null, null, null);
            var membresiaXMiembroSvc = new Mock<MembresiaXMiembroService>(null, null, null, null,null,null,null);
            var mapper = new Mock<AsistenciaMaper>(null, null, null);

            var fechaInicio = DateTime.Today;
            var fechaOld = fechaInicio.AddDays(1);
            var fechaNew = fechaInicio.AddDays(10);

            var existing = new Asistencia
            {
                AsistenciaId = 1,
                Fecha = fechaOld,
                MiembroXClaseId = 1,
                MembresiaXMiembroId = 1,
                TipoDeAsistenciaId = 1,
                MiembroXClase = new MiembroXClase { MiembroXClaseId = 1, FechaInscripcion = DateTime.Today.AddDays(-5) },
                MembresiaXMiembro = new MembresiaXMiembro { MembresiaXMiembroId = 1, FechaInicio = DateTime.Today.AddDays(-30), FechaFin = DateTime.Today.AddDays(30),EstadoMembresiaId=1,MembresiaId=1,MiembroId=1,PagoId=1 },
                TipoDeAsistencia = new TipoDeAsistencia { TipoDeAsistenciaId = 1, Descripcion = "Presencial" }
            };

            var updated = new Asistencia
            {
                AsistenciaId = 1,
                Fecha = fechaNew,
                MiembroXClaseId = 1,
                MembresiaXMiembroId = 1,
                TipoDeAsistenciaId = 1,
                MiembroXClase = existing.MiembroXClase,
                MembresiaXMiembro = existing.MembresiaXMiembro,
                TipoDeAsistencia = existing.TipoDeAsistencia
            };

            // El query será llamado varias veces: Exists (1), lectura para modificar (2) y lectura final (3)
            qry.SetupSequence(q => q.GetAsistenciaById(1))
                .ReturnsAsync(existing)
                .ReturnsAsync(existing)
                .ReturnsAsync(updated);

            cmd.Setup(c => c.UpdateAsistencia(It.IsAny<Asistencia>())).ReturnsAsync(updated);
            mapper.Setup(m => m.GetAsistenciaResponse(updated)).ReturnsAsync(new AsistenciaResponse
            {
                AsistenciaId = 1,
                Fecha = fechaNew,
                MiembroXClase = new MiembroXClaseResponse { MiembroXClaseId = 1 },
                MembresiaXMiembro = new MembresiaXMiembroResponse { MembresiaXMiembroId = 1,FechaFin=DateTime.Today.AddDays(2),FechaInicio=DateTime.Today },
                TipoDeAsistencia = new GenericResponse { Id = 1, Descripcion = "Presencial" }
            });

            // Validaciones
            membresiaXMiembroSvc.Setup(s => s.GetMembresiaXMiembroById(It.IsAny<int>())).ReturnsAsync(new MembresiaXMiembroResponse { MembresiaXMiembroId = 1, FechaInicio = DateTime.Today.AddDays(-30), FechaFin = DateTime.Today.AddDays(30) });
            miembroXClaseSvc.Setup(s => s.GetMiembroXClaseById(It.IsAny<int>())).ReturnsAsync(new MiembroXClaseResponse { MiembroXClaseId = 1, FechaInscripcion = DateTime.Today.AddDays(-5) });
            tipoSvc.Setup(s => s.GetTipoDeAsistenciaById(It.IsAny<int>())).ReturnsAsync(new GenericResponse { Id = 1, Descripcion = "Presencial" });

            var service = new AsistenciaService(cmd.Object, qry.Object, tipoSvc.Object, miembroXClaseSvc.Object, membresiaXMiembroSvc.Object, mapper.Object);

            var request = new AsistenciaRequest
            {
                MiembroXClaseId = 1,
                MembresiaXMiembroId = 1,
                TipoDeAsistenciaId = 1,
                Fecha = fechaNew
            };

            var result = await service.UpdateAsistencia(1, request);

            Assert.NotNull(result);
            Assert.Equal(fechaNew, result.Fecha);
        }

        [Fact]
        public async Task UpdateAsistencia_ThrowsNotFoundException_WhenIdInvalid()
        {
            var cmd = new Mock<AsistenciaCommand>(null);
            var qry = new Mock<AsistenciaQuery>(null);
            var tipoSvc = new Mock<TipoDeAsistenciaService>(null, null, null);
            var miembroXClaseSvc = new Mock<MiembroXClaseService>(null, null, null, null, null);
            var membresiaXMiembroSvc = new Mock<MembresiaXMiembroService>(null, null, null, null, null, null, null);
            var mapper = new Mock<AsistenciaMaper>(null, null, null);

            qry.Setup(q => q.GetAsistenciaById(It.IsAny<int>())).ReturnsAsync((Asistencia?)null);

            var service = new AsistenciaService(cmd.Object, qry.Object, tipoSvc.Object, miembroXClaseSvc.Object, membresiaXMiembroSvc.Object, mapper.Object);

            var request = new AsistenciaRequest
            {
                MiembroXClaseId = 1,
                MembresiaXMiembroId = 1,
                TipoDeAsistenciaId = 1,
                Fecha = DateTime.Today
            };

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsistencia(999, request));
        }

        [Fact]
        public async Task UpdateAsistencia_ThrowsBadRequestException_WhenTipoDeAsistenciaInvalid()
        {
            var cmd = new Mock<AsistenciaCommand>(null);
            var qry = new Mock<AsistenciaQuery>(null);
            var tipoSvc = new Mock<TipoDeAsistenciaService>(null, null, null);
            var miembroXClaseSvc = new Mock<MiembroXClaseService>(null, null, null, null, null);
            var membresiaXMiembroSvc = new Mock<MembresiaXMiembroService>(null, null, null, null, null, null, null);
            var mapper = new Mock<AsistenciaMaper>(null, null, null);

            var existing = new Asistencia
            {
                AsistenciaId = 1,
                Fecha = DateTime.Today,
                MiembroXClaseId = 1,
                MembresiaXMiembroId = 1,
                TipoDeAsistenciaId = 1,
                MiembroXClase = new MiembroXClase { MiembroXClaseId = 1, FechaInscripcion = DateTime.Today.AddDays(-5) },
                MembresiaXMiembro = new MembresiaXMiembro { MembresiaXMiembroId = 1, FechaInicio = DateTime.Today.AddDays(-30), FechaFin = DateTime.Today.AddDays(30),EstadoMembresiaId=1,MembresiaId=1,MiembroId=1,PagoId=1 },
                TipoDeAsistencia = new TipoDeAsistencia { TipoDeAsistenciaId = 1, Descripcion = "Presencial" }
            };

            qry.Setup(q => q.GetAsistenciaById(1)).ReturnsAsync(existing);

            // Validaciones: tipo invalido
            membresiaXMiembroSvc.Setup(s => s.GetMembresiaXMiembroById(It.IsAny<int>())).ReturnsAsync(new MembresiaXMiembroResponse { MembresiaXMiembroId = 1, FechaInicio = DateTime.Today.AddDays(-30), FechaFin = DateTime.Today.AddDays(30) });
            miembroXClaseSvc.Setup(s => s.GetMiembroXClaseById(It.IsAny<int>())).ReturnsAsync(new MiembroXClaseResponse { MiembroXClaseId = 1, FechaInscripcion = DateTime.Today.AddDays(-5) });
            tipoSvc.Setup(s => s.GetTipoDeAsistenciaById(It.IsAny<int>())).ReturnsAsync((GenericResponse?)null);

            var service = new AsistenciaService(cmd.Object, qry.Object, tipoSvc.Object, miembroXClaseSvc.Object, membresiaXMiembroSvc.Object, mapper.Object);

            var request = new AsistenciaRequest
            {
                MiembroXClaseId = 1,
                MembresiaXMiembroId = 1,
                TipoDeAsistenciaId = 999,
                Fecha = DateTime.Today.AddDays(1)
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.UpdateAsistencia(1, request));
            Assert.Contains("TipoDeAsistencia", ex.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}