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
    public class ClaseServiceTests
    {
        [Fact]
        public async Task GetAllClases_ReturnsList()
        {
            var claseQuery = new Mock<ClaseQuery>(null);
            var claseCommand = new Mock<ClaseCommand>(null);

            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new ActividadMapper();
            var actividadService = new Mock<ActividadService>(
                actividadQuery.Object,
                actividadCommand.Object,
                actividadMapper
            );

            var entrenadorQuery = new Mock<EntrenadorQuery>(null);
            var entrenadorCommand = new Mock<EntrenadorCommand>(null);
            var entrenadorMapper = new EntrenadorMapper();
            var entrenadorService = new Mock<EntrenadorService>(
                entrenadorQuery.Object,
                entrenadorCommand.Object,
                entrenadorMapper
            );

            var claseMapper = new ClaseMapper(entrenadorMapper, actividadMapper);

            claseQuery.Setup(q => q.GetAllClases()).ReturnsAsync(new List<Clase>());

            var service = new ClaseService(
                claseQuery.Object,
                claseCommand.Object,
                claseMapper,
                actividadService.Object,
                entrenadorService.Object
            );

            var result = await service.GetAllClases();

            Assert.NotNull(result);
            Assert.IsType<List<ClaseResponse>>(result);
        }

        [Fact]
        public async Task GetClaseById_ReturnsResponse_WhenIdIsValid()
        {
            var claseQuery = new Mock<ClaseQuery>(null);
            var claseCommand = new Mock<ClaseCommand>(null);

            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new ActividadMapper();
            var actividadService = new Mock<ActividadService>(
                actividadQuery.Object,
                actividadCommand.Object,
                actividadMapper
            );

            var entrenadorQuery = new Mock<EntrenadorQuery>(null);
            var entrenadorCommand = new Mock<EntrenadorCommand>(null);
            var entrenadorMapper = new EntrenadorMapper();
            var entrenadorService = new Mock<EntrenadorService>(
                entrenadorQuery.Object,
                entrenadorCommand.Object,
                entrenadorMapper
            );

            var claseMapper = new ClaseMapper(entrenadorMapper, actividadMapper);

            var fecha = DateTime.Today;
            var horaInicio = TimeOnly.FromTimeSpan(TimeSpan.FromHours(10));
            var horaFin = TimeOnly.FromTimeSpan(TimeSpan.FromHours(11));

            var entity = new Clase
            {
                ClaseId = 1,
                ActividadId = 1,
                Actividad = new Actividad { ActividadId = 1, Nombre = "Yoga", Descripcion = "Clase de yoga" },
                EntrenadorId = 2,
                Entrenador = new Entrenador
                {
                    Id = 2,
                    Nombre = "Pablo",
                    DNI = 12345678,
                    FechaNacimiento = new DateTime(1985, 5, 20),
                    Telefono = "555-0123",
                    Direccion = "Calle Principal 1",
                    Email = "pablo@example.com",
                    Foto = "pablo.jpg",
                    Certificacion = "Certificación A",
                    Activo = true,
                    Eliminado = false
                },
                Fecha = fecha,
                HoraInicio = horaInicio,
                HoraFin = horaFin,
                Cupo = 20
            };

            claseQuery.Setup(q => q.GetClaseById(1)).ReturnsAsync(entity);

            var service = new ClaseService(
                claseQuery.Object,
                claseCommand.Object,
                claseMapper,
                actividadService.Object,
                entrenadorService.Object
            );

            var result = await service.GetClaseById(1);

            Assert.NotNull(result);
            Assert.IsType<ClaseResponse>(result);
            Assert.Equal(20, result.Cupo);
            Assert.Equal(horaInicio, result.HoraInicio);
        }


        [Fact]
        public async Task GetClaseById_ThrowsNotFoundException_WhenIdInvalid()
        {
            var claseQuery = new Mock<ClaseQuery>(null);
            var claseCommand = new Mock<ClaseCommand>(null);

            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new ActividadMapper();
            var actividadService = new Mock<ActividadService>(
                actividadQuery.Object,
                actividadCommand.Object,
                actividadMapper
            );

            var entrenadorQuery = new Mock<EntrenadorQuery>(null);
            var entrenadorCommand = new Mock<EntrenadorCommand>(null);
            var entrenadorMapper = new EntrenadorMapper();
            var entrenadorService = new Mock<EntrenadorService>(
                entrenadorQuery.Object,
                entrenadorCommand.Object,
                entrenadorMapper
            );

            var claseMapper = new ClaseMapper(entrenadorMapper, actividadMapper);

            claseQuery.Setup(q => q.GetClaseById(It.IsAny<int>())).ReturnsAsync((Clase?)null);

            var service = new ClaseService(
                claseQuery.Object,
                claseCommand.Object,
                claseMapper,
                actividadService.Object,
                entrenadorService.Object
            );

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetClaseById(0));
            Assert.Equal("Id de clase invalido", ex.Message);
        }

        [Fact]
        public async Task AddClase_ReturnsResponse_WhenRequestIsValid()
        {

            var claseQuery = new Mock<ClaseQuery>(null);
            var claseCommand = new Mock<ClaseCommand>(null);


            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new ActividadMapper();


            var actividadService = new Mock<ActividadService>(
                actividadQuery.Object,
                actividadCommand.Object,
                actividadMapper
            );


            var entrenadorQuery = new Mock<EntrenadorQuery>(null);
            var entrenadorCommand = new Mock<EntrenadorCommand>(null);
            var entrenadorMapper = new EntrenadorMapper();


            var entrenadorService = new Mock<EntrenadorService>(
                entrenadorQuery.Object,
                entrenadorCommand.Object,
                entrenadorMapper
            );


            var claseMapper = new ClaseMapper(entrenadorMapper, actividadMapper);

            var fecha = DateTime.Today;
            var horaInicio = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9));
            var horaFin = TimeOnly.FromTimeSpan(TimeSpan.FromHours(10));

            var added = new Clase { ClaseId = 1 };
            var fetched = new Clase
            {
                ClaseId = 1,
                ActividadId = 1,
                Actividad = new Actividad { ActividadId = 1, Nombre = "Pilates", Descripcion = "Clase de pilates" },
                EntrenadorId = 2,
                Entrenador = new Entrenador
                {
                    Id = 2,
                    Nombre = "Lucia",
                    DNI = 87654321,
                    FechaNacimiento = new DateTime(1992, 7, 15),
                    Telefono = "555-5678",
                    Direccion = "Av. Secundaria 2",
                    Email = "lucia@example.com",
                    Foto = "lucia.jpg",
                    Certificacion = "Certificación B",
                    Activo = true,
                    Eliminado = false
                },
                Fecha = fecha,
                HoraInicio = horaInicio,
                HoraFin = horaFin,
                Cupo = 15
            };

            actividadService.Setup(s => s.GetActividadById(It.IsAny<int>()))
                .ReturnsAsync(new ActividadResponse { Nombre = "Pilates", Descripcion = "Clase de pilates" });

            entrenadorService.Setup(s => s.GetEntrenadorById(It.IsAny<int>()))
                .ReturnsAsync(new EntrenadorResponse
                {
                    Id = 2,
                    Nombre = "Lucia",
                    DNI = 87654321,
                    FechaNacimiento = new DateTime(1992, 7, 15),
                    Telefono = "555-5678",
                    Direccion = "Av. Secundaria 2",
                    Email = "lucia@example.com",
                    Foto = "lucia.jpg",
                    Eliminado = false,
                    Certificacion = "Certificación B",
                    Activo = true
                });

            claseCommand.Setup(c => c.AddClase(It.IsAny<Clase>())).ReturnsAsync(added);
            claseQuery.Setup(q => q.GetClaseById(1)).ReturnsAsync(fetched);


            var service = new ClaseService(
                claseQuery.Object,
                claseCommand.Object,
                claseMapper,
                actividadService.Object,
                entrenadorService.Object
            );

            var request = new ClaseRequest
            {
                ActividadId = 1,
                EntrenadorId = 2,
                Fecha = fecha,
                HoraInicio = horaInicio,
                HoraFin = horaFin,
                Cupo = 15
            };

            var result = await service.AddClaseAsync(request);

            Assert.NotNull(result);
            Assert.IsType<ClaseResponse>(result);
            Assert.Equal(15, result.Cupo);
        }

        [Fact]
        public async Task AddClase_ThrowsBadRequestException_WhenEntrenadorInvalid()
        {
            var claseQuery = new Mock<ClaseQuery>(null);
            var claseCommand = new Mock<ClaseCommand>(null);

            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new ActividadMapper();
            var actividadService = new Mock<ActividadService>(
                actividadQuery.Object,
                actividadCommand.Object,
                actividadMapper
            );

            var entrenadorQuery = new Mock<EntrenadorQuery>(null);
            var entrenadorCommand = new Mock<EntrenadorCommand>(null);
            var entrenadorMapper = new EntrenadorMapper();
            var entrenadorService = new Mock<EntrenadorService>(
                entrenadorQuery.Object,
                entrenadorCommand.Object,
                entrenadorMapper
            );

            var claseMapper = new ClaseMapper(entrenadorMapper, actividadMapper);

            actividadService.Setup(s => s.GetActividadById(It.IsAny<int>()))
                .ReturnsAsync(new ActividadResponse { Nombre = "Pilates", Descripcion = "Clase de pilates" });
            entrenadorService.Setup(s => s.GetEntrenadorById(It.IsAny<int>())).ReturnsAsync((EntrenadorResponse?)null);

            var service = new ClaseService(
                claseQuery.Object,
                claseCommand.Object,
                claseMapper,
                actividadService.Object,
                entrenadorService.Object
            );

            var request = new ClaseRequest
            {
                ActividadId = 1,
                EntrenadorId = 999,
                Fecha = DateTime.Today,
                HoraInicio = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)),
                HoraFin = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                Cupo = 10
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.AddClaseAsync(request));
            Assert.Equal("Datos de Entrenador invalidos", ex.Message);
        }


        [Fact]
        public async Task AddClase_ThrowsBadRequestException_WhenActividadInvalid()
        {
            var claseQuery = new Mock<ClaseQuery>(null);
            var claseCommand = new Mock<ClaseCommand>(null);

            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new ActividadMapper();

            var actividadService = new Mock<ActividadService>(
                actividadQuery.Object,
                actividadCommand.Object,
                actividadMapper
            );

            var entrenadorQuery = new Mock<EntrenadorQuery>(null);
            var entrenadorCommand = new Mock<EntrenadorCommand>(null);
            var entrenadorMapper = new EntrenadorMapper();

            var entrenadorService = new Mock<EntrenadorService>(
                entrenadorQuery.Object,
                entrenadorCommand.Object,
                entrenadorMapper
            );

            var claseMapper = new ClaseMapper(entrenadorMapper, actividadMapper);


            actividadService.Setup(s => s.GetActividadById(It.IsAny<int>()))
                .ReturnsAsync((ActividadResponse?)null);


            entrenadorService.Setup(s => s.GetEntrenadorById(It.IsAny<int>()))
                .ReturnsAsync(new EntrenadorResponse
                {
                    Id = 2,
                    Nombre = "Lucia",
                    DNI = 87654321,
                    FechaNacimiento = new DateTime(1992, 7, 15),
                    Telefono = "555-5678",
                    Direccion = "Av. Secundaria 2",
                    Email = "lucia@example.com",
                    Foto = "lucia.jpg",
                    Eliminado = false,
                    Certificacion = "Certificación B",
                    Activo = true
                });
            var service = new ClaseService(
                claseQuery.Object,
                claseCommand.Object,
                claseMapper,
                actividadService.Object,
                entrenadorService.Object
            );

            var request = new ClaseRequest
            {
                ActividadId = 999,
                EntrenadorId = 2,
                Fecha = DateTime.Today,
                HoraInicio = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)),
                HoraFin = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                Cupo = 10
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.AddClaseAsync(request));

            Assert.Equal("Datos de Actividad invalidos", ex.Message);
        }


        [Fact]
        public async Task UpdateClase_ReturnsResponse_WhenRequestIsValid()
        {
            var claseQuery = new Mock<ClaseQuery>(null);
            var claseCommand = new Mock<ClaseCommand>(null);

            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new ActividadMapper();
            var actividadService = new Mock<ActividadService>(
                actividadQuery.Object,
                actividadCommand.Object,
                actividadMapper
            );

            var entrenadorQuery = new Mock<EntrenadorQuery>(null);
            var entrenadorCommand = new Mock<EntrenadorCommand>(null);
            var entrenadorMapper = new EntrenadorMapper();
            var entrenadorService = new Mock<EntrenadorService>(
                entrenadorQuery.Object,
                entrenadorCommand.Object,
                entrenadorMapper
            );

            var claseMapper = new ClaseMapper(entrenadorMapper, actividadMapper);

            var fecha = DateTime.Today;
            var horaInicio = TimeOnly.FromTimeSpan(TimeSpan.FromHours(7));
            var horaFinOld = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8));
            var horaFinNew = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9));

            var existing = new Clase
            {
                ClaseId = 1,
                ActividadId = 1,
                EntrenadorId = 2,
                Fecha = fecha,
                HoraInicio = horaInicio,
                HoraFin = horaFinOld,
                Cupo = 10,
                Actividad = new Actividad { ActividadId = 1, Nombre = "A", Descripcion = "Descripción A" },
                Entrenador = new Entrenador
                {
                    Id = 2,
                    Nombre = "E",
                    DNI = 55500011,
                    FechaNacimiento = new DateTime(1988, 3, 10),
                    Telefono = "555-0001",
                    Direccion = "Calle Tercera 3",
                    Email = "e@example.com",
                    Foto = "e.jpg",
                    Certificacion = "Certificación E",
                    Activo = true,
                    Eliminado = false
                }
            };

            var updated = new Clase
            {
                ClaseId = 1,
                ActividadId = 1,
                EntrenadorId = 2,
                Fecha = fecha,
                HoraInicio = horaInicio,
                HoraFin = horaFinNew,
                Cupo = 20,
                Actividad = existing.Actividad,
                Entrenador = existing.Entrenador
            };

            claseQuery.SetupSequence(q => q.GetClaseById(1))
                .ReturnsAsync(existing)
                .ReturnsAsync(existing)
                .ReturnsAsync(updated);

            claseCommand.Setup(c => c.UpdateClase(It.IsAny<Clase>())).ReturnsAsync(updated);

            actividadService.Setup(s => s.GetActividadById(It.IsAny<int>()))
                .ReturnsAsync(new ActividadResponse { Nombre = "A", Descripcion = "Descripción A" });

            entrenadorService.Setup(s => s.GetEntrenadorById(It.IsAny<int>()))
                .ReturnsAsync(new EntrenadorResponse
                {
                    Id = 2,
                    Nombre = "E",
                    DNI = 55500011,
                    FechaNacimiento = new DateTime(1988, 3, 10),
                    Telefono = "555-0001",
                    Direccion = "Calle Tercera 3",
                    Email = "e@example.com",
                    Foto = "e.jpg",
                    Eliminado = false,
                    Certificacion = "Certificación E",
                    Activo = true
                });

            var service = new ClaseService(
                claseQuery.Object,
                claseCommand.Object,
                claseMapper,
                actividadService.Object,
                entrenadorService.Object
            );

            var request = new ClaseRequest
            {
                ActividadId = 1,
                EntrenadorId = 2,
                Fecha = fecha,
                HoraInicio = horaInicio,
                HoraFin = horaFinNew,
                Cupo = 20
            };

            var result = await service.UpdateClase(1, request);

            Assert.NotNull(result);
            Assert.Equal(horaFinNew, result.HoraFin);
            Assert.Equal(20, result.Cupo);
        }

        [Fact]
        public async Task UpdateClase_ThrowsNotFoundException_WhenIdIsInvalid()
        {
            var claseQuery = new Mock<ClaseQuery>(null);
            var claseCommand = new Mock<ClaseCommand>(null);

            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new ActividadMapper();
            var actividadService = new Mock<ActividadService>(
                actividadQuery.Object,
                actividadCommand.Object,
                actividadMapper
            );

            var entrenadorQuery = new Mock<EntrenadorQuery>(null);
            var entrenadorCommand = new Mock<EntrenadorCommand>(null);
            var entrenadorMapper = new EntrenadorMapper();
            var entrenadorService = new Mock<EntrenadorService>(
                entrenadorQuery.Object,
                entrenadorCommand.Object,
                entrenadorMapper
            );

            var claseMapper = new ClaseMapper(entrenadorMapper, actividadMapper);

            claseQuery.Setup(q => q.GetClaseById(It.IsAny<int>())).ReturnsAsync((Clase?)null);

            var service = new ClaseService(
                claseQuery.Object,
                claseCommand.Object,
                claseMapper,
                actividadService.Object,
                entrenadorService.Object
            );

            var request = new ClaseRequest
            {
                ActividadId = 1,
                EntrenadorId = 2,
                Fecha = DateTime.Today,
                HoraInicio = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)),
                HoraFin = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                Cupo = 10
            };

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateClase(999, request));
        }

        [Fact]
        public async Task UpdateClase_ThrowsBadRequestException_WhenEntrenadorInvalid()
        {
            var claseQuery = new Mock<ClaseQuery>(null);
            var claseCommand = new Mock<ClaseCommand>(null);

            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new ActividadMapper();
            var actividadService = new Mock<ActividadService>(
                actividadQuery.Object,
                actividadCommand.Object,
                actividadMapper
            );

            var entrenadorQuery = new Mock<EntrenadorQuery>(null);
            var entrenadorCommand = new Mock<EntrenadorCommand>(null);
            var entrenadorMapper = new EntrenadorMapper();
            var entrenadorService = new Mock<EntrenadorService>(
                entrenadorQuery.Object,
                entrenadorCommand.Object,
                entrenadorMapper
            );

            var claseMapper = new ClaseMapper(entrenadorMapper, actividadMapper);

            var existing = new Clase
            {
                ClaseId = 1,
                ActividadId = 1,
                EntrenadorId = 2,
                Fecha = DateTime.Today,
                HoraInicio = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)),
                HoraFin = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                Cupo = 10,
                Actividad = new Actividad { ActividadId = 1, Nombre = "A", Descripcion = "Descripción A" },
                Entrenador = new Entrenador
                {
                    Id = 2,
                    Nombre = "E",
                    DNI = 55500011,
                    FechaNacimiento = new DateTime(1988, 3, 10),
                    Telefono = "555-0001",
                    Direccion = "Calle Tercera 3",
                    Email = "e@example.com",
                    Foto = "e.jpg",
                    Certificacion = "Certificación E",
                    Activo = true,
                    Eliminado = false
                }
            };

            claseQuery.Setup(q => q.GetClaseById(1)).ReturnsAsync(existing);
            actividadService.Setup(s => s.GetActividadById(It.IsAny<int>()))
                .ReturnsAsync(new ActividadResponse { Nombre = "A", Descripcion = "Descripción A" });
            entrenadorService.Setup(s => s.GetEntrenadorById(It.IsAny<int>())).ReturnsAsync((EntrenadorResponse?)null);

            var service = new ClaseService(
                claseQuery.Object,
                claseCommand.Object,
                claseMapper,
                actividadService.Object,
                entrenadorService.Object
            );

            var request = new ClaseRequest
            {
                ActividadId = 1,
                EntrenadorId = 999,
                Fecha = DateTime.Today,
                HoraInicio = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)),
                HoraFin = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                Cupo = 10
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.UpdateClase(1, request));
            Assert.Equal("Datos de Entrenador invalidos", ex.Message);
        }

        [Fact]
        public async Task UpdateClase_ThrowsBadRequestException_WhenActividadInvalid()
        {
            var claseQuery = new Mock<ClaseQuery>(null);
            var claseCommand = new Mock<ClaseCommand>(null);

            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new ActividadMapper();
            var actividadService = new Mock<ActividadService>(
                actividadQuery.Object,
                actividadCommand.Object,
                actividadMapper
            );

            var entrenadorQuery = new Mock<EntrenadorQuery>(null);
            var entrenadorCommand = new Mock<EntrenadorCommand>(null);
            var entrenadorMapper = new EntrenadorMapper();
            var entrenadorService = new Mock<EntrenadorService>(
                entrenadorQuery.Object,
                entrenadorCommand.Object,
                entrenadorMapper
            );

            var claseMapper = new ClaseMapper(entrenadorMapper, actividadMapper);

            var existing = new Clase
            {
                ClaseId = 1,
                ActividadId = 1,
                EntrenadorId = 2,
                Fecha = DateTime.Today,
                HoraInicio = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)),
                HoraFin = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                Cupo = 10,
                Actividad = new Actividad { ActividadId = 1, Nombre = "A", Descripcion = "Descripción A" },
                Entrenador = new Entrenador
                {
                    Id = 2,
                    Nombre = "E",
                    DNI = 55500011,
                    FechaNacimiento = new DateTime(1988, 3, 10),
                    Telefono = "555-0001",
                    Direccion = "Calle Tercera 3",
                    Email = "e@example.com",
                    Foto = "e.jpg",
                    Certificacion = "Certificación E",
                    Activo = true,
                    Eliminado = false
                }
            };

            claseQuery.Setup(q => q.GetClaseById(1)).ReturnsAsync(existing);
            actividadService.Setup(s => s.GetActividadById(It.IsAny<int>())).ReturnsAsync((ActividadResponse?)null);
            entrenadorService.Setup(s => s.GetEntrenadorById(It.IsAny<int>()))
                .ReturnsAsync(new EntrenadorResponse
                {
                    Id = 2,
                    Nombre = "E",
                    DNI = 55500011,
                    FechaNacimiento = new DateTime(1988, 3, 10),
                    Telefono = "555-0001",
                    Direccion = "Calle Tercera 3",
                    Email = "e@example.com",
                    Foto = "e.jpg",
                    Eliminado = false,
                    Certificacion = "Certificación E",
                    Activo = true
                });

            var service = new ClaseService(
                claseQuery.Object,
                claseCommand.Object,
                claseMapper,
                actividadService.Object,
                entrenadorService.Object
            );

            var request = new ClaseRequest
            {
                ActividadId = 999,
                EntrenadorId = 2,
                Fecha = DateTime.Today,
                HoraInicio = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)),
                HoraFin = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                Cupo = 10
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.UpdateClase(1, request));
            Assert.Equal("Datos de Actividad invalidos", ex.Message);
        }

        [Fact]
        public async Task DeleteClase_ReturnsResponse_WhenIdIsValid()
        {
            var claseQuery = new Mock<ClaseQuery>(null);
            var claseCommand = new Mock<ClaseCommand>(null);

            var actividadQuery = new Mock<ActividadQuery>(null);
            var actividadCommand = new Mock<ActividadCommand>(null);
            var actividadMapper = new ActividadMapper();
            var actividadService = new Mock<ActividadService>(
                actividadQuery.Object,
                actividadCommand.Object,
                actividadMapper
            );

            var entrenadorQuery = new Mock<EntrenadorQuery>(null);
            var entrenadorCommand = new Mock<EntrenadorCommand>(null);
            var entrenadorMapper = new EntrenadorMapper();
            var entrenadorService = new Mock<EntrenadorService>(
                entrenadorQuery.Object,
                entrenadorCommand.Object,
                entrenadorMapper
            );

            var claseMapper = new ClaseMapper(entrenadorMapper, actividadMapper);

            var fecha = DateTime.Today;
            var horaInicio = TimeOnly.FromTimeSpan(TimeSpan.FromHours(10));
            var horaFin = TimeOnly.FromTimeSpan(TimeSpan.FromHours(11));

            var entity = new Clase
            {
                ClaseId = 1,
                Actividad = new Actividad { ActividadId = 1, Nombre = "Yoga", Descripcion = "Clase de yoga" },
                Entrenador = new Entrenador
                {
                    Id = 2,
                    Nombre = "Pablo",
                    DNI = 12345678,
                    FechaNacimiento = new DateTime(1985, 5, 20),
                    Telefono = "555-0123",
                    Direccion = "Calle Principal 1",
                    Email = "pablo@example.com",
                    Foto = "pablo.jpg",
                    Certificacion = "Certificación A",
                    Activo = true,
                    Eliminado = false
                },
                Fecha = fecha,
                HoraInicio = horaInicio,
                HoraFin = horaFin,
                Cupo = 20
            };

            claseQuery.Setup(q => q.GetClaseById(1)).ReturnsAsync(entity);
            claseCommand.Setup(c => c.DeleteClase(entity)).Returns(Task.CompletedTask);

            var service = new ClaseService(
                claseQuery.Object,
                claseCommand.Object,
                claseMapper,
                actividadService.Object,
                entrenadorService.Object
            );

            var result = await service.DeleteClase(1);

            Assert.NotNull(result);
            Assert.IsType<ClaseResponse>(result);
            Assert.Equal(20, result.Cupo);
        }

    }
}