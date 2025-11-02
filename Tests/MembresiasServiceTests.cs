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
    public class MembresiasServiceTests
    {
        [Fact]
        public async Task GetAllMembresias_ReturnsList()
        {
            var query = new Mock<MembresiaQuery>(null);
            var command = new Mock<MembresiaCommand>(null);
            var miembroQuery = new Mock<MiembroQuery>(null);
            var tipoQuery = new Mock<TipoDeMembresiaQuery>(null);
            var tipoCommand = new Mock<TipoDeMembresiaCommand>(null);
            var tipoService = new Mock<TipoDeMembresiaService>(tipoCommand.Object, tipoQuery.Object);

            var genericMapper = new GenericMapper();
            var mapper = new Mock<MembresiaMapper>(genericMapper);

            query.Setup(q => q.GetAllMembresias()).ReturnsAsync(new List<Membresia>());
            mapper.Setup(m => m.GetAllMembresiasResponse(It.IsAny<List<Membresia>>()))
                .ReturnsAsync(new List<MembresiaResponse>());

            var service = new MembresiasService(query.Object, command.Object, tipoService.Object, mapper.Object, miembroQuery.Object);

            var result = await service.GetAllMembresias();

            Assert.NotNull(result);
            Assert.IsType<List<MembresiaResponse>>(result);
        }

        [Fact]
        public async Task GetMembresiaById_ReturnsResponse_WhenIdIsValid()
        {
            var query = new Mock<MembresiaQuery>(null);
            var command = new Mock<MembresiaCommand>(null);
            var miembroQuery = new Mock<MiembroQuery>(null);
            var tipoQuery = new Mock<TipoDeMembresiaQuery>(null);
            var tipoCommand = new Mock<TipoDeMembresiaCommand>(null);
            var tipoService = new Mock<TipoDeMembresiaService>(tipoCommand.Object, tipoQuery.Object);

            var genericMapper = new GenericMapper();
            var mapper = new Mock<MembresiaMapper>(genericMapper);

            var membresia = new Membresia
            {
                MembresiaId = 1,
                TipoDeMembresiaId = 1,
                DuracionEnDias = 30,
                CostoBase = 100m
            };
            var response = new MembresiaResponse
            {
                DuracionEnDias = membresia.DuracionEnDias,
                CostoBase = membresia.CostoBase
            };

            query.Setup(q => q.GetMembresiaById(1)).ReturnsAsync(membresia);
            mapper.Setup(m => m.GetMembresiaResponse(membresia)).ReturnsAsync(response);

            var service = new MembresiasService(query.Object, command.Object, tipoService.Object, mapper.Object, miembroQuery.Object);

            var result = await service.GetMembresiaById(1);

            Assert.NotNull(result);
            Assert.IsType<MembresiaResponse>(result);
            Assert.Equal(30, result.DuracionEnDias);
        }

        [Fact]
        public async Task GetMembresiaById_ThrowsNotFoundException_WhenIdIsInvalid()
        {
            var query = new Mock<MembresiaQuery>(null);
            var command = new Mock<MembresiaCommand>(null);
            var miembroQuery = new Mock<MiembroQuery>(null);
            var tipoQuery = new Mock<TipoDeMembresiaQuery>(null);
            var tipoCommand = new Mock<TipoDeMembresiaCommand>(null);
            var tipoService = new Mock<TipoDeMembresiaService>(tipoCommand.Object, tipoQuery.Object);

            var genericMapper = new GenericMapper();
            var mapper = new Mock<MembresiaMapper>(genericMapper);

            query.Setup(q => q.GetMembresiaById(It.IsAny<int>())).ReturnsAsync((Membresia?)null);

            var service = new MembresiasService(query.Object, command.Object, tipoService.Object, mapper.Object, miembroQuery.Object);

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetMembresiaById(0));
            Assert.Equal("Id de membresia invalido.", ex.Message);
        }

        [Fact]
        public async Task AddMembresia_ReturnsResponse_WhenRequestIsValid()
        {
            var query = new Mock<MembresiaQuery>(null);
            var command = new Mock<MembresiaCommand>(null);

            var tipoQuery = new Mock<TipoDeMembresiaQuery>(null);
            var tipoCommand = new Mock<TipoDeMembresiaCommand>(null);
            var miembroQuery = new Mock<MiembroQuery>(null);
            var tipoService = new Mock<TipoDeMembresiaService>(tipoCommand.Object, tipoQuery.Object);

            var genericMapper = new GenericMapper();
            var mapper = new Mock<MembresiaMapper>(genericMapper);

            var tipo = new TipoDeMembresia { TipoDeMembresiaId = 1, Descripcion = "Mes" };

            tipoService.Setup(s => s.GetTipoDeMembresiaById(1)).ReturnsAsync(tipo);
            command.Setup(c => c.AddMembresia(It.IsAny<Membresia>())).ReturnsAsync(new Membresia());
            mapper.Setup(m => m.GetMembresiaResponse(It.IsAny<Membresia>()))
                .ReturnsAsync(new MembresiaResponse { DuracionEnDias = 30, CostoBase = 100m });

            var service = new MembresiasService(query.Object, command.Object, tipoService.Object, mapper.Object,miembroQuery.Object);

            var request = new MembresiaRequest { TipoDeMembresiaId = 1, DuracionEnDias = 30, CostoBase = 100m };

            var result = await service.AddMembresia(request);

            Assert.NotNull(result);
            Assert.Equal(30, result.DuracionEnDias);
        }

        [Fact]
        public async Task AddMembresia_ThrowsBadRequestException_WhenRequestIsNull()
        {
            var query = new Mock<MembresiaQuery>(null);
            var command = new Mock<MembresiaCommand>(null);

            var tipoQuery = new Mock<TipoDeMembresiaQuery>(null);
            var tipoCommand = new Mock<TipoDeMembresiaCommand>(null);
            var miembroQuery = new Mock<MiembroQuery>(null);
            var tipoService = new Mock<TipoDeMembresiaService>(tipoCommand.Object, tipoQuery.Object);

            var genericMapper = new GenericMapper();
            var mapper = new Mock<MembresiaMapper>(genericMapper);

            var service = new MembresiasService(query.Object, command.Object, tipoService.Object, mapper.Object, miembroQuery.Object);

            await Assert.ThrowsAsync<BadRequestException>(() => service.AddMembresia(null!));
        }

        [Theory]
        [InlineData(0, 100.0, "DuracionEnDias invalida")]
        [InlineData(30, 0.0, "CostoBase invalido")]
        [InlineData(-1, 50.0, "DuracionEnDias invalida")]
        public async Task AddMembresia_ThrowsBadRequestException_WhenInvalidFields(int duracion, double costo, string expectedMessage)
        {
            var query = new Mock<MembresiaQuery>(null);
            var command = new Mock<MembresiaCommand>(null);

            var tipoQuery = new Mock<TipoDeMembresiaQuery>(null);
            var tipoCommand = new Mock<TipoDeMembresiaCommand>(null);
            var miembroQuery = new Mock<MiembroQuery>(null);
            var tipoService = new Mock<TipoDeMembresiaService>(tipoCommand.Object, tipoQuery.Object);

            var genericMapper = new GenericMapper();
            var mapper = new Mock<MembresiaMapper>(genericMapper);

            tipoService.Setup(s => s.GetTipoDeMembresiaById(It.IsAny<int>()))
                .ReturnsAsync(new TipoDeMembresia { TipoDeMembresiaId = 1, Descripcion = "Mes" });

            var service = new MembresiasService(query.Object, command.Object, tipoService.Object, mapper.Object, miembroQuery.Object);

            var request = new MembresiaRequest { TipoDeMembresiaId = 1, DuracionEnDias = duracion, CostoBase = (decimal)costo };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.AddMembresia(request));
            Assert.Contains(expectedMessage.Split(' ')[0], ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AddMembresia_ThrowsNotFoundException_WhenTipoDeMembresiaInvalid()
        {
            var query = new Mock<MembresiaQuery>(null);
            var command = new Mock<MembresiaCommand>(null);
            var miembroQuery = new Mock<MiembroQuery>(null);
            var tipoQuery = new Mock<TipoDeMembresiaQuery>(null);
            var tipoCommand = new Mock<TipoDeMembresiaCommand>(null);
            var tipoService = new Mock<TipoDeMembresiaService>(tipoCommand.Object, tipoQuery.Object);

            var genericMapper = new GenericMapper();
            var mapper = new Mock<MembresiaMapper>(genericMapper);

            tipoService.Setup(s => s.GetTipoDeMembresiaById(It.IsAny<int>())).ReturnsAsync((TipoDeMembresia?)null);

            var service = new MembresiasService(query.Object, command.Object, tipoService.Object, mapper.Object, miembroQuery.Object);

            var request = new MembresiaRequest { TipoDeMembresiaId = 999, DuracionEnDias = 30, CostoBase = 100m };

            await Assert.ThrowsAsync<NotFoundException>(() => service.AddMembresia(request));
        }

        [Fact]
        public async Task UpdateMembresia_ReturnsResponse_WhenRequestIsValid()
        {
            var query = new Mock<MembresiaQuery>(null);
            var command = new Mock<MembresiaCommand>(null);
            var miembroQuery = new Mock<MiembroQuery>(null);
            var tipoQuery = new Mock<TipoDeMembresiaQuery>(null);
            var tipoCommand = new Mock<TipoDeMembresiaCommand>(null);
            var tipoService = new Mock<TipoDeMembresiaService>(tipoCommand.Object, tipoQuery.Object);

            var genericMapper = new GenericMapper();
            var mapper = new Mock<MembresiaMapper>(genericMapper);

            var existing = new Membresia { MembresiaId = 1, TipoDeMembresiaId = 1, DuracionEnDias = 30, CostoBase = 100m };
            var tipo = new TipoDeMembresia { TipoDeMembresiaId = 1, Descripcion = "Mes" };
            var updated = new Membresia { MembresiaId = 1, TipoDeMembresiaId = 1, DuracionEnDias = 60, CostoBase = 180m };

            query.Setup(q => q.GetMembresiaById(1)).ReturnsAsync(existing);
            tipoService.Setup(s => s.GetTipoDeMembresiaById(1)).ReturnsAsync(tipo);
            command.Setup(c => c.UpdateMembresia(It.IsAny<Membresia>())).ReturnsAsync(updated);
            mapper.Setup(m => m.GetMembresiaResponse(updated)).ReturnsAsync(new MembresiaResponse { DuracionEnDias = 60, CostoBase = 180m });

            var service = new MembresiasService(query.Object, command.Object, tipoService.Object, mapper.Object, miembroQuery.Object);

            var request = new MembresiaRequest { TipoDeMembresiaId = 1, DuracionEnDias = 60, CostoBase = 180m };

            var result = await service.UpdateMembresia(1, request);

            Assert.NotNull(result);
            Assert.Equal(60, result.DuracionEnDias);
            Assert.Equal(180m, result.CostoBase);
        }

        [Fact]
        public async Task UpdateMembresia_ThrowsNotFoundException_WhenIdIsInvalid()
        {
            var query = new Mock<MembresiaQuery>(null);
            var command = new Mock<MembresiaCommand>(null);
            var miembroQuery = new Mock<MiembroQuery>(null);
            var tipoQuery = new Mock<TipoDeMembresiaQuery>(null);
            var tipoCommand = new Mock<TipoDeMembresiaCommand>(null);
            var tipoService = new Mock<TipoDeMembresiaService>(tipoCommand.Object, tipoQuery.Object);

            var genericMapper = new GenericMapper();
            var mapper = new Mock<MembresiaMapper>(genericMapper);

            query.Setup(q => q.GetMembresiaById(It.IsAny<int>())).ReturnsAsync((Membresia?)null);

            var service = new MembresiasService(query.Object, command.Object, tipoService.Object, mapper.Object, miembroQuery.Object);

            var request = new MembresiaRequest { TipoDeMembresiaId = 1, DuracionEnDias = 30, CostoBase = 100m };

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateMembresia(999, request));
        }

        [Fact]
        public async Task UpdateMembresia_ThrowsNotFoundException_WhenTipoDeMembresiaInvalid()
        {
            var query = new Mock<MembresiaQuery>(null);
            var command = new Mock<MembresiaCommand>(null);
            var miembroQuery = new Mock<MiembroQuery>(null);
            var tipoQuery = new Mock<TipoDeMembresiaQuery>(null);
            var tipoCommand = new Mock<TipoDeMembresiaCommand>(null);
            var tipoService = new Mock<TipoDeMembresiaService>(tipoCommand.Object, tipoQuery.Object);

            var genericMapper = new GenericMapper();
            var mapper = new Mock<MembresiaMapper>(genericMapper);

            var existing = new Membresia { MembresiaId = 1, TipoDeMembresiaId = 1, DuracionEnDias = 30, CostoBase = 100m };
            query.Setup(q => q.GetMembresiaById(1)).ReturnsAsync(existing);
            tipoService.Setup(s => s.GetTipoDeMembresiaById(It.IsAny<int>())).ReturnsAsync((TipoDeMembresia?)null);

            var service = new MembresiasService(query.Object, command.Object, tipoService.Object, mapper.Object, miembroQuery.Object);

            var request = new MembresiaRequest { TipoDeMembresiaId = 999, DuracionEnDias = 30, CostoBase = 100m };

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateMembresia(1, request));
        }

        [Theory]
        [InlineData(0, 100.0)]
        [InlineData(30, 0.0)]
        [InlineData(-5, 10.0)]
        public async Task UpdateMembresia_ThrowsBadRequestException_WhenInvalidFields(int duracion, double costo)
        {
            var query = new Mock<MembresiaQuery>(null);
            var command = new Mock<MembresiaCommand>(null);
            var miembroQuery = new Mock<MiembroQuery>(null);
            var tipoQuery = new Mock<TipoDeMembresiaQuery>(null);
            var tipoCommand = new Mock<TipoDeMembresiaCommand>(null);
            var tipoService = new Mock<TipoDeMembresiaService>(tipoCommand.Object, tipoQuery.Object);

            var genericMapper = new GenericMapper();
            var mapper = new Mock<MembresiaMapper>(genericMapper);

            var existing = new Membresia { MembresiaId = 1, TipoDeMembresiaId = 1, DuracionEnDias = 30, CostoBase = 100m };
            query.Setup(q => q.GetMembresiaById(1)).ReturnsAsync(existing);
            tipoService.Setup(s => s.GetTipoDeMembresiaById(It.IsAny<int>()))
                .ReturnsAsync(new TipoDeMembresia { TipoDeMembresiaId = 1, Descripcion = "Mes" });

            var service = new MembresiasService(query.Object, command.Object, tipoService.Object, mapper.Object, miembroQuery.Object);

            var request = new MembresiaRequest { TipoDeMembresiaId = 1, DuracionEnDias = duracion, CostoBase = (decimal)costo };

            await Assert.ThrowsAsync<BadRequestException>(() => service.UpdateMembresia(1, request));
        }
    }
}