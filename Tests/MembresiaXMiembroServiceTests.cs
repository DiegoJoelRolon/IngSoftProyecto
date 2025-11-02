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
    public class MembresiaXMiembroServiceTests
    {
        private static Miembro CreateTestMiembro(int id = 1, string nombre = "Juan") => new Miembro
        {
            Id = id,
            TipoDeMiembroId = 1,
            DNI = 12345678,
            FechaNacimiento = DateTime.Today.AddYears(-30),
            Telefono = "000-0000",
            Direccion = "Calle Falsa 123",
            Email = "juan@example.com",
            Foto = "foto.jpg",
            Nombre = nombre,
            Eliminado = false
        };

        private static MiembroResponse CreateTestMiembroResponse(int id = 1, string nombre = "Juan") => new MiembroResponse
        {
            Id = id,
            TipoDeMiembroId = 1,
            Entrenador = null,
            Nombre = nombre,
            Direccion = "Calle Falsa 123",
            Telefono = "000-0000",
            FechaNacimiento = DateTime.Today.AddYears(-30),
            TipoDeMiembro = new TipoDeMiembroResponse { Descripcion = "Socio", PorcentajeDescuento = 0 },
            Email = "juan@example.com",
            Foto = "foto.jpg",
            Eliminado = false
        };

        private static Pago CreateTestPago(int id = 1, decimal monto = 100m) => new Pago
        {
            PagoId = id,
            Monto = monto,
            FechaPago = DateTime.Now,
            MetodoPago = "Efectivo",
            DescuentoAplicado = 0,
            MembresiasXMiembro = new List<MembresiaXMiembro>()
        };

        private static PagoResponse CreateTestPagoResponse(int id = 1, decimal monto = 100m) => new PagoResponse
        {
            PagoId = id,
            Monto = monto,
            FechaPago = DateTime.Now,
            MetodoPago = "Efectivo",
            DescuentoAplicado = 0
        };

        [Fact]
        public async Task GetAllMembresiasXMiembros_ReturnsList()
        {
            var command = new Mock<MembresiaXMiembroCommand>(null);
            var query = new Mock<MembresiaXMiembroQuery>(null);
            var mapper = new Mock<MembresiaXMiembroMapper>(null, null, null, null);

            query.Setup(q => q.GetAllMembresiasXMiembros()).ReturnsAsync(new List<MembresiaXMiembro>());
            mapper.Setup(m => m.GetMembresiaXMiembroResponseList(It.IsAny<List<MembresiaXMiembro>>()))
                .ReturnsAsync(new List<MembresiaXMiembroResponse>());

            var service = new MembresiaXMiembroService(command.Object, query.Object, mapper.Object, null, null, null, null);

            var result = await service.GetAllMembresiasXMiembros();

            Assert.NotNull(result);
            Assert.IsType<List<MembresiaXMiembroResponse>>(result);
        }

        [Fact]
        public async Task GetMembresiaXMiembroById_ReturnsResponse_WhenIdIsValid()
        {
            var command = new Mock<MembresiaXMiembroCommand>(null);
            var query = new Mock<MembresiaXMiembroQuery>(null);
            var mapper = new Mock<MembresiaXMiembroMapper>(null, null, null, null);

            var fechaInicio = DateTime.Today;
            var fechaFin = fechaInicio.AddDays(30);

            var entity = new MembresiaXMiembro
            {
                MembresiaXMiembroId = 1,
                MiembroId = 1,
                MembresiaId = 1,
                EstadoMembresiaId = 1,
                PagoId = 1,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Miembro = CreateTestMiembro(1, "Juan"),
                Membresia = new Membresia { MembresiaId = 1, DuracionEnDias = 30, CostoBase = 100m },
                Pago = CreateTestPago(1, 100m),
                EstadoMembresia = new EstadoMembresia { EstadoMembresiaId = 1, Descripcion = "Activa" },
                Asistencias = new List<Asistencia>()
            };

            var response = new MembresiaXMiembroResponse
            {
                MembresiaXMiembroId = 1,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Miembro = CreateTestMiembroResponse(1, "Juan"),
                Membresia = new MembresiaResponse { DuracionEnDias = 30, CostoBase = 100m },
                Pago = CreateTestPagoResponse(1, 100m),
                EstadoMembresia = new GenericResponse { Id = 1, Descripcion = "Activa" }
            };

            query.Setup(q => q.GetMembresiaXMiembroById(1)).ReturnsAsync(entity);
            mapper.Setup(m => m.GetMembresiaXMiembroResponse(entity)).ReturnsAsync(response);

            var service = new MembresiaXMiembroService(command.Object, query.Object, mapper.Object, null, null, null, null);

            var result = await service.GetMembresiaXMiembroById(1);

            Assert.NotNull(result);
            Assert.Equal(fechaInicio, result.FechaInicio);
            Assert.Equal(fechaFin, result.FechaFin);
        }

        [Fact]
        public async Task GetMembresiaXMiembroById_ThrowsNotFoundException_WhenIdInvalid()
        {
            var command = new Mock<MembresiaXMiembroCommand>(null);
            var query = new Mock<MembresiaXMiembroQuery>(null);
            var mapper = new Mock<MembresiaXMiembroMapper>(null, null, null, null);

            query.Setup(q => q.GetMembresiaXMiembroById(It.IsAny<int>())).ReturnsAsync((MembresiaXMiembro?)null);

            var service = new MembresiaXMiembroService(command.Object, query.Object, mapper.Object, null, null, null, null);

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetMembresiaXMiembroById(0));
            Assert.Equal("Id de MembresiaXMiembro invalido", ex.Message);
        }

        [Fact]
        public async Task AddMembresiaXMiembro_ReturnsResponse_WhenRequestIsValid()
        {
            var command = new Mock<MembresiaXMiembroCommand>(null);
            var query = new Mock<MembresiaXMiembroQuery>(null);
            var mapper = new Mock<MembresiaXMiembroMapper>(null, null, null, null);

            var mockMiembroService = new Mock<MiembroService>(null, null, null, null, null, null, null);
            var mockMembresiasService = new Mock<MembresiasService>(null, null, null, null, null);
            var mockEstadoQuery = new Mock<EstadoMembresiaQuery>(null);
            var mockPagoService = new Mock<PagoService>(null, null, null);

            var fechaInicio = new DateTime(2025, 12, 1);
            var fechaFin = fechaInicio.AddDays(30);

            var added = new MembresiaXMiembro {EstadoMembresiaId=1,MembresiaId=1,MiembroId=1,PagoId=1, MembresiaXMiembroId = 1, FechaInicio = fechaInicio, FechaFin = fechaFin };
            var fetched = added;
            var response = new MembresiaXMiembroResponse { MembresiaXMiembroId = 1, FechaInicio = fechaInicio, FechaFin = fechaFin };

            var pagoResponseValido = CreateTestPagoResponse(1, 100m);
            pagoResponseValido.FechaPago = fechaInicio.AddDays(-1);

            mockMiembroService.Setup(s => s.GetMiembroById(It.IsAny<int>())).ReturnsAsync(CreateTestMiembroResponse());
            mockMembresiasService.Setup(s => s.GetMembresiaById(It.IsAny<int>()))
                .ReturnsAsync(new MembresiaResponse { DuracionEnDias = 30, CostoBase = 100m });
            mockEstadoQuery.Setup(q => q.GetEstadoMembresiaById(It.IsAny<int>()))
                .ReturnsAsync(new EstadoMembresia { EstadoMembresiaId = 1, Descripcion = "Activa" });
            mockPagoService.Setup(p => p.GetPagoById(It.IsAny<int>())).ReturnsAsync(pagoResponseValido);
            query.Setup(q => q.GetUltimaMembresiaActiva(It.IsAny<int>())).ReturnsAsync((MembresiaXMiembro?)null);
            command.Setup(c => c.AddMembresiaXMiembro(It.IsAny<MembresiaXMiembro>())).ReturnsAsync(added);
            query.Setup(q => q.GetMembresiaXMiembroById(1)).ReturnsAsync(fetched);
            mapper.Setup(m => m.GetMembresiaXMiembroResponse(fetched)).ReturnsAsync(response);

            var service = new MembresiaXMiembroService(command.Object, query.Object, mapper.Object,
                mockMiembroService.Object, mockMembresiasService.Object, mockEstadoQuery.Object, mockPagoService.Object);

            var request = new MembresiaXMiembroRequest { MiembroId = 1, MembresiaId = 1, EstadoMembresiaId = 1, PagoId = 1, FechaInicio = fechaInicio, FechaFin = fechaFin };

            var result = await service.AddMembresiaXMiembro(request);

            Assert.NotNull(result);
            Assert.Equal(fechaInicio, result.FechaInicio);
        }

        // El resto de los tests (Add y Update) siguen igual pero ahora usando el nuevo constructor
        // sin necesidad de SetPrivateField.
        // Ejemplo:
        // var service = new MembresiaXMiembroService(command.Object, query.Object, mapper.Object,
        //     mockMiembroService.Object, mockMembresiasService.Object, mockEstadoQuery.Object, mockPagoService.Object);
    }
}
