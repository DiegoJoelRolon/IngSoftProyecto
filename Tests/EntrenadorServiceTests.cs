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
    public class EntrenadorServiceTests
    {
        [Fact]
        public async Task GetAllEntrenadores_ReturnsList()
        {
            var query = new Mock<EntrenadorQuery>(null);
            var command = new Mock<EntrenadorCommand>(null);
            var mapper = new Mock<EntrenadorMapper>();

            query.Setup(q => q.GetAllEntrenadores()).ReturnsAsync(new List<Entrenador>());
            mapper.Setup(m => m.GetAllEntrenadoresResponse(It.IsAny<List<Entrenador>>()))
                .ReturnsAsync(new List<EntrenadorResponse>());

            var service = new EntrenadorService(query.Object, command.Object, mapper.Object);

            var result = await service.GetAllEntrenadores();

            Assert.NotNull(result);
            Assert.IsType<List<EntrenadorResponse>>(result);
        }

        [Fact]
        public async Task GetEntrenadorById_ReturnsResponse_WhenIdIsValid()
        {
            var query = new Mock<EntrenadorQuery>(null);
            var command = new Mock<EntrenadorCommand>(null);
            var mapper = new Mock<EntrenadorMapper>();

            var fecha = DateTime.Now.AddYears(-35);
            var entrenador = new Entrenador
            {
                Id = 1,
                Nombre = "Pablo",
                DNI = 12345678,
                FechaNacimiento = fecha,
                Telefono = "555-1234",
                Direccion = "Calle Falsa 123",
                Email = "pablo@test.com",
                Foto = "foto.jpg",
                Eliminado = false,
                Certificacion = "CertA",
                Activo = true
            };
            var response = new EntrenadorResponse
            {
                Id = 1,
                Nombre = entrenador.Nombre,
                DNI = entrenador.DNI,
                FechaNacimiento = entrenador.FechaNacimiento,
                Telefono = entrenador.Telefono,
                Direccion = entrenador.Direccion,
                Email = entrenador.Email,
                Foto = entrenador.Foto,
                Eliminado = entrenador.Eliminado,
                Certificacion = entrenador.Certificacion,
                Activo = entrenador.Activo
            };

            query.Setup(q => q.GetEntrenadorById(1)).ReturnsAsync(entrenador);
            mapper.Setup(m => m.GetEntrenadorResponse(entrenador)).ReturnsAsync(response);

            var service = new EntrenadorService(query.Object, command.Object, mapper.Object);

            var result = await service.GetEntrenadorById(1);

            Assert.NotNull(result);
            Assert.IsType<EntrenadorResponse>(result);
            Assert.Equal(entrenador.Nombre, result.Nombre);
        }

        [Fact]
        public async Task GetEntrenadorById_ThrowsNotFoundException_WhenIdInvalid()
        {
            var query = new Mock<EntrenadorQuery>(null);
            var command = new Mock<EntrenadorCommand>(null);
            var mapper = new Mock<EntrenadorMapper>();

            query.Setup(q => q.GetEntrenadorById(It.IsAny<int>())).ReturnsAsync((Entrenador?)null);

            var service = new EntrenadorService(query.Object, command.Object, mapper.Object);

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetEntrenadorById(0));
            Assert.Equal("Id de entrenador invalido", ex.Message);
        }

        [Fact]
        public async Task AddEntrenadorAsync_ReturnsResponse_WhenRequestIsValid()
        {
            var query = new Mock<EntrenadorQuery>(null);
            var command = new Mock<EntrenadorCommand>(null);
            var mapper = new Mock<EntrenadorMapper>();

            var fecha = DateTime.Now.AddYears(-30);
            var added = new Entrenador
            {
                Id = 1,
                Nombre = "Lucia",
                DNI = 87654321,
                FechaNacimiento = fecha,
                Telefono = "555-0000",
                Direccion = "Av Siempre Viva 1",
                Email = "lucia@test.com",
                Foto = "foto2.jpg",
                Certificacion = "CertB",
                Activo = true,
                Eliminado = false
            };
            var response = new EntrenadorResponse
            {
                Id = 1,
                Nombre = added.Nombre,
                DNI = added.DNI,
                FechaNacimiento = added.FechaNacimiento,
                Telefono = added.Telefono,
                Direccion = added.Direccion,
                Email = added.Email,
                Foto = added.Foto,
                Eliminado = added.Eliminado,
                Certificacion = added.Certificacion,
                Activo = added.Activo
            };

            command.Setup(c => c.AddEntrenadorAsync(It.IsAny<Entrenador>())).ReturnsAsync(added);
            query.Setup(q => q.GetEntrenadorById(1)).ReturnsAsync(added);
            mapper.Setup(m => m.GetEntrenadorResponse(added)).ReturnsAsync(response);

            var service = new EntrenadorService(query.Object, command.Object, mapper.Object);

            var request = new EntrenadorRequest
            {
                Nombre = "Lucia",
                DNI = 87654321,
                FechaNacimiento = fecha,
                Telefono = "555-0000",
                Direccion = "Av Siempre Viva 1",
                Email = "lucia@test.com",
                Foto = "foto2.jpg",
                Certificacion = "CertB",
                Activo = true
            };

            var result = await service.AddEntrenadorAsync(request);

            Assert.NotNull(result);
            Assert.IsType<EntrenadorResponse>(result);
            Assert.Equal(request.Nombre, result.Nombre);
        }

        [Theory]
        [InlineData(null, 12345678, "555", "dir", "test@test.com", "foto.jpg", "Cert", true)]
        [InlineData("", 12345678, "555", "dir", "test@test.com", "foto.jpg", "Cert", true)]
        [InlineData("Pablo", 12345678, null, "dir", "test@test.com", "foto.jpg", "Cert", true)]
        [InlineData("Pablo", 12345678, "555", null, "test@test.com", "foto.jpg", "Cert", true)]
        [InlineData("Pablo", 12345678, "555", "dir", null, "foto.jpg", "Cert", true)]
        //[InlineData("Pablo", 12345678, "555", "dir", "test@test.com", null, "Cert", true)]
        [InlineData("Pablo", 12345678, "555", "dir", "test@test.com", "foto.jpg", null, true)]
        public async Task AddEntrenadorAsync_ThrowsBadRequestException_WhenInvalidFields(
            string nombre,
            int dni,
            string telefono,
            string direccion,
            string email,
            string foto,
            string certificacion,
            bool activo)
        {
            var query = new Mock<EntrenadorQuery>(null);
            var command = new Mock<EntrenadorCommand>(null);
            var mapper = new Mock<EntrenadorMapper>();

            var service = new EntrenadorService(query.Object, command.Object, mapper.Object);

            var request = new EntrenadorRequest
            {
                Nombre = nombre!,
                DNI = dni,
                FechaNacimiento = DateTime.Now.AddYears(-30),
                Telefono = telefono!,
                Direccion = direccion!,
                Email = email!,
                Foto = foto!,
                Certificacion = certificacion!,
                Activo = activo
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.AddEntrenadorAsync(request));
            Assert.Equal("Datos de entrenador invalidos", ex.Message);
        }

        [Fact]
        public async Task UpdateEntrenadorAsync_ReturnsResponse_WhenRequestIsValid()
        {
            var query = new Mock<EntrenadorQuery>(null);
            var command = new Mock<EntrenadorCommand>(null);
            var mapper = new Mock<EntrenadorMapper>();

            var fecha = DateTime.Now.AddYears(-40);

            var existing = new Entrenador
            {
                Id = 1,
                Nombre = "Old",
                DNI = 11122233,
                FechaNacimiento = fecha,
                Telefono = "111",
                Direccion = "Old dir",
                Email = "old@test.com",
                Foto = "old.jpg",
                Certificacion = "OldCert",
                Activo = true,
                Eliminado = false
            };

            var updated = new Entrenador
            {
                Id = 1,
                Nombre = "Nuevo",
                DNI = 11122233,
                FechaNacimiento = fecha,
                Telefono = "222",
                Direccion = "New dir",
                Email = "new@test.com",
                Foto = "new.jpg",
                Certificacion = "NewCert",
                Activo = true,
                Eliminado = false
            };

            var response = new EntrenadorResponse
            {
                Id = 1,
                Nombre = updated.Nombre,
                DNI = updated.DNI,
                FechaNacimiento = updated.FechaNacimiento,
                Telefono = updated.Telefono,
                Direccion = updated.Direccion,
                Email = updated.Email,
                Foto = updated.Foto,
                Eliminado = updated.Eliminado,
                Certificacion = updated.Certificacion,
                Activo = updated.Activo
            };

            query.Setup(q => q.GetEntrenadorById(1)).ReturnsAsync(existing);
            command.Setup(c => c.UpdateEntrenadorAsync(It.IsAny<Entrenador>())).ReturnsAsync(updated);
            mapper.Setup(m => m.GetEntrenadorResponse(updated)).ReturnsAsync(response);

            var service = new EntrenadorService(query.Object, command.Object, mapper.Object);

            var request = new EntrenadorRequest
            {
                Nombre = "Nuevo",
                DNI = 11122233,
                FechaNacimiento = fecha,
                Telefono = "222",
                Direccion = "New dir",
                Email = "new@test.com",
                Foto = "new.jpg",
                Certificacion = "NewCert",
                Activo = true
            };

            var result = await service.UpdateEntrenadorAsync(1, request);

            Assert.NotNull(result);
            Assert.IsType<EntrenadorResponse>(result);
            Assert.Equal(request.Nombre, result.Nombre);
        }

        [Fact]
        public async Task UpdateEntrenadorAsync_ThrowsNotFoundException_WhenIdIsInvalid()
        {
            var query = new Mock<EntrenadorQuery>(null);
            var command = new Mock<EntrenadorCommand>(null);
            var mapper = new Mock<EntrenadorMapper>();

            query.Setup(q => q.GetEntrenadorById(It.IsAny<int>())).ReturnsAsync((Entrenador?)null);

            var service = new EntrenadorService(query.Object, command.Object, mapper.Object);

            var request = new EntrenadorRequest
            {
                Nombre = "X",
                DNI = 11122233,
                FechaNacimiento = DateTime.Now.AddYears(-30),
                Telefono = "333",
                Direccion = "dir",
                Email = "x@test.com",
                Foto = "x.jpg",
                Certificacion = "CertX",
                Activo = true
            };

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateEntrenadorAsync(999, request));
        }

        [Theory]
        [InlineData(null, 12345678, "555", "dir", "test@test.com", "foto.jpg", "Cert", true)]
        public async Task UpdateEntrenadorAsync_ThrowsBadRequestException_WhenInvalidFields(
            string nombre,
            int dni,
            string telefono,
            string direccion,
            string email,
            string foto,
            string certificacion,
            bool activo)
        {
            var query = new Mock<EntrenadorQuery>(null);
            var command = new Mock<EntrenadorCommand>(null);
            var mapper = new Mock<EntrenadorMapper>();

            var existing = new Entrenador
            {
                Id = 1,
                Nombre = "Exist",
                DNI = 11111111,
                FechaNacimiento = DateTime.Now.AddYears(-40),
                Telefono = "111",
                Direccion = "dir",
                Email = "exist@test.com",
                Foto = "exist.jpg",
                Certificacion = "CertExist",
                Activo = true,
                Eliminado = false
            };

            query.Setup(q => q.GetEntrenadorById(1)).ReturnsAsync(existing);

            var service = new EntrenadorService(query.Object, command.Object, mapper.Object);

            var request = new EntrenadorRequest
            {
                Nombre = nombre!,
                DNI = dni,
                FechaNacimiento = DateTime.Now.AddYears(-30),
                Telefono = telefono!,
                Direccion = direccion!,
                Email = email!,
                Foto = foto!,
                Certificacion = certificacion!,
                Activo = activo
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.UpdateEntrenadorAsync(1, request));
            Assert.Equal("Datos de entrenador invalidos", ex.Message);
        }

        [Fact]
        public async Task DeleteEntrenadorAsync_ReturnsResponse_WhenIdIsValid()
        {
            var query = new Mock<EntrenadorQuery>(null);
            var command = new Mock<EntrenadorCommand>(null);
            var mapper = new Mock<EntrenadorMapper>();

            var fecha = DateTime.Now.AddYears(-30);

            var existing = new Entrenador
            {
                Id = 1,
                Nombre = "ToDelete",
                DNI = 22233344,
                FechaNacimiento = fecha,
                Telefono = "444",
                Direccion = "del dir",
                Email = "del@test.com",
                Foto = "del.jpg",
                Certificacion = "CertDel",
                Activo = true,
                Eliminado = false
            };
            var deleted = new Entrenador
            {
                Id = 1,
                Nombre = existing.Nombre,
                DNI = existing.DNI,
                FechaNacimiento = existing.FechaNacimiento,
                Telefono = existing.Telefono,
                Direccion = existing.Direccion,
                Email = existing.Email,
                Foto = existing.Foto,
                Certificacion = existing.Certificacion,
                Activo = existing.Activo,
                Eliminado = true
            };

            query.SetupSequence(q => q.GetEntrenadorById(1))
                .ReturnsAsync(existing)
                .ReturnsAsync(deleted);

            command.Setup(c => c.DeleteEntrenadorAsync(It.IsAny<Entrenador>())).Returns(Task.CompletedTask);
            mapper.Setup(m => m.GetEntrenadorResponse(deleted)).ReturnsAsync(new EntrenadorResponse
            {
                Id = deleted.Id,
                Nombre = deleted.Nombre,
                DNI = deleted.DNI,
                FechaNacimiento = deleted.FechaNacimiento,
                Telefono = deleted.Telefono,
                Direccion = deleted.Direccion,
                Email = deleted.Email,
                Foto = deleted.Foto,
                Eliminado = deleted.Eliminado,
                Certificacion = deleted.Certificacion,
                Activo = deleted.Activo
            });

            var service = new EntrenadorService(query.Object, command.Object, mapper.Object);

            var result = await service.DeleteEntrenadorAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Eliminado);
        }

        [Fact]
        public async Task RestoreEntrenadorAsync_ReturnsResponse_WhenIdIsValid()
        {
            var query = new Mock<EntrenadorQuery>(null);
            var command = new Mock<EntrenadorCommand>(null);
            var mapper = new Mock<EntrenadorMapper>();

            var fecha = DateTime.Now.AddYears(-30);

            var existing = new Entrenador
            {
                Id = 1,
                Nombre = "ToRestore",
                DNI = 33344455,
                FechaNacimiento = fecha,
                Telefono = "777",
                Direccion = "rest dir",
                Email = "rest@test.com",
                Foto = "rest.jpg",
                Certificacion = "CertR",
                Activo = true,
                Eliminado = true
            };
            var restored = new Entrenador
            {
                Id = 1,
                Nombre = existing.Nombre,
                DNI = existing.DNI,
                FechaNacimiento = existing.FechaNacimiento,
                Telefono = existing.Telefono,
                Direccion = existing.Direccion,
                Email = existing.Email,
                Foto = existing.Foto,
                Certificacion = existing.Certificacion,
                Activo = existing.Activo,
                Eliminado = false
            };

            query.SetupSequence(q => q.GetEntrenadorById(1))
                .ReturnsAsync(existing)   // EntrenadorExists
                .ReturnsAsync(existing)   // lectura previa a restore
                .ReturnsAsync(restored);  // lectura final tras restore

            command.Setup(c => c.RestoreEntrenadorAsync(It.IsAny<Entrenador>())).ReturnsAsync(restored);
            mapper.Setup(m => m.GetEntrenadorResponse(restored)).ReturnsAsync(new EntrenadorResponse
            {
                Id = restored.Id,
                Nombre = restored.Nombre,
                DNI = restored.DNI,
                FechaNacimiento = restored.FechaNacimiento,
                Telefono = restored.Telefono,
                Direccion = restored.Direccion,
                Email = restored.Email,
                Foto = restored.Foto,
                Eliminado = restored.Eliminado,
                Certificacion = restored.Certificacion,
                Activo = restored.Activo
            });

            var service = new EntrenadorService(query.Object, command.Object, mapper.Object);

            var result = await service.RestoreEntrenadorAsync(1);

            Assert.NotNull(result);
            Assert.False(result.Eliminado);
        }
    }
}