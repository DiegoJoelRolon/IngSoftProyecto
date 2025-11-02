using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Models;
using IngSoftProyecto.Services;
using Moq;
using Xunit;

namespace Tests
{
    public class TipoDeMembresiaServiceTests
    {
        [Fact]
        public async Task GetAllTiposDeMembresias_ReturnsList()
        {
            var query = new Mock<TipoDeMembresiaQuery>(null);
            var command = new Mock<TipoDeMembresiaCommand>(null);

            query.Setup(q => q.GetAllTiposDeMembresias()).ReturnsAsync(new List<TipoDeMembresia>());

            var service = new TipoDeMembresiaService(command.Object, query.Object);

            var result = await service.GetAllTiposDeMembresias();

            Assert.NotNull(result);
            Assert.IsType<List<TipoDeMembresia>>(result);
        }

        [Fact]
        public async Task GetTipoDeMembresiaById_ReturnsTipo_WhenIdValid()
        {
            var query = new Mock<TipoDeMembresiaQuery>(null);
            var command = new Mock<TipoDeMembresiaCommand>(null);

            var tipo = new TipoDeMembresia { TipoDeMembresiaId = 1, Descripcion = "Mensual" };

            query.Setup(q => q.GetTipoDeMembresiaById(1)).ReturnsAsync(tipo);

            var service = new TipoDeMembresiaService(command.Object, query.Object);

            var result = await service.GetTipoDeMembresiaById(1);

            Assert.NotNull(result);
            Assert.Equal("Mensual", result!.Descripcion);
            Assert.Equal(1, result.TipoDeMembresiaId);
        }

        [Fact]
        public async Task GetTipoDeMembresiaById_ThrowsNotFoundException_WhenIdInvalid()
        {
            var query = new Mock<TipoDeMembresiaQuery>(null);
            var command = new Mock<TipoDeMembresiaCommand>(null);

            // devuelve null para cualquier id
            query.Setup(q => q.GetTipoDeMembresiaById(It.IsAny<int>())).ReturnsAsync((TipoDeMembresia?)null);

            var service = new TipoDeMembresiaService(command.Object, query.Object);

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetTipoDeMembresiaById(0));
            Assert.Contains("Id de tipo de membresia invalido", ex.Message, System.StringComparison.OrdinalIgnoreCase);

            // y para id > 0 cuando no existe
            var ex2 = await Assert.ThrowsAsync<NotFoundException>(() => service.GetTipoDeMembresiaById(99));
            Assert.Contains("Id de tipo de membresia invalido", ex2.Message, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AddTipoDeMembresia_ReturnsTipo_WhenRequestIsValid()
        {
            var query = new Mock<TipoDeMembresiaQuery>(null);
            var command = new Mock<TipoDeMembresiaCommand>(null);

            var tipo = new TipoDeMembresia { TipoDeMembresiaId = 1, Descripcion = "Anual" };

            command.Setup(c => c.AddTipoDeMembresia(It.IsAny<TipoDeMembresia>())).ReturnsAsync(tipo);

            var service = new TipoDeMembresiaService(command.Object, query.Object);

            var request = new TipoDeMembresia { Descripcion = "Anual" };
            var result = await service.AddTipoDeMembresia(request);

            Assert.NotNull(result);
            Assert.Equal("Anual", result.Descripcion);
            Assert.Equal(1, result.TipoDeMembresiaId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task AddTipoDeMembresia_ThrowsBadRequestException_WhenDescripcionInvalid(string descripcion)
        {
            var query = new Mock<TipoDeMembresiaQuery>(null);
            var command = new Mock<TipoDeMembresiaCommand>(null);

            var service = new TipoDeMembresiaService(command.Object, query.Object);

            var request = new TipoDeMembresia { Descripcion = descripcion! };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.AddTipoDeMembresia(request));
            Assert.Contains("Descripcion no puede estar vacio", ex.Message, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task UpdateTipoDeMembresia_Succeeds_WhenRequestIsValid()
        {
            var query = new Mock<TipoDeMembresiaQuery>(null);
            var command = new Mock<TipoDeMembresiaCommand>(null);

            var existing = new TipoDeMembresia { TipoDeMembresiaId = 1, Descripcion = "Antigua" };

            query.Setup(q => q.GetTipoDeMembresiaById(1)).ReturnsAsync(existing);
            command.Setup(c => c.UpdateTipoDeMembresia(It.IsAny<TipoDeMembresia>())).Returns(Task.CompletedTask);

            var service = new TipoDeMembresiaService(command.Object, query.Object);

            var request = new TipoDeMembresia { TipoDeMembresiaId = 1, Descripcion = "Actualizada" };

            await service.UpdateTipoDeMembresia(request);

            command.Verify(c => c.UpdateTipoDeMembresia(It.Is<TipoDeMembresia>(t => t.TipoDeMembresiaId == 1 && t.Descripcion == "Actualizada")), Times.Once);
        }

        [Fact]
        public async Task UpdateTipoDeMembresia_ThrowsNotFoundException_WhenIdInvalid()
        {
            var query = new Mock<TipoDeMembresiaQuery>(null);
            var command = new Mock<TipoDeMembresiaCommand>(null);

            query.Setup(q => q.GetTipoDeMembresiaById(It.IsAny<int>())).ReturnsAsync((TipoDeMembresia?)null);

            var service = new TipoDeMembresiaService(command.Object, query.Object);

            var request = new TipoDeMembresia { TipoDeMembresiaId = 999, Descripcion = "X" };

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateTipoDeMembresia(request));
        }

        [Fact]
        public async Task DeleteTipoDeMembresia_Succeeds_WhenRequestIsValid()
        {
            var query = new Mock<TipoDeMembresiaQuery>(null);
            var command = new Mock<TipoDeMembresiaCommand>(null);

            var existing = new TipoDeMembresia { TipoDeMembresiaId = 2, Descripcion = "Temporal" };

            query.Setup(q => q.GetTipoDeMembresiaById(2)).ReturnsAsync(existing);
            command.Setup(c => c.DeleteTipoDeMembresia(It.IsAny<TipoDeMembresia>())).Returns(Task.CompletedTask);

            var service = new TipoDeMembresiaService(command.Object, query.Object);

            var request = new TipoDeMembresia { TipoDeMembresiaId = 2, Descripcion = "Temporal" };

            await service.DeleteTipoDeMembresia(request);

            command.Verify(c => c.DeleteTipoDeMembresia(It.Is<TipoDeMembresia>(t => t.TipoDeMembresiaId == 2)), Times.Once);
        }

        [Fact]
        public async Task DeleteTipoDeMembresia_ThrowsNotFoundException_WhenIdInvalid()
        {
            var query = new Mock<TipoDeMembresiaQuery>(null);
            var command = new Mock<TipoDeMembresiaCommand>(null);

            query.Setup(q => q.GetTipoDeMembresiaById(It.IsAny<int>())).ReturnsAsync((TipoDeMembresia?)null);

            var service = new TipoDeMembresiaService(command.Object, query.Object);

            var request = new TipoDeMembresia { TipoDeMembresiaId = 999, Descripcion = "X" };

            await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteTipoDeMembresia(request));
        }
    }
}