using System;
using System.Collections.Generic;
using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Services;
using Moq;
using Xunit;

namespace Tests
{
    public class TipoDeAsistenciaServiceTests
    {
        [Fact]
        public async Task GetAllTipoDeAsistencias_ReturnsList()
        {
            var cmd = new Mock<TipoDeAsistenciaCommand>(null);
            var qry = new Mock<TipoDeAsistenciaQuery>(null);
            var mapper = new Mock<GenericMapper>();

            qry.Setup(q => q.GetAllTipoDeAsistencia()).ReturnsAsync(new List<TipoDeAsistencia>());
            mapper.Setup(m => m.GetAllGenericResponse(It.IsAny<List<TipoDeAsistencia>>()))
                .ReturnsAsync(new List<GenericResponse>());

            var service = new TipoDeAsistenciaService(cmd.Object, qry.Object, mapper.Object);

            var result = await service.GetAllTipoDeAsistencia();

            Assert.NotNull(result);
            Assert.IsType<List<GenericResponse>>(result);
        }

        [Fact]
        public async Task GetTipoDeAsistenciaById_ReturnsResponse_WhenIdIsValid()
        {
            var cmd = new Mock<TipoDeAsistenciaCommand>(null);
            var qry = new Mock<TipoDeAsistenciaQuery>(null);
            var mapper = new Mock<GenericMapper>();

            var tipo = new TipoDeAsistencia { TipoDeAsistenciaId = 1, Descripcion = "Presencial" };
            var response = new GenericResponse { Id = 1, Descripcion = "Presencial" };

            qry.Setup(q => q.GetTipoDeAsistenciaById(1)).ReturnsAsync(tipo);
            mapper.Setup(m => m.GetGenericResponse(tipo)).ReturnsAsync(response);

            var service = new TipoDeAsistenciaService(cmd.Object, qry.Object, mapper.Object);

            var result = await service.GetTipoDeAsistenciaById(1);

            Assert.NotNull(result);
            Assert.IsType<GenericResponse>(result);
            Assert.Equal("Presencial", result.Descripcion);
        }

        [Fact]
        public async Task GetTipoDeAsistenciaById_ThrowsException_WhenIdInvalid()
        {
            var cmd = new Mock<TipoDeAsistenciaCommand>(null);
            var qry = new Mock<TipoDeAsistenciaQuery>(null);
            var mapper = new Mock<GenericMapper>();

            qry.Setup(q => q.GetTipoDeAsistenciaById(It.IsAny<int>())).ReturnsAsync((TipoDeAsistencia?)null);

            var service = new TipoDeAsistenciaService(cmd.Object, qry.Object, mapper.Object);

            var ex = await Assert.ThrowsAsync<Exception>(() => service.GetTipoDeAsistenciaById(0));
            Assert.Equal("Id de tipo de asistencia invalido", ex.Message);
        }

        [Fact]
        public async Task AddTipoDeAsistencia_ReturnsResponse_WhenRequestIsValid()
        {
            var cmd = new Mock<TipoDeAsistenciaCommand>(null);
            var qry = new Mock<TipoDeAsistenciaQuery>(null);
            var mapper = new Mock<GenericMapper>();

            var tipo = new TipoDeAsistencia { TipoDeAsistenciaId = 1, Descripcion = "Virtual" };

            cmd.Setup(c => c.AddTipoDeAsistencia(It.IsAny<TipoDeAsistencia>())).ReturnsAsync(tipo);
            mapper.Setup(m => m.GetGenericResponse(It.IsAny<TipoDeAsistencia>()))
                .ReturnsAsync(new GenericResponse { Id = 1, Descripcion = "Virtual" });

            var service = new TipoDeAsistenciaService(cmd.Object, qry.Object, mapper.Object);

            var result = await service.AddTipoDeAsistencia(tipo);

            Assert.NotNull(result);
            Assert.IsType<GenericResponse>(result);
            Assert.Equal("Virtual", result.Descripcion);
        }

        [Fact]
        public async Task AddTipoDeAsistencia_ThrowsException_WhenDescripcionEmpty()
        {
            var cmd = new Mock<TipoDeAsistenciaCommand>(null);
            var qry = new Mock<TipoDeAsistenciaQuery>(null);
            var mapper = new Mock<GenericMapper>();

            var service = new TipoDeAsistenciaService(cmd.Object, qry.Object, mapper.Object);

            var tipo = new TipoDeAsistencia { Descripcion = "" };

            var ex = await Assert.ThrowsAsync<Exception>(() => service.AddTipoDeAsistencia(tipo));
            Assert.Equal("Descripcion no puede estar vacio", ex.Message);
        }

        [Fact]
        public async Task UpdateTipoDeAsistencia_ReturnsResponse_WhenRequestIsValid()
        {
            var cmd = new Mock<TipoDeAsistenciaCommand>(null);
            var qry = new Mock<TipoDeAsistenciaQuery>(null);
            var mapper = new Mock<GenericMapper>();

            var existing = new TipoDeAsistencia { TipoDeAsistenciaId = 1, Descripcion = "Old" };
            var updated = new TipoDeAsistencia { TipoDeAsistenciaId = 1, Descripcion = "New" };

            qry.Setup(q => q.GetTipoDeAsistenciaById(1)).ReturnsAsync(existing);
            cmd.Setup(c => c.UpdateTipoDeAsistencia(1, It.IsAny<TipoDeAsistencia>())).ReturnsAsync(updated);
            mapper.Setup(m => m.GetGenericResponse(updated)).ReturnsAsync(new GenericResponse { Id = 1, Descripcion = "New" });

            var service = new TipoDeAsistenciaService(cmd.Object, qry.Object, mapper.Object);

            var result = await service.UpdateTipoDeAsistencia(1, updated);

            Assert.NotNull(result);
            Assert.IsType<GenericResponse>(result);
            Assert.Equal("New", result.Descripcion);
        }

        [Fact]
        public async Task UpdateTipoDeAsistencia_ThrowsException_WhenIdInvalid()
        {
            var cmd = new Mock<TipoDeAsistenciaCommand>(null);
            var qry = new Mock<TipoDeAsistenciaQuery>(null);
            var mapper = new Mock<GenericMapper>();

            qry.Setup(q => q.GetTipoDeAsistenciaById(It.IsAny<int>())).ReturnsAsync((TipoDeAsistencia?)null);

            var service = new TipoDeAsistenciaService(cmd.Object, qry.Object, mapper.Object);

            var request = new TipoDeAsistencia { Descripcion = "New" };

            await Assert.ThrowsAsync<Exception>(() => service.UpdateTipoDeAsistencia(999, request));
        }

        [Fact]
        public async Task UpdateTipoDeAsistencia_ThrowsException_WhenDescripcionInvalid()
        {
            var cmd = new Mock<TipoDeAsistenciaCommand>(null);
            var qry = new Mock<TipoDeAsistenciaQuery>(null);
            var mapper = new Mock<GenericMapper>();

            var existing = new TipoDeAsistencia { TipoDeAsistenciaId = 1, Descripcion = "Old" };
            qry.Setup(q => q.GetTipoDeAsistenciaById(1)).ReturnsAsync(existing);

            var service = new TipoDeAsistenciaService(cmd.Object, qry.Object, mapper.Object);

            var request = new TipoDeAsistencia { Descripcion = "" };

            var ex = await Assert.ThrowsAsync<Exception>(() => service.UpdateTipoDeAsistencia(1, request));
            Assert.Equal("Descripcion no puede estar vacio", ex.Message);
        }
    }
}