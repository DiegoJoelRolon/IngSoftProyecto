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
    public class ActividadServiceTests
    {

        [Fact]
        public async Task GetAllActividades_ReturnsList()
        {
            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new Mock<ActividadMapper>();
            actividadQuery.Setup(q => q.GetAllActividades()).ReturnsAsync(new List<Actividad>());
            actividadMapper.Setup(m => m.GetAllActividadesResponse(It.IsAny<List<Actividad>>()))
                .ReturnsAsync(new List<ActividadResponse>());
            var service = new ActividadService(actividadQuery.Object, actividadCommand.Object, actividadMapper.Object);

            var result = await service.GetAllActividades();

            Assert.NotNull(result);
            Assert.IsType<List<ActividadResponse>>(result);
        }

        [Fact]
        public async Task AddActividad_ReturnsActividadResponse_WhenRequestIsValid()
        {
            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new Mock<ActividadMapper>();
            var actividad = new Actividad { Nombre = "Yoga", Descripcion = "Clase de yoga" };
            actividadCommand.Setup(c => c.AddActividad(It.IsAny<Actividad>())).Returns(Task.CompletedTask);
            actividadMapper.Setup(m => m.GetActividadResponse(It.IsAny<Actividad>()))
                .ReturnsAsync(new ActividadResponse { Nombre = "Yoga", Descripcion = "Clase de yoga" });
            var service = new ActividadService(actividadQuery.Object, actividadCommand.Object, actividadMapper.Object);

            var request = new ActividadRequest { Nombre = "Yoga", Descripcion = "Clase de yoga" };
            var result = await service.AddActividad(request);

            Assert.NotNull(result);
            Assert.Equal("Yoga", result.Nombre);
            Assert.Equal("Clase de yoga", result.Descripcion);
        }


        [Theory]
        [InlineData(null, "Clase de yoga")]
        [InlineData("", "Clase de yoga")]
        [InlineData("Yoga", null)]
        [InlineData("Yoga", "")]
        public async Task AddActividad_ThrowsBadRequestException_WhenInvalidFields(string nombre, string descripcion)
        {
            // Arrange
            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new Mock<ActividadMapper>();
            var service = new ActividadService(actividadQuery.Object, actividadCommand.Object, actividadMapper.Object);

            var request = new ActividadRequest { Nombre = nombre!, Descripcion = descripcion! };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => service.AddActividad(request));
            Assert.Equal("Nombre o descripcion no pueden estar vacios", exception.Message);
        }




        [Fact]
        public async Task GetActividadById_ReturnsActividadResponse_WhenIdExists()
        {
            // Arrange
            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new Mock<ActividadMapper>();
            var actividad = new Actividad { ActividadId = 1, Nombre = "Yoga", Descripcion = "Clase de yoga" };
            var actividadResponse = new ActividadResponse { Nombre = "Yoga", Descripcion = "Clase de yoga" };

            actividadQuery.Setup(q => q.GetActividadById(1)).ReturnsAsync(actividad);
            actividadMapper.Setup(m => m.GetActividadResponse(actividad)).ReturnsAsync(actividadResponse);

            var service = new ActividadService(actividadQuery.Object, actividadCommand.Object, actividadMapper.Object);

            // Act
            var result = await service.GetActividadById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Yoga", result.Nombre);
            Assert.Equal("Clase de yoga", result.Descripcion);
        }

        [Fact]
        public async Task GetActividadById_ThrowsNotFoundException_WhenIdDoesNotExist()
        {
            // Arrange
            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new Mock<ActividadMapper>();

            actividadQuery.Setup(q => q.GetActividadById(99)).ReturnsAsync((Actividad?)null);

            var service = new ActividadService(actividadQuery.Object, actividadCommand.Object, actividadMapper.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.GetActividadById(99));
            Assert.Equal("Id de miembro invalido", exception.Message);
        }

    }
}