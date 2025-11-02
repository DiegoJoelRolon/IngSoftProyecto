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
    public class PagoServiceTests
    {
        [Fact]
        public async Task GetAllPagos_ReturnsList()
        {
            var command = new Mock<PagoCommand>(null);
            var query = new Mock<PagoQuery>(null);
            var mapper = new Mock<PagoMapper>();

            query.Setup(q => q.GetAllPagos()).ReturnsAsync(new List<Pago>());
            mapper.Setup(m => m.GetPagoResponseList(It.IsAny<List<Pago>>()))
                .ReturnsAsync(new List<PagoResponse>());

            var service = new PagoService(command.Object, query.Object, mapper.Object);

            var result = await service.GetAllPagos();

            Assert.NotNull(result);
            Assert.IsType<List<PagoResponse>>(result);
        }

        [Fact]
        public async Task GetPagoById_ReturnsResponse_WhenIdIsValid()
        {
            var command = new Mock<PagoCommand>(null);
            var query = new Mock<PagoQuery>(null);
            var mapper = new Mock<PagoMapper>();

            var fecha = DateTime.Today;
            var pago = new Pago
            {
                PagoId = 1,
                Monto = 150m,
                FechaPago = fecha,
                MetodoPago = "Efectivo",
                DescuentoAplicado = 0
            };
            var response = new PagoResponse
            {
                PagoId = 1,
                Monto = 150m,
                FechaPago = fecha,
                MetodoPago = "Efectivo",
                DescuentoAplicado = 0
            };

            query.Setup(q => q.GetPagoById(1)).ReturnsAsync(pago);
            mapper.Setup(m => m.GetPagoResponse(pago)).ReturnsAsync(response);

            var service = new PagoService(command.Object, query.Object, mapper.Object);

            var result = await service.GetPagoById(1);

            Assert.NotNull(result);
            Assert.IsType<PagoResponse>(result);
            Assert.Equal(150m, result.Monto);
        }

        [Fact]
        public async Task GetPagoById_ThrowsException_WhenIdInvalid()
        {
            var command = new Mock<PagoCommand>(null);
            var query = new Mock<PagoQuery>(null);
            var mapper = new Mock<PagoMapper>();

            query.Setup(q => q.GetPagoById(It.IsAny<int>())).ReturnsAsync((Pago?)null);

            var service = new PagoService(command.Object, query.Object, mapper.Object);

            var ex = await Assert.ThrowsAsync<Exception>(() => service.GetPagoById(999));
            Assert.Equal("El pago con ID 999 no existe.", ex.Message);
        }

        [Fact]
        public async Task AddPago_ReturnsResponse_WhenRequestIsValid()
        {
            var command = new Mock<PagoCommand>(null);
            var query = new Mock<PagoQuery>(null);
            var mapper = new Mock<PagoMapper>();

            var fecha = DateTime.Today;
            var added = new Pago
            {
                PagoId = 1,
                Monto = 200m,
                FechaPago = fecha,
                MetodoPago = "Tarjeta",
                DescuentoAplicado = 10
            };
            var response = new PagoResponse
            {
                PagoId = 1,
                Monto = 200m,
                FechaPago = fecha,
                MetodoPago = "Tarjeta",
                DescuentoAplicado = 10
            };

            command.Setup(c => c.AddPago(It.IsAny<Pago>())).ReturnsAsync(added);
            query.Setup(q => q.GetPagoById(1)).ReturnsAsync(added);
            mapper.Setup(m => m.GetPagoResponse(added)).ReturnsAsync(response);

            var service = new PagoService(command.Object, query.Object, mapper.Object);

            var request = new PagoRequest
            {
                Monto = 200m,
                FechaPago = fecha,
                MetodoPago = "Tarjeta",
                DescuentoAplicado = 10
            };

            var result = await service.AddPago(request);

            Assert.NotNull(result);
            Assert.IsType<PagoResponse>(result);
            Assert.Equal(200m, result.Monto);
            Assert.Equal(10, result.DescuentoAplicado);
        }

        [Fact]
        public async Task AddPago_ThrowsBadRequestException_WhenMontoInvalid()
        {
            var command = new Mock<PagoCommand>(null);
            var query = new Mock<PagoQuery>(null);
            var mapper = new Mock<PagoMapper>();

            var service = new PagoService(command.Object, query.Object, mapper.Object);

            var request = new PagoRequest
            {
                Monto = 0m,
                FechaPago = DateTime.Today,
                MetodoPago = "Efectivo",
                DescuentoAplicado = 0
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.AddPago(request));
            Assert.Equal("El monto del pago debe ser mayor que cero.", ex.Message);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public async Task AddPago_ThrowsBadRequestException_WhenDescuentoInvalid(int descuento)
        {
            var command = new Mock<PagoCommand>(null);
            var query = new Mock<PagoQuery>(null);
            var mapper = new Mock<PagoMapper>();

            var service = new PagoService(command.Object, query.Object, mapper.Object);

            var request = new PagoRequest
            {
                Monto = 100m,
                FechaPago = DateTime.Today,
                MetodoPago = "Efectivo",
                DescuentoAplicado = descuento
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.AddPago(request));
            Assert.Equal("El descuento aplicado debe estar entre 0 y 100.", ex.Message);
        }

        [Fact]
        public async Task UpdatePago_ReturnsResponse_WhenRequestIsValid()
        {
            var command = new Mock<PagoCommand>(null);
            var query = new Mock<PagoQuery>(null);
            var mapper = new Mock<PagoMapper>();

            var fecha = DateTime.Today;
            var existing = new Pago
            {
                PagoId = 1,
                Monto = 100m,
                FechaPago = fecha.AddDays(-1),
                MetodoPago = "Efectivo",
                DescuentoAplicado = 0
            };
            var updated = new Pago
            {
                PagoId = 1,
                Monto = 180m,
                FechaPago = fecha,
                MetodoPago = "Tarjeta",
                DescuentoAplicado = 5
            };

            query.SetupSequence(q => q.GetPagoById(1))
                .ReturnsAsync(existing)   // PagoExists
                .ReturnsAsync(existing)   // lectura para modificar
                .ReturnsAsync(updated);   // lectura final
            command.Setup(c => c.UpdatePago(It.IsAny<Pago>())).ReturnsAsync(updated);
            mapper.Setup(m => m.GetPagoResponse(updated)).ReturnsAsync(new PagoResponse
            {
                PagoId = 1,
                Monto = 180m,
                FechaPago = fecha,
                MetodoPago = "Tarjeta",
                DescuentoAplicado = 5
            });

            var service = new PagoService(command.Object, query.Object, mapper.Object);

            var request = new PagoRequest
            {
                Monto = 180m,
                FechaPago = fecha,
                MetodoPago = "Tarjeta",
                DescuentoAplicado = 5
            };

            var result = await service.UpdatePago(1, request);

            Assert.NotNull(result);
            Assert.Equal(180m, result.Monto);
            Assert.Equal(5, result.DescuentoAplicado);
        }

        [Fact]
        public async Task UpdatePago_ThrowsException_WhenIdIsInvalid()
        {
            var command = new Mock<PagoCommand>(null);
            var query = new Mock<PagoQuery>(null);
            var mapper = new Mock<PagoMapper>();

            query.Setup(q => q.GetPagoById(It.IsAny<int>())).ReturnsAsync((Pago?)null);

            var service = new PagoService(command.Object, query.Object, mapper.Object);

            var request = new PagoRequest
            {
                Monto = 100m,
                FechaPago = DateTime.Today,
                MetodoPago = "Efectivo",
                DescuentoAplicado = 0
            };

            var ex = await Assert.ThrowsAsync<Exception>(() => service.UpdatePago(999, request));
            Assert.Equal("El pago con ID 999 no existe.", ex.Message);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(100, -5)]
        [InlineData(100, 150)]
        public async Task UpdatePago_ThrowsBadRequestException_WhenInvalidFields(decimal monto, int descuento)
        {
            var command = new Mock<PagoCommand>(null);
            var query = new Mock<PagoQuery>(null);
            var mapper = new Mock<PagoMapper>();

            var existing = new Pago
            {
                PagoId = 1,
                Monto = 100m,
                FechaPago = DateTime.Today.AddDays(-1),
                MetodoPago = "Efectivo",
                DescuentoAplicado = 0
            };

            query.Setup(q => q.GetPagoById(1)).ReturnsAsync(existing);

            var service = new PagoService(command.Object, query.Object, mapper.Object);

            var request = new PagoRequest
            {
                Monto = monto,
                FechaPago = DateTime.Today,
                MetodoPago = "Efectivo",
                DescuentoAplicado = descuento
            };

            await Assert.ThrowsAsync<BadRequestException>(() => service.UpdatePago(1, request));
        }
    }
}