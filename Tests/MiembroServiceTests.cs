using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Services;
using Moq;

namespace Tests
{
    public class MiembroServiceTests
    {
        //**********************************************GetAllMiembros
        [Fact]
        public async Task GetAllMiembros_ReturnsList()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var miembroMapper = new Mock<MiembroMapper>();
            miembroQuery.Setup(q => q.GetAllMiembros()).ReturnsAsync(new List<Miembro>());
            miembroMapper.Setup(m => m.GetAllMiembrosResponse(It.IsAny<List<Miembro>>()))
                .ReturnsAsync(new List<MiembroResponse>());
            var service = new MiembroService(miembroQuery.Object, miembroCommand.Object, miembroMapper.Object, tipoDeMiembroQuery.Object, null, null);

            var result = await service.GetAllMiembros();

            Assert.NotNull(result);
            Assert.IsType<List<MiembroResponse>>(result);
        }

        //************************************************GetMiembroById
        [Fact]
        public async Task GetMiembroById_ReturnsMiembroResponse_WhenIdIsValid()
        {

            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var miembroMapper = new Mock<MiembroMapper>();

            var tipoDeMiembro = new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "desc", PorcentajeDescuento = 10 };
            var miembro = new Miembro
            {
                MiembroId = 1,
                TipoDeMiembroId = 1,
                EntrenadorId = 2,
                Nombre = "Juan",
                Direccion = "Calle Falsa 123",
                Telefono = "555-1234",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                TipoDeMiembro = tipoDeMiembro,
                Email = "juan@test.com",
                Foto = "foto.jpg",
                Eliminado = false
            };
            var miembroResponse = new MiembroResponse
            {
                TipoDeMiembroId = miembro.TipoDeMiembroId,
                EntrenadorId = miembro.EntrenadorId,
                Nombre = miembro.Nombre,
                Direccion = miembro.Direccion,
                Telefono = miembro.Telefono,
                FechaNacimiento = miembro.FechaNacimiento,
                TipoDeMiembro = new TipoDeMiembroResponse
                {
                    Descripcion = tipoDeMiembro.Descripcion,
                    PorcentajeDescuento = tipoDeMiembro.PorcentajeDescuento
                },
                Email = miembro.Email,
                Foto = miembro.Foto,
                Eliminado = miembro.Eliminado
            };

            miembroQuery.Setup(q => q.GetMiembroById(1)).ReturnsAsync(miembro);
            miembroMapper.Setup(m => m.GetMiembroResponse(miembro)).ReturnsAsync(miembroResponse);

            var service = new MiembroService(miembroQuery.Object, miembroCommand.Object, miembroMapper.Object, tipoDeMiembroQuery.Object, null, null);

            var result = await service.GetMiembroById(1);


            Assert.NotNull(result);
            Assert.IsType<MiembroResponse>(result);
            Assert.Equal(miembro.Nombre, result.Nombre);
        }


        [Fact]
        public async Task GetMiembroById_ThrowsNotFoundException_WhenIdInvalid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var miembroMapper = new Mock<MiembroMapper>();
            miembroQuery.Setup(q => q.GetMiembroById(It.IsAny<int>())).ReturnsAsync((Miembro)null);
            var service = new MiembroService(miembroQuery.Object, miembroCommand.Object, miembroMapper.Object, tipoDeMiembroQuery.Object, null, null);

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetMiembroById(0));
        }

        //*********************************************************AddMiembro
        [Fact]
        public async Task AddMiembro_ReturnsMiembroResponse_WhenRequestIsValid()
        {
            // Arrange
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var miembroMapper = new Mock<MiembroMapper>();

            var tipoDeMiembro = new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "desc", PorcentajeDescuento = 10 };
            var miembro = new Miembro
            {
                MiembroId = 1,
                TipoDeMiembroId = 1,
                EntrenadorId = 2,
                Nombre = "Juan",
                Direccion = "Calle Falsa 123",
                Telefono = "555-1234",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                TipoDeMiembro = tipoDeMiembro,
                Email = "juan@test.com",
                Foto = "foto.jpg",
                Eliminado = false
            };
            var miembroResponse = new MiembroResponse
            {
                TipoDeMiembroId = miembro.TipoDeMiembroId,
                EntrenadorId = miembro.EntrenadorId,
                Nombre = miembro.Nombre,
                Direccion = miembro.Direccion,
                Telefono = miembro.Telefono,
                FechaNacimiento = miembro.FechaNacimiento,
                TipoDeMiembro = new TipoDeMiembroResponse
                {
                    Descripcion = tipoDeMiembro.Descripcion,
                    PorcentajeDescuento = tipoDeMiembro.PorcentajeDescuento
                },
                Email = miembro.Email,
                Foto = miembro.Foto,
                Eliminado = miembro.Eliminado
            };

            var request = new MiembroRequest
            {
                TipoDeMiembroId = 1,
                EntrenadorId = 2,
                Nombre = "Juan",
                Direccion = "Calle Falsa 123",
                Telefono = "555-1234",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                Email = "juan@test.com",
                Foto = "foto.jpg"
            };

            tipoDeMiembroQuery.Setup(q => q.GetTipoDeMiembroById(1)).ReturnsAsync(tipoDeMiembro);
            miembroCommand.Setup(c => c.AddMiembro(It.IsAny<Miembro>())).ReturnsAsync(miembro);
            miembroQuery.Setup(q => q.GetMiembroById(1)).ReturnsAsync(miembro);
            miembroMapper.Setup(m => m.GetMiembroResponse(miembro)).ReturnsAsync(miembroResponse);

            var service = new MiembroService(miembroQuery.Object, miembroCommand.Object, miembroMapper.Object, tipoDeMiembroQuery.Object, null, null);

            // Act
            var result = await service.AddMiembro(request);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MiembroResponse>(result);
            Assert.Equal(request.Nombre, result.Nombre);
        }


        [Theory]
        [InlineData(1, null, "123", "2000-01-01", "test@test.com", "foto", "Nombre")]
        [InlineData(1, "dir", null, "2000-01-01", "test@test.com", "foto", "Telefono")]
        [InlineData(1, "dir", "123", "2027-01-01", "test@test.com", "foto", "FechaNacimiento")] // Futuro
        [InlineData(1, "dir", "123", "2000-01-01", null, "foto", "Email")]
        [InlineData(1, "dir", "123", "2000-01-01", "test@test.com", null, "Foto")]
        public async Task AddMiembro_ThrowsBadRequestException_WhenInvalidFields(
            int? tipoDeMiembroId,
            string nombre,
            string telefono,
            string fechaNacimientoStr,
            string email,
            string foto,
            string expectedInvalidField)
        {
            // Arrange
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var miembroMapper = new Mock<MiembroMapper>();

            tipoDeMiembroQuery.Setup(q => q.GetTipoDeMiembroById(It.IsAny<int>()))
                .ReturnsAsync(new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "desc", PorcentajeDescuento = 0 });

            var service = new MiembroService(miembroQuery.Object, miembroCommand.Object, miembroMapper.Object, tipoDeMiembroQuery.Object, null, null);

            var fechaNacimiento = DateTime.Parse(fechaNacimientoStr);

            var request = new MiembroRequest
            {
                TipoDeMiembroId = tipoDeMiembroId.HasValue ? tipoDeMiembroId.Value : 1,
                Nombre = nombre,
                Direccion = "dir",
                Telefono = telefono,
                FechaNacimiento = fechaNacimiento,
                Email = email,
                Foto = foto
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => service.AddMiembro(request));

            Assert.Contains(expectedInvalidField, exception.Message, StringComparison.OrdinalIgnoreCase);
        }


        [Fact]
        public async Task AddMiembro_ThrowsNotFoundException_WhenTipoDeMiembroIdIsInvalid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var miembroMapper = new Mock<MiembroMapper>();

            tipoDeMiembroQuery
                .Setup(q => q.GetTipoDeMiembroById(It.Is<int>(id => id == 9999)))
                .ReturnsAsync((TipoDeMiembro)null);

            tipoDeMiembroQuery
                .Setup(q => q.GetTipoDeMiembroById(It.Is<int>(id => id != 9999)))
                .ReturnsAsync(new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "desc", PorcentajeDescuento = 0 });

            var service = new MiembroService(miembroQuery.Object, miembroCommand.Object, miembroMapper.Object, tipoDeMiembroQuery.Object, null, null);

            var request = new MiembroRequest
            {
                TipoDeMiembroId = 9999,
                Nombre = "test",
                Direccion = "dir",
                Telefono = "123",
                FechaNacimiento = DateTime.Now.AddYears(-20),
                Email = "test@test.com",
                Foto = "foto"
            };

            await Assert.ThrowsAsync<NotFoundException>(() => service.AddMiembro(request));
        }

        //************************************************UpdateMiembro

        [Fact]
        public async Task UpdateMiembro_ReturnsMiembroResponse_WhenRequestIsValid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var miembroMapper = new Mock<MiembroMapper>();

            var tipoDeMiembro = new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "desc", PorcentajeDescuento = 10 };
            var miembro = new Miembro
            {
                MiembroId = 1,
                TipoDeMiembroId = 1,
                EntrenadorId = 2,
                Nombre = "Juan",
                Direccion = "Calle Falsa 123",
                Telefono = "555-1234",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                TipoDeMiembro = tipoDeMiembro,
                Email = "juan@test.com",
                Foto = "foto.jpg",
                Eliminado = false
            };
            var miembroResponse = new MiembroResponse
            {
                TipoDeMiembroId = miembro.TipoDeMiembroId,
                EntrenadorId = miembro.EntrenadorId,
                Nombre = miembro.Nombre,
                Direccion = miembro.Direccion,
                Telefono = miembro.Telefono,
                FechaNacimiento = miembro.FechaNacimiento,
                TipoDeMiembro = new TipoDeMiembroResponse
                {
                    Descripcion = tipoDeMiembro.Descripcion,
                    PorcentajeDescuento = tipoDeMiembro.PorcentajeDescuento
                },
                Email = miembro.Email,
                Foto = miembro.Foto,
                Eliminado = miembro.Eliminado
            };

            var request = new MiembroRequest
            {
                TipoDeMiembroId = 1,
                EntrenadorId = 2,
                Nombre = "Juan",
                Direccion = "Calle Falsa 123",
                Telefono = "555-1234",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                Email = "juan@test.com",
                Foto = "foto.jpg"
            };

            tipoDeMiembroQuery.Setup(q => q.GetTipoDeMiembroById(1)).ReturnsAsync(tipoDeMiembro);
            miembroQuery.Setup(q => q.GetMiembroById(1)).ReturnsAsync(miembro);
            miembroCommand.Setup(c => c.UpdateMiembro(It.IsAny<Miembro>())).ReturnsAsync(miembro);
            miembroMapper.Setup(m => m.GetMiembroResponse(miembro)).ReturnsAsync(miembroResponse);

            var service = new MiembroService(miembroQuery.Object, miembroCommand.Object, miembroMapper.Object, tipoDeMiembroQuery.Object, null, null);

            var result = await service.UpdateMiembro(1, request);

            Assert.NotNull(result);
            Assert.IsType<MiembroResponse>(result);
            Assert.Equal(request.Nombre, result.Nombre);
        }

        [Fact]
        public async Task UpdateMiembro_ThrowsNotFoundException_WhenIdIsInvalid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var miembroMapper = new Mock<MiembroMapper>();

            miembroQuery.Setup(q => q.GetMiembroById(It.IsAny<int>())).ReturnsAsync((Miembro)null);

            var service = new MiembroService(miembroQuery.Object, miembroCommand.Object, miembroMapper.Object, tipoDeMiembroQuery.Object, null, null);

            var request = new MiembroRequest
            {
                TipoDeMiembroId = 1,
                EntrenadorId = 2,
                Nombre = "Juan",
                Direccion = "Calle Falsa 123",
                Telefono = "555-1234",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                Email = "juan@test.com",
                Foto = "foto.jpg"
            };

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateMiembro(999, request));
        }

        [Fact]
        public async Task UpdateMiembro_ThrowsNotFoundException_WhenTipoDeMiembroIdIsInvalid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var miembroMapper = new Mock<MiembroMapper>();

            var miembroExistente = new Miembro
            {
                MiembroId = 1,
                TipoDeMiembroId = 1,
                EntrenadorId = 2,
                Nombre = "Juan",
                Direccion = "Calle Falsa 123",
                Telefono = "555-1234",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                TipoDeMiembro = new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "desc", PorcentajeDescuento = 10 },
                Email = "juan@test.com",
                Foto = "foto.jpg",
                Eliminado = false
            };

            miembroQuery.Setup(q => q.GetMiembroById(1)).ReturnsAsync(miembroExistente);

            tipoDeMiembroQuery
                .Setup(q => q.GetTipoDeMiembroById(It.Is<int>(id => id == 9999)))
                .ReturnsAsync((TipoDeMiembro)null);

            var service = new MiembroService(miembroQuery.Object, miembroCommand.Object, miembroMapper.Object, tipoDeMiembroQuery.Object, null, null);

            var request = new MiembroRequest
            {
                TipoDeMiembroId = 9999,
                EntrenadorId = 2,
                Nombre = "Juan",
                Direccion = "Calle Falsa 123",
                Telefono = "555-1234",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                Email = "juan@test.com",
                Foto = "foto.jpg"
            };

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateMiembro(1, request));
        }

        [Theory]
        [InlineData(1, null, "555-1234", "2000-01-01", "juan@test.com", "foto.jpg", "Nombre")]
        [InlineData(1, "Juan", null, "2000-01-01", "juan@test.com", "foto.jpg", "Telefono")]
        [InlineData(1, "Juan", "555-1234", "2027-01-01", "juan@test.com", "foto.jpg", "FechaNacimiento")]
        [InlineData(1, "Juan", "555-1234", "2000-01-01", null, "foto.jpg", "Email")]
        [InlineData(1, "Juan", "555-1234", "2000-01-01", "juan@test.com", null, "Foto")]
        public async Task UpdateMiembro_ThrowsBadRequestException_WhenInvalidFields(
            int? tipoDeMiembroId,
            string nombre,
            string telefono,
            string fechaNacimientoStr,
            string email,
            string foto,
            string expectedInvalidField)
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var miembroMapper = new Mock<MiembroMapper>();

            tipoDeMiembroQuery.Setup(q => q.GetTipoDeMiembroById(It.IsAny<int>()))
                .ReturnsAsync(new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "desc", PorcentajeDescuento = 0 });

            var miembro = new Miembro
            {
                MiembroId = 1,
                TipoDeMiembroId = 1,
                EntrenadorId = 2,
                Nombre = "Juan",
                Direccion = "Calle Falsa 123",
                Telefono = "555-1234",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                TipoDeMiembro = new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "desc", PorcentajeDescuento = 10 },
                Email = "juan@test.com",
                Foto = "foto.jpg",
                Eliminado = false
            };

            miembroQuery.Setup(q => q.GetMiembroById(1)).ReturnsAsync(miembro);

            var service = new MiembroService(miembroQuery.Object, miembroCommand.Object, miembroMapper.Object, tipoDeMiembroQuery.Object, null, null);

            var fechaNacimiento = DateTime.Parse(fechaNacimientoStr);

            var request = new MiembroRequest
            {
                TipoDeMiembroId = tipoDeMiembroId ?? 1,
                Nombre = nombre,
                Direccion = "Calle Falsa 123",
                Telefono = telefono,
                FechaNacimiento = fechaNacimiento,
                Email = email,
                Foto = foto
            };

            var exception = await Assert.ThrowsAsync<BadRequestException>(() => service.UpdateMiembro(1, request));
            Assert.Contains(expectedInvalidField, exception.Message, StringComparison.OrdinalIgnoreCase);
        }

        //************************************************DeleteMiembro
        [Fact]
        public async Task DeleteMiembro_ReturnsMiembroResponse_WhenIdIsValid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var miembroMapper = new Mock<MiembroMapper>();

            var tipoDeMiembro = new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "desc", PorcentajeDescuento = 10 };
            var miembro = new Miembro
            {
                MiembroId = 1,
                TipoDeMiembroId = 1,
                EntrenadorId = 2,
                Nombre = "Juan",
                Direccion = "Calle Falsa 123",
                Telefono = "555-1234",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                TipoDeMiembro = tipoDeMiembro,
                Email = "juan@test.com",
                Foto = "foto.jpg",
                Eliminado = false
            };
            var miembroEliminado = new Miembro
            {
                MiembroId = miembro.MiembroId,
                TipoDeMiembroId = miembro.TipoDeMiembroId,
                TipoDeMiembro = miembro.TipoDeMiembro,
                EntrenadorId = miembro.EntrenadorId,
                Entrenador = miembro.Entrenador,
                Nombre = miembro.Nombre,
                Direccion = miembro.Direccion,
                Telefono = miembro.Telefono,
                FechaNacimiento = miembro.FechaNacimiento,
                Email = miembro.Email,
                Foto = miembro.Foto,
                Eliminado = true,
                MembresiasXMiembros = miembro.MembresiasXMiembros,
                MiembrosXClases = miembro.MiembrosXClases
            };
            var miembroResponse = new MiembroResponse
            {
                TipoDeMiembroId = miembro.TipoDeMiembroId,
                EntrenadorId = miembro.EntrenadorId,
                Nombre = miembro.Nombre,
                Direccion = miembro.Direccion,
                Telefono = miembro.Telefono,
                FechaNacimiento = miembro.FechaNacimiento,
                TipoDeMiembro = new TipoDeMiembroResponse
                {
                    Descripcion = tipoDeMiembro.Descripcion,
                    PorcentajeDescuento = tipoDeMiembro.PorcentajeDescuento
                },
                Email = miembro.Email,
                Foto = miembro.Foto,
                Eliminado = true
            };

            miembroQuery.Setup(q => q.GetMiembroById(1)).ReturnsAsync(miembro);
            miembroCommand.Setup(c => c.UpdateMiembro(It.IsAny<Miembro>())).ReturnsAsync(miembroEliminado);
            miembroMapper.Setup(m => m.GetMiembroResponse(It.Is<Miembro>(m => m.Eliminado))).ReturnsAsync(miembroResponse);

            var service = new MiembroService(miembroQuery.Object, miembroCommand.Object, miembroMapper.Object, tipoDeMiembroQuery.Object, null, null);

            var result = await service.DeleteMiembro(1);

            Assert.NotNull(result);
            Assert.True(result.Eliminado);
        }

        [Fact]
        public async Task DeleteMiembro_ThrowsNotFoundException_WhenIdIsInvalid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var miembroMapper = new Mock<MiembroMapper>();

            miembroQuery.Setup(q => q.GetMiembroById(It.IsAny<int>())).ReturnsAsync((Miembro)null);

            var service = new MiembroService(miembroQuery.Object, miembroCommand.Object, miembroMapper.Object, tipoDeMiembroQuery.Object, null, null);

            await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteMiembro(999));
        }


        //************************************************RestoreMiembro
        [Fact]
        public async Task RestoreMiembro_ReturnsMiembroResponse_WhenIdIsValid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var miembroMapper = new Mock<MiembroMapper>();

            var tipoDeMiembro = new TipoDeMiembro { TipoDeMiembroId = 1, Descripcion = "desc", PorcentajeDescuento = 10 };
            var miembro = new Miembro
            {
                MiembroId = 1,
                TipoDeMiembroId = 1,
                EntrenadorId = 2,
                Nombre = "Juan",
                Direccion = "Calle Falsa 123",
                Telefono = "555-1234",
                FechaNacimiento = DateTime.Now.AddYears(-30),
                TipoDeMiembro = tipoDeMiembro,
                Email = "juan@test.com",
                Foto = "foto.jpg",
                Eliminado = true
            };
            var miembroRestaurado = new Miembro
            {
                MiembroId = miembro.MiembroId,
                TipoDeMiembroId = miembro.TipoDeMiembroId,
                TipoDeMiembro = miembro.TipoDeMiembro,
                EntrenadorId = miembro.EntrenadorId,
                Entrenador = miembro.Entrenador,
                Nombre = miembro.Nombre,
                Direccion = miembro.Direccion,
                Telefono = miembro.Telefono,
                FechaNacimiento = miembro.FechaNacimiento,
                Email = miembro.Email,
                Foto = miembro.Foto,
                Eliminado = false,
                MembresiasXMiembros = miembro.MembresiasXMiembros,
                MiembrosXClases = miembro.MiembrosXClases
            };
            var miembroResponse = new MiembroResponse
            {
                TipoDeMiembroId = miembro.TipoDeMiembroId,
                EntrenadorId = miembro.EntrenadorId,
                Nombre = miembro.Nombre,
                Direccion = miembro.Direccion,
                Telefono = miembro.Telefono,
                FechaNacimiento = miembro.FechaNacimiento,
                TipoDeMiembro = new TipoDeMiembroResponse
                {
                    Descripcion = tipoDeMiembro.Descripcion,
                    PorcentajeDescuento = tipoDeMiembro.PorcentajeDescuento
                },
                Email = miembro.Email,
                Foto = miembro.Foto,
                Eliminado = false
            };

            miembroQuery.Setup(q => q.GetMiembroById(1)).ReturnsAsync(miembro);
            miembroCommand.Setup(c => c.UpdateMiembro(It.IsAny<Miembro>())).ReturnsAsync(miembroRestaurado);
            miembroMapper.Setup(m => m.GetMiembroResponse(It.Is<Miembro>(m => !m.Eliminado))).ReturnsAsync(miembroResponse);

            var service = new MiembroService(miembroQuery.Object, miembroCommand.Object, miembroMapper.Object, tipoDeMiembroQuery.Object, null, null);

            var result = await service.RestoreMiembro(1);

            Assert.NotNull(result);
            Assert.False(result.Eliminado);
        }

        [Fact]
        public async Task RestoreMiembro_ThrowsNotFoundException_WhenIdIsInvalid()
        {
            var miembroQuery = new Mock<MiembroQuery>(null);
            var miembroCommand = new Mock<MiembroCommand>(null);
            var tipoDeMiembroQuery = new Mock<TipoDeMiembroQuery>(null);
            var miembroMapper = new Mock<MiembroMapper>();

            miembroQuery.Setup(q => q.GetMiembroById(It.IsAny<int>())).ReturnsAsync((Miembro)null);

            var service = new MiembroService(miembroQuery.Object, miembroCommand.Object, miembroMapper.Object, tipoDeMiembroQuery.Object, null, null);

            await Assert.ThrowsAsync<NotFoundException>(() => service.RestoreMiembro(999));
        }


    }
}
