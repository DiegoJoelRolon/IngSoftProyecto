using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public class MiembroServiceTests
    {
        //********************************************** GetAllMiembros
        [Fact]
        public async Task GetAllMiembros_ReturnsList()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var tipoDeMiembroCommand = new Mock<TipoDeMiembroCommand>(null);

            var entrenadorMapper = new EntrenadorMapper();
            var tipoDeMiembroMapper = new TipoDeMiembroMapper();
            var miembroMapper = new MiembroMapper(entrenadorMapper, tipoDeMiembroMapper);

            var entrenadorService = new Mock<EntrenadorService>(null, null, entrenadorMapper);

            miembroQuery.Setup(q => q.GetAllMiembros()).ReturnsAsync(new List<Miembro>());

            var service = new MiembroService(
                miembroQuery.Object,
                miembroCommand.Object,
                miembroMapper,
                tipoDeMiembroQuery.Object,
                tipoDeMiembroCommand.Object,
                tipoDeMiembroMapper,
                entrenadorService.Object

            );

            var result = await service.GetAllMiembros();

            Assert.NotNull(result);
            Assert.IsType<List<MiembroResponse>>(result);
        }

        //********************************************** GetMiembroById
        [Fact]
        public async Task GetMiembroById_ReturnsResponse_WhenIdIsValid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var tipoDeMiembroCommand = new Mock<TipoDeMiembroCommand>(null);
            var entrenadorMapper = new EntrenadorMapper();
            var tipoDeMiembroMapper = new TipoDeMiembroMapper();
            var miembroMapper = new MiembroMapper(entrenadorMapper, tipoDeMiembroMapper);

            var entrenadorService = new Mock<EntrenadorService>(null, null, entrenadorMapper);
            

            var tipoDeMiembro = new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "Premium", PorcentajeDescuento = 10 };

            var miembro = new Miembro
            {
                Id = 1,
                DNI = 12345678,
                TipoDeMiembroId = 1,
                TipoDeMiembro = tipoDeMiembro,
                Nombre = "Juan",
                Direccion = "Calle Falsa 123",
                Telefono = "555-1234",
                FechaNacimiento = new DateTime(1995, 3, 10),
                Email = "juan@test.com",
                Foto = "juan.jpg",
                Eliminado = false
            };

            miembroQuery.Setup(q => q.GetMiembroById(1)).ReturnsAsync(miembro);

            var service = new MiembroService(
                miembroQuery.Object,
                miembroCommand.Object,
                miembroMapper,
                tipoDeMiembroQuery.Object,
                tipoDeMiembroCommand.Object,
                tipoDeMiembroMapper,
                entrenadorService.Object

            );

            var result = await service.GetMiembroById(1);

            Assert.NotNull(result);
            Assert.IsType<MiembroResponse>(result);
            Assert.Equal("Juan", result.Nombre);
        }

        [Fact]
        public async Task GetMiembroById_ThrowsNotFoundException_WhenIdInvalid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            miembroQuery.Setup(q => q.GetMiembroById(It.IsAny<int>())).ReturnsAsync((Miembro?)null);

            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);

            var entrenadorMapper = new EntrenadorMapper();
            var tipoDeMiembroMapper = new TipoDeMiembroMapper();
            var miembroMapper = new MiembroMapper(entrenadorMapper, tipoDeMiembroMapper);

            var service = new MiembroService(
                miembroQuery.Object,
                miembroCommand.Object,
                miembroMapper,
                tipoDeMiembroQuery.Object,
                null,
                null,
                null
            );

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetMiembroById(0));
            Assert.Equal("Id de miembro invalido", ex.Message);
        }

        //********************************************** AddMiembro
        [Fact]
        public async Task AddMiembro_ReturnsResponse_WhenRequestIsValid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var tipoDeMiembroCommand = new Mock<TipoDeMiembroCommand>(null);

            var entrenadorMapper = new EntrenadorMapper();
            var tipoDeMiembroMapper = new TipoDeMiembroMapper();
            var miembroMapper = new MiembroMapper(entrenadorMapper, tipoDeMiembroMapper);

            var entrenadorService = new Mock<EntrenadorService>(null, null, entrenadorMapper);

            var tipoDeMiembro = new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "Básico", PorcentajeDescuento = 5 };
            tipoDeMiembroQuery.Setup(q => q.GetTipoDeMiembroById(1)).ReturnsAsync(tipoDeMiembro);

            var added = new Miembro {
                Id = 1,
                Nombre = "Lucia",
                DNI = 87654321,
                FechaNacimiento = new DateTime(1992, 7, 15),
                Telefono = "12345678",
                Direccion = "Av. Secundaria 2",
                Email = "lucia@example.com",
                Foto = "lucia.jpg",
                Eliminado = false,
                TipoDeMiembroId = 1,
            };

            var fetched = new Miembro
            {
                Id = 1,
                DNI = 12345678,
                Nombre = "Carlos",
                TipoDeMiembroId = 1,
                TipoDeMiembro = tipoDeMiembro,
                Email = "carlos@test.com",
                Direccion = "Calle Luna 45",
                Telefono = "12345678",
                FechaNacimiento = new DateTime(1990, 5, 15),
                Foto = "foto.jpg",
                Eliminado = false
            };

            miembroCommand.Setup(c => c.AddMiembro(It.IsAny<Miembro>())).ReturnsAsync(added);
            miembroQuery.Setup(q => q.GetMiembroById(1)).ReturnsAsync(fetched);

            var service = new MiembroService(
                miembroQuery.Object,
                miembroCommand.Object,
                miembroMapper,
                tipoDeMiembroQuery.Object,
                tipoDeMiembroCommand.Object,
                tipoDeMiembroMapper,
                entrenadorService.Object
            );

            var request = new MiembroRequest
            {
                TipoDeMiembroId = 1,
                DNI = 12345678,
                Nombre = "Carlos",
                Direccion = "Calle Luna 45",
                Telefono = "12345678",
                FechaNacimiento = new DateTime(1990, 5, 15),
                Email = "carlos@test.com",
                Foto = "foto.jpg"
            };

            var result = await service.AddMiembro(request);

            Assert.NotNull(result);
            Assert.IsType<MiembroResponse>(result);
            Assert.Equal("Carlos", result.Nombre);
        }

        [Fact]
        public async Task AddMiembro_ThrowsNotFoundException_WhenTipoDeMiembroInvalid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);

            tipoDeMiembroQuery.Setup(q => q.GetTipoDeMiembroById(It.IsAny<int>())).ReturnsAsync((TipoDeMiembro?)null);

            var miembroMapper = new MiembroMapper(new EntrenadorMapper(), new TipoDeMiembroMapper());
            var service = new MiembroService(
                miembroQuery.Object,
                miembroCommand.Object,
                miembroMapper,
                tipoDeMiembroQuery.Object,
                null,
                null,
                null
            );

            var request = new MiembroRequest
            {
                TipoDeMiembroId = 999,
                DNI = 12345678,
                Nombre = "Juan",
                Direccion = "Calle",
                Telefono = "12345678",
                FechaNacimiento = DateTime.Today.AddYears(-20),
                Email = "juan@test.com",
                Foto = "foto.jpg"
            };

            await Assert.ThrowsAsync<NotFoundException>(() => service.AddMiembro(request));
        }

        //********************************************** UpdateMiembro
        [Fact]
        public async Task UpdateMiembro_ReturnsResponse_WhenRequestIsValid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);

            var entrenadorMapper = new EntrenadorMapper();
            var tipoDeMiembroMapper = new TipoDeMiembroMapper();
            var miembroMapper = new MiembroMapper(entrenadorMapper, tipoDeMiembroMapper);

            var tipoDeMiembro = new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "Premium", PorcentajeDescuento = 10 };

            var existing = new Miembro
            {
                Id = 1,
                DNI = 12345678,
                TipoDeMiembroId = 1,
                TipoDeMiembro = tipoDeMiembro,
                Nombre = "Juan",
                Direccion = "Calle 1",
                Telefono = "12345678",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Email = "juan@test.com",
                Foto = "foto.jpg"
            };

            var updated = new Miembro
            {
                Id = 1,
                DNI = 12345678,
                TipoDeMiembroId = 1,
                TipoDeMiembro = tipoDeMiembro,
                Nombre = "Juan Actualizado",
                Direccion = "Calle 2",
                Telefono = "12345678",
                FechaNacimiento = existing.FechaNacimiento,
                Email = "juan@test.com",
                Foto = "foto.jpg"
            };

            tipoDeMiembroQuery.Setup(q => q.GetTipoDeMiembroById(1)).ReturnsAsync(tipoDeMiembro);
            miembroQuery.SetupSequence(q => q.GetMiembroById(1))
                .ReturnsAsync(existing) // Llamada 1: Lee la entidad existente
                .ReturnsAsync(updated)  // Llamada 2: Lee la entidad actualizada después del comando
                .ReturnsAsync(updated);

            miembroCommand.Setup(c => c.UpdateMiembro(It.IsAny<Miembro>())).ReturnsAsync(updated);

            var service = new MiembroService(
                miembroQuery.Object,
                miembroCommand.Object,
                miembroMapper,
                tipoDeMiembroQuery.Object,
                null,
                null,
                null
            );

            var request = new MiembroRequest
            {
                TipoDeMiembroId = 1,
                DNI = 12345678,
                Nombre = "Juan Actualizado",
                Direccion = "Calle 2",
                Telefono = "12345678",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Email = "juan@test.com",
                Foto = "foto.jpg"
            };

            var result = await service.UpdateMiembro(1, request);

            Assert.NotNull(result);
            Assert.Equal("Juan Actualizado", result.Nombre);
        }

        [Fact]
        public async Task UpdateMiembro_ThrowsNotFoundException_WhenIdInvalid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            miembroQuery.Setup(q => q.GetMiembroById(It.IsAny<int>())).ReturnsAsync((Miembro?)null);

            var miembroMapper = new MiembroMapper(new EntrenadorMapper(), new TipoDeMiembroMapper());
            var service = new MiembroService(miembroQuery.Object, null, miembroMapper, null, null, null,null);

            var request = new MiembroRequest
            {
                TipoDeMiembroId = 1,
                DNI = 12345678,
                Nombre = "Juan",
                Direccion = "Calle 1",
                Telefono = "12345678",
                FechaNacimiento = DateTime.Today.AddYears(-20),
                Email = "juan@test.com",
                Foto = "foto.jpg"
            };

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateMiembro(999, request));
        }

        //********************************************** DeleteMiembro
        [Fact]
        public async Task DeleteMiembro_ReturnsResponse_WhenIdIsValid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);

            var entrenadorMapper = new EntrenadorMapper();
            var tipoDeMiembroMapper = new TipoDeMiembroMapper();
            var miembroMapper = new MiembroMapper(entrenadorMapper, tipoDeMiembroMapper);

            var tipoDeMiembro = new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "desc", PorcentajeDescuento = 10 };

            var miembro = new Miembro
            {
                TipoDeMiembroId = 1,
                DNI = 12345678,
                Nombre = "Juan",
                Direccion = "Calle 1",
                Telefono = "12345678",
                FechaNacimiento = DateTime.Today.AddYears(-20),
                Email = "juan@test.com",
                Foto = "foto.jpg",
                Id = 1,
                TipoDeMiembro = tipoDeMiembro,
                Eliminado = false
            };

            var eliminado = miembro;
            eliminado.Eliminado = true;

            miembroQuery.Setup(q => q.GetMiembroById(1)).ReturnsAsync(miembro);
            miembroCommand.Setup(c => c.UpdateMiembro(It.IsAny<Miembro>())).ReturnsAsync(eliminado);

            var service = new MiembroService(
                miembroQuery.Object,
                miembroCommand.Object,
                miembroMapper,
                null,
                null,
                null,
                null
            );

            var result = await service.DeleteMiembro(1);

            Assert.NotNull(result);
            Assert.True(result.Eliminado);
        }

        [Fact]
        public async Task DeleteMiembro_ThrowsNotFoundException_WhenIdInvalid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            miembroQuery.Setup(q => q.GetMiembroById(It.IsAny<int>())).ReturnsAsync((Miembro?)null);

            var miembroMapper = new MiembroMapper(new EntrenadorMapper(), new TipoDeMiembroMapper());
            var service = new MiembroService(miembroQuery.Object, null, miembroMapper, null, null, null,null);

            await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteMiembro(999));
        }
    }
}
