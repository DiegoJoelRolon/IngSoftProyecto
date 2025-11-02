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
    public class MiembroXClaseServiceTests
    {
        [Fact]
        public async Task GetAllMiembrosXClases_ReturnsList()
        {
            var command = new Mock<MiembroXClaseCommand>(null);
            var query = new Mock<MiembroXClaseQuery>(null);
            var mapper = new Mock<MiembroXClaseMapper>(null, null);
            var miembroService = new Mock<MiembroService>(null, null, null, null, null, null, null);
            var claseService = new Mock<ClaseService>(null, null, null, null, null);

            query.Setup(q => q.GetAllMiembrosXClases()).ReturnsAsync(new List<MiembroXClase>());
            mapper.Setup(m => m.GetMiembroXClaseResponseList(It.IsAny<List<MiembroXClase>>()))
                .ReturnsAsync(new List<MiembroXClaseResponse>());

            var service = new MiembroXClaseService(command.Object, query.Object, mapper.Object, miembroService.Object, claseService.Object);

            var result = await service.GetAllMiembrosXClases();

            Assert.NotNull(result);
            Assert.IsType<List<MiembroXClaseResponse>>(result);
        }

        [Fact]
        public async Task GetMiembroXClaseById_ReturnsResponse_WhenIdIsValid()
        {
            var command = new Mock<MiembroXClaseCommand>(null);
            var query = new Mock<MiembroXClaseQuery>(null);
            var mapper = new Mock<MiembroXClaseMapper>(null, null);
            var miembroService = new Mock<MiembroService>(null, null, null, null, null, null, null);
            var claseService = new Mock<ClaseService>(null, null, null, null, null);

            var fechaInscripcion = DateTime.Today.AddDays(-5);

            var entity = new MiembroXClase
            {
                MiembroXClaseId = 1,
                MiembroId = 1,
                ClaseId = 2,
                FechaInscripcion = fechaInscripcion,
                Miembro = new Miembro
                {
                    Id = 1,
                    TipoDeMiembroId = 1,
                    DNI = 12345678,
                    Nombre = "Ana",
                    Direccion = "Calle Falsa 123",
                    Telefono = "123456789",
                    FechaNacimiento = DateTime.Today.AddYears(-30),
                    Email = "ana@example.com",
                    Foto = "foto.jpg",
                    Eliminado = false
                },
                Clase = new Clase
                {
                    ClaseId = 2,
                    ActividadId = 1,
                    EntrenadorId = 1,
                    Fecha = DateTime.Today,
                    HoraInicio = new TimeOnly(9, 0),
                    HoraFin = new TimeOnly(10, 0),
                    Cupo = 20
                }
            };

            var response = new MiembroXClaseResponse
            {
                MiembroXClaseId = 1,
                FechaInscripcion = fechaInscripcion,
                Miembro = new MiembroResponse
                {
                    Id = 1,
                    TipoDeMiembroId = 1,
                    TipoDeMiembro = new TipoDeMiembroResponse { Descripcion = "Regular", PorcentajeDescuento = 0 },
                    Nombre = "Ana",
                    Direccion = "Calle Falsa 123",
                    Telefono = "123456789",
                    FechaNacimiento = DateTime.Today.AddYears(-30),
                    Email = "ana@example.com",
                    Foto = "foto.jpg",
                    Eliminado = false,
                    Entrenador = null
                },
                Clase = new ClaseResponse
                {
                    ClaseId = 2,
                    Actividad = new ActividadResponse { Nombre = "Actividad", Descripcion = "Desc" },
                    Entrenador = new EntrenadorResponse
                    {
                        Id = 1,
                        Nombre = "Entr",
                        DNI = 11111111,
                        FechaNacimiento = DateTime.Today.AddYears(-40),
                        Telefono = "987654321",
                        Direccion = "Direccion Entr",
                        Email = "entr@example.com",
                        Foto = "fotoEntr.jpg",
                        Eliminado = false,
                        Certificacion = "Cert",
                        Activo = true
                    },
                    Fecha = DateTime.Today,
                    HoraInicio = new TimeOnly(9, 0),
                    HoraFin = new TimeOnly(10, 0),
                    Cupo = 20
                }
            };

            query.Setup(q => q.GetMiembroXClaseById(1)).ReturnsAsync(entity);
            mapper.Setup(m => m.GetMiembroXClaseResponse(entity)).ReturnsAsync(response);

            var service = new MiembroXClaseService(command.Object, query.Object, mapper.Object, miembroService.Object, claseService.Object);

            var result = await service.GetMiembroXClaseById(1);

            Assert.NotNull(result);
            Assert.IsType<MiembroXClaseResponse>(result);
            Assert.Equal(fechaInscripcion, result.FechaInscripcion);
        }

        [Fact]
        public async Task GetMiembroXClaseById_ThrowsNotFoundException_WhenIdInvalid()
        {
            var command = new Mock<MiembroXClaseCommand>(null);
            var query = new Mock<MiembroXClaseQuery>(null);
            var mapper = new Mock<MiembroXClaseMapper>(null, null);
            var miembroService = new Mock<MiembroService>(null, null, null, null, null, null, null);
            var claseService = new Mock<ClaseService>(null, null, null, null, null);

            query.Setup(q => q.GetMiembroXClaseById(It.IsAny<int>())).ReturnsAsync((MiembroXClase?)null);

            var service = new MiembroXClaseService(command.Object, query.Object, mapper.Object, miembroService.Object, claseService.Object);

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetMiembroXClaseById(0));
            Assert.Equal("Id de MiembroXClase invalido", ex.Message);
        }

        [Fact]
        public async Task AddMiembroXClase_ReturnsResponse_WhenRequestIsValid()
        {
            var command = new Mock<MiembroXClaseCommand>(null);
            var query = new Mock<MiembroXClaseQuery>(null);
            var mapper = new Mock<MiembroXClaseMapper>(null, null);
            var miembroService = new Mock<MiembroService>(null, null, null, null, null, null,null);
            var claseService = new Mock<ClaseService>(null, null, null, null, null);

            var fechaInscripcion = DateTime.Today.AddDays(-1);

            var added = new MiembroXClase { MiembroXClaseId = 1, FechaInscripcion = fechaInscripcion };
            var fetched = new MiembroXClase
            {
                MiembroXClaseId = 1,
                MiembroId = 1,
                ClaseId = 2,
                FechaInscripcion = fechaInscripcion,
                Miembro = new Miembro
                {
                    Id = 1,
                    TipoDeMiembroId = 1,
                    DNI = 12345678,
                    Nombre = "Ana",
                    Direccion = "Calle Falsa 123",
                    Telefono = "123456789",
                    FechaNacimiento = DateTime.Today.AddYears(-30),
                    Email = "ana@example.com",
                    Foto = "foto.jpg",
                    Eliminado = false
                },
                Clase = new Clase
                {
                    ClaseId = 2,
                    ActividadId = 1,
                    EntrenadorId = 1,
                    Fecha = DateTime.Today,
                    HoraInicio = new TimeOnly(9, 0),
                    HoraFin = new TimeOnly(10, 0),
                    Cupo = 10
                }
            };
            var response = new MiembroXClaseResponse
            {
                MiembroXClaseId = 1,
                FechaInscripcion = fechaInscripcion,
                Miembro = new MiembroResponse
                {
                    Id = 1,
                    TipoDeMiembroId = 1,
                    TipoDeMiembro = new TipoDeMiembroResponse { Descripcion = "Regular", PorcentajeDescuento = 0 },
                    Nombre = "Ana",
                    Direccion = "Calle Falsa 123",
                    Telefono = "123456789",
                    FechaNacimiento = DateTime.Today.AddYears(-30),
                    Email = "ana@example.com",
                    Foto = "foto.jpg",
                    Eliminado = false,
                    Entrenador = null
                },
                Clase = new ClaseResponse
                {
                    ClaseId = 2,
                    Actividad = new ActividadResponse { Nombre = "Actividad", Descripcion = "Desc" },
                    Entrenador = new EntrenadorResponse
                    {
                        Id = 1,
                        Nombre = "Entr",
                        DNI = 11111111,
                        FechaNacimiento = DateTime.Today.AddYears(-40),
                        Telefono = "987654321",
                        Direccion = "Direccion Entr",
                        Email = "entr@example.com",
                        Foto = "fotoEntr.jpg",
                        Eliminado = false,
                        Certificacion = "Cert",
                        Activo = true
                    },
                    Fecha = DateTime.Today,
                    HoraInicio = new TimeOnly(9, 0),
                    HoraFin = new TimeOnly(10, 0),
                    Cupo = 10
                }
            };

            miembroService.Setup(s => s.GetMiembroById(It.IsAny<int>())).ReturnsAsync(new MiembroResponse
            {
                Id = 1,
                TipoDeMiembroId = 1,
                TipoDeMiembro = new TipoDeMiembroResponse { Descripcion = "Regular", PorcentajeDescuento = 0 },
                Nombre = "Ana",
                Direccion = "Calle Falsa 123",
                Telefono = "123456789",
                FechaNacimiento = DateTime.Today.AddYears(-30),
                Email = "ana@example.com",
                Foto = "foto.jpg",
                Eliminado = false,
                Entrenador = null
            });
            claseService.Setup(s => s.GetClaseById(It.IsAny<int>())).ReturnsAsync(new ClaseResponse
            {
                ClaseId = 2,
                Actividad = new ActividadResponse { Nombre = "Actividad", Descripcion = "Desc" },
                Entrenador = new EntrenadorResponse
                {
                    Id = 1,
                    Nombre = "Entr",
                    DNI = 11111111,
                    FechaNacimiento = DateTime.Today.AddYears(-40),
                    Telefono = "987654321",
                    Direccion = "Direccion Entr",
                    Email = "entr@example.com",
                    Foto = "fotoEntr.jpg",
                    Eliminado = false,
                    Certificacion = "Cert",
                    Activo = true
                },
                Fecha = DateTime.Today,
                HoraInicio = new TimeOnly(9, 0),
                HoraFin = new TimeOnly(10, 0),
                Cupo = 10
            });

            command.Setup(c => c.AddMiembroXClase(It.IsAny<MiembroXClase>())).ReturnsAsync(added);
            query.Setup(q => q.GetMiembroXClaseById(1)).ReturnsAsync(fetched);
            mapper.Setup(m => m.GetMiembroXClaseResponse(fetched)).ReturnsAsync(response);

            var service = new MiembroXClaseService(command.Object, query.Object, mapper.Object, miembroService.Object, claseService.Object);

            var request = new MiembroXClaseRequest
            {
                MiembroId = 1,
                ClaseId = 2,
                FechaInscripcion = fechaInscripcion
            };

            var result = await service.AddMiembroXClase(request);

            Assert.NotNull(result);
            Assert.IsType<MiembroXClaseResponse>(result);
            Assert.Equal(fechaInscripcion, result.FechaInscripcion);
        }

        [Fact]
        public async Task AddMiembroXClase_ThrowsBadRequestException_WhenMiembroInvalid()
        {
            var command = new Mock<MiembroXClaseCommand>(null);
            var query = new Mock<MiembroXClaseQuery>(null);
            var mapper = new Mock<MiembroXClaseMapper>(null, null);
            var miembroService = new Mock<MiembroService>(null, null, null, null, null, null, null);
            var claseService = new Mock<ClaseService>(null, null, null, null, null);

            miembroService.Setup(s => s.GetMiembroById(It.IsAny<int>())).ReturnsAsync((MiembroResponse?)null);
            claseService.Setup(s => s.GetClaseById(It.IsAny<int>())).ReturnsAsync(new ClaseResponse
            {
                ClaseId = 2,
                Actividad = new ActividadResponse { Nombre = "Actividad", Descripcion = "Desc" },
                Entrenador = new EntrenadorResponse
                {
                    Id = 1,
                    Nombre = "Entr",
                    DNI = 11111111,
                    FechaNacimiento = DateTime.Today.AddYears(-40),
                    Telefono = "987654321",
                    Direccion = "Direccion Entr",
                    Email = "entr@example.com",
                    Foto = "fotoEntr.jpg",
                    Eliminado = false,
                    Certificacion = "Cert",
                    Activo = true
                },
                Fecha = DateTime.Today,
                HoraInicio = new TimeOnly(9, 0),
                HoraFin = new TimeOnly(10, 0),
                Cupo = 10
            });

            var service = new MiembroXClaseService(command.Object, query.Object, mapper.Object, miembroService.Object, claseService.Object);

            var request = new MiembroXClaseRequest
            {
                MiembroId = 999,
                ClaseId = 2,
                FechaInscripcion = DateTime.Today
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.AddMiembroXClase(request));
            Assert.Contains("Miembro", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AddMiembroXClase_ThrowsBadRequestException_WhenClaseInvalid()
        {
            var command = new Mock<MiembroXClaseCommand>(null);
            var query = new Mock<MiembroXClaseQuery>(null);
            var mapper = new Mock<MiembroXClaseMapper>(null, null);
            var miembroService = new Mock<MiembroService>(null, null, null, null, null, null,null);
            var claseService = new Mock<ClaseService>(null, null, null, null, null);

            // Configurar el mock de MiembroService para que retorne un Miembro válido
            miembroService.Setup(s => s.GetMiembroById(It.IsAny<int>())).ReturnsAsync(new MiembroResponse
            {
                Id = 1,
                TipoDeMiembroId = 1,
                TipoDeMiembro = new TipoDeMiembroResponse { Descripcion = "Regular", PorcentajeDescuento = 0 },
                Nombre = "Ana",
                Direccion = "Calle Falsa 123",
                Telefono = "123456789",
                FechaNacimiento = DateTime.Today.AddYears(-30),
                Email = "ana@example.com",
                Foto = "foto.jpg",
                Eliminado = false,
                Entrenador = null
            });

            // Configurar el mock de ClaseService para que retorne null, simulando un ID de clase inválido
            claseService.Setup(s => s.GetClaseById(It.IsAny<int>())).ReturnsAsync((ClaseResponse?)null);

            var service = new MiembroXClaseService(command.Object, query.Object, mapper.Object, miembroService.Object, claseService.Object);

            var request = new MiembroXClaseRequest
            {
                MiembroId = 1,
                ClaseId = 999,  // Usamos un ID de clase que no existe
                FechaInscripcion = DateTime.Today
            };

            // La expectativa es que se lance una BadRequestException
            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.AddMiembroXClase(request));

            // Verificamos que el mensaje de la excepción contiene "Clase"
            Assert.Contains("Clase", ex.Message, StringComparison.OrdinalIgnoreCase);
        }
        [Fact]
        public async Task AddMiembroXClase_ThrowsBadRequestException_WhenFechaInscripcionInFuture()
        {
            var command = new Mock<MiembroXClaseCommand>(null);
            var query = new Mock<MiembroXClaseQuery>(null);
            var mapper = new Mock<MiembroXClaseMapper>(null, null);
            var miembroService = new Mock<MiembroService>(null, null, null, null, null, null, null);
            var claseService = new Mock<ClaseService>(null, null, null, null, null);

            miembroService.Setup(s => s.GetMiembroById(It.IsAny<int>())).ReturnsAsync(new MiembroResponse
            {
                Id = 1,
                TipoDeMiembroId = 1,
                TipoDeMiembro = new TipoDeMiembroResponse { Descripcion = "Regular", PorcentajeDescuento = 0 },
                Nombre = "Ana",
                Direccion = "Calle Falsa 123",
                Telefono = "123456789",
                FechaNacimiento = DateTime.Today.AddYears(-30),
                Email = "ana@example.com",
                Foto = "foto.jpg",
                Eliminado = false,
                Entrenador = null
            });
            claseService.Setup(s => s.GetClaseById(It.IsAny<int>())).ReturnsAsync(new ClaseResponse
            {
                ClaseId = 2,
                Actividad = new ActividadResponse { Nombre = "Actividad", Descripcion = "Desc" },
                Entrenador = new EntrenadorResponse
                {
                    Id = 1,
                    Nombre = "Entr",
                    DNI = 11111111,
                    FechaNacimiento = DateTime.Today.AddYears(-40),
                    Telefono = "987654321",
                    Direccion = "Direccion Entr",
                    Email = "entr@example.com",
                    Foto = "fotoEntr.jpg",
                    Eliminado = false,
                    Certificacion = "Cert",
                    Activo = true
                },
                Fecha = DateTime.Today,
                HoraInicio = new TimeOnly(9, 0),
                HoraFin = new TimeOnly(10, 0),
                Cupo = 10
            });

            var service = new MiembroXClaseService(command.Object, query.Object, mapper.Object, miembroService.Object, claseService.Object);

            var request = new MiembroXClaseRequest
            {
                MiembroId = 1,
                ClaseId = 2,
                FechaInscripcion = DateTime.Today.AddDays(10) // futuro
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.AddMiembroXClase(request));
            Assert.Contains("Fecha de inscripcion", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task UpdateMiembroXClase_ReturnsResponse_WhenRequestIsValid()
        {
            var command = new Mock<MiembroXClaseCommand>(null);
            var query = new Mock<MiembroXClaseQuery>(null);
            var mapper = new Mock<MiembroXClaseMapper>(null, null);
            var miembroService = new Mock<MiembroService>(null, null, null, null, null, null, null);
            var claseService = new Mock<ClaseService>(null, null, null, null, null);

            var fechaOld = DateTime.Today.AddDays(-10);
            var fechaNew = DateTime.Today.AddDays(-1);

            var existing = new MiembroXClase
            {
                MiembroXClaseId = 1,
                MiembroId = 1,
                ClaseId = 2,
                FechaInscripcion = fechaOld,
                Miembro = new Miembro
                {
                    Id = 1,
                    TipoDeMiembroId = 1,
                    DNI = 12345678,
                    Nombre = "Ana",
                    Direccion = "Calle Falsa 123",
                    Telefono = "123456789",
                    FechaNacimiento = DateTime.Today.AddYears(-30),
                    Email = "ana@example.com",
                    Foto = "foto.jpg",
                    Eliminado = false
                },
                Clase = new Clase
                {
                    ClaseId = 2,
                    ActividadId = 1,
                    EntrenadorId = 1,
                    Fecha = DateTime.Today,
                    HoraInicio = new TimeOnly(9, 0),
                    HoraFin = new TimeOnly(10, 0),
                    Cupo = 5
                }
            };

            var updated = new MiembroXClase
            {
                MiembroXClaseId = 1,
                MiembroId = 1,
                ClaseId = 2,
                FechaInscripcion = fechaNew,
                Miembro = existing.Miembro,
                Clase = existing.Clase
            };

            // Sequence: Exists -> read for modification -> final read after update
            query.SetupSequence(q => q.GetMiembroXClaseById(1))
                .ReturnsAsync(existing)
                .ReturnsAsync(existing)
                .ReturnsAsync(updated);

            command.Setup(c => c.UpdateMiembroXClase(It.IsAny<MiembroXClase>())).ReturnsAsync(updated);
            mapper.Setup(m => m.GetMiembroXClaseResponse(updated)).ReturnsAsync(new MiembroXClaseResponse
            {
                MiembroXClaseId = 1,
                FechaInscripcion = fechaNew,
                Miembro = new MiembroResponse
                {
                    Id = 1,
                    TipoDeMiembroId = 1,
                    TipoDeMiembro = new TipoDeMiembroResponse { Descripcion = "Regular", PorcentajeDescuento = 0 },
                    Nombre = "Ana",
                    Direccion = "Calle Falsa 123",
                    Telefono = "123456789",
                    FechaNacimiento = DateTime.Today.AddYears(-30),
                    Email = "ana@example.com",
                    Foto = "foto.jpg",
                    Eliminado = false,
                    Entrenador = null
                },
                Clase = new ClaseResponse
                {
                    ClaseId = 2,
                    Actividad = new ActividadResponse { Nombre = "Actividad", Descripcion = "Desc" },
                    Entrenador = new EntrenadorResponse
                    {
                        Id = 1,
                        Nombre = "Entr",
                        DNI = 11111111,
                        FechaNacimiento = DateTime.Today.AddYears(-40),
                        Telefono = "987654321",
                        Direccion = "Direccion Entr",
                        Email = "entr@example.com",
                        Foto = "fotoEntr.jpg",
                        Eliminado = false,
                        Certificacion = "Cert",
                        Activo = true
                    },
                    Fecha = DateTime.Today,
                    HoraInicio = new TimeOnly(9, 0),
                    HoraFin = new TimeOnly(10, 0),
                    Cupo = 5
                }
            });

            miembroService.Setup(s => s.GetMiembroById(It.IsAny<int>())).ReturnsAsync(new MiembroResponse
            {
                Id = 1,
                TipoDeMiembroId = 1,
                TipoDeMiembro = new TipoDeMiembroResponse { Descripcion = "Regular", PorcentajeDescuento = 0 },
                Nombre = "Ana",
                Direccion = "Calle Falsa 123",
                Telefono = "123456789",
                FechaNacimiento = DateTime.Today.AddYears(-30),
                Email = "ana@example.com",
                Foto = "foto.jpg",
                Eliminado = false,
                Entrenador = null
            });
            claseService.Setup(s => s.GetClaseById(It.IsAny<int>())).ReturnsAsync(new ClaseResponse
            {
                ClaseId = 2,
                Actividad = new ActividadResponse { Nombre = "Actividad", Descripcion = "Desc" },
                Entrenador = new EntrenadorResponse
                {
                    Id = 1,
                    Nombre = "Entr",
                    DNI = 11111111,
                    FechaNacimiento = DateTime.Today.AddYears(-40),
                    Telefono = "987654321",
                    Direccion = "Direccion Entr",
                    Email = "entr@example.com",
                    Foto = "fotoEntr.jpg",
                    Eliminado = false,
                    Certificacion = "Cert",
                    Activo = true
                },
                Fecha = DateTime.Today,
                HoraInicio = new TimeOnly(9, 0),
                HoraFin = new TimeOnly(10, 0),
                Cupo = 5
            });

            var service = new MiembroXClaseService(command.Object, query.Object, mapper.Object, miembroService.Object, claseService.Object);

            var request = new MiembroXClaseRequest
            {
                MiembroId = 1,
                ClaseId = 2,
                FechaInscripcion = fechaNew
            };

            var result = await service.UpdateMiembroXClase(1, request);

            Assert.NotNull(result);
            Assert.Equal(fechaNew, result.FechaInscripcion);
        }

        [Fact]
        public async Task UpdateMiembroXClase_ThrowsNotFoundException_WhenIdIsInvalid()
        {
            var command = new Mock<MiembroXClaseCommand>(null);
            var query = new Mock<MiembroXClaseQuery>(null);
            var mapper = new Mock<MiembroXClaseMapper>(null, null);
            var miembroService = new Mock<MiembroService>(null, null, null, null, null, null, null);
            var claseService = new Mock<ClaseService>(null, null, null, null, null);

            query.Setup(q => q.GetMiembroXClaseById(It.IsAny<int>())).ReturnsAsync((MiembroXClase?)null);

            var service = new MiembroXClaseService(command.Object, query.Object, mapper.Object, miembroService.Object, claseService.Object);

            var request = new MiembroXClaseRequest
            {
                MiembroId = 1,
                ClaseId = 2,
                FechaInscripcion = DateTime.Today
            };

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateMiembroXClase(999, request));
        }

        [Fact]
        public async Task UpdateMiembroXClase_ThrowsBadRequestException_WhenMiembroInvalid()
        {
            var command = new Mock<MiembroXClaseCommand>(null);
            var query = new Mock<MiembroXClaseQuery>(null);
            var mapper = new Mock<MiembroXClaseMapper>(null, null);
            var miembroService = new Mock<MiembroService>(null, null, null, null, null, null, null);
            var claseService = new Mock<ClaseService>(null, null, null, null, null);

            var existing = new MiembroXClase
            {
                MiembroXClaseId = 1,
                MiembroId = 1,
                ClaseId = 2,
                FechaInscripcion = DateTime.Today.AddDays(-5),
                Miembro = new Miembro
                {
                    Id = 1,
                    TipoDeMiembroId = 1,
                    DNI = 12345678,
                    Nombre = "Ana",
                    Direccion = "Calle Falsa 123",
                    Telefono = "123456789",
                    FechaNacimiento = DateTime.Today.AddYears(-30),
                    Email = "ana@example.com",
                    Foto = "foto.jpg",
                    Eliminado = false
                },
                Clase = new Clase
                {
                    ClaseId = 2,
                    ActividadId = 1,
                    EntrenadorId = 1,
                    Fecha = DateTime.Today,
                    HoraInicio = new TimeOnly(9, 0),
                    HoraFin = new TimeOnly(10, 0),
                    Cupo = 5
                }
            };

            query.Setup(q => q.GetMiembroXClaseById(1)).ReturnsAsync(existing);
            miembroService.Setup(s => s.GetMiembroById(It.IsAny<int>())).ReturnsAsync((MiembroResponse?)null);
            claseService.Setup(s => s.GetClaseById(It.IsAny<int>())).ReturnsAsync(new ClaseResponse
            {
                ClaseId = 2,
                Actividad = new ActividadResponse { Nombre = "Actividad", Descripcion = "Desc" },
                Entrenador = new EntrenadorResponse
                {
                    Id = 1,
                    Nombre = "Entr",
                    DNI = 11111111,
                    FechaNacimiento = DateTime.Today.AddYears(-40),
                    Telefono = "987654321",
                    Direccion = "Direccion Entr",
                    Email = "entr@example.com",
                    Foto = "fotoEntr.jpg",
                    Eliminado = false,
                    Certificacion = "Cert",
                    Activo = true
                },
                Fecha = DateTime.Today,
                HoraInicio = new TimeOnly(9, 0),
                HoraFin = new TimeOnly(10, 0),
                Cupo = 5
            });

            var service = new MiembroXClaseService(command.Object, query.Object, mapper.Object, miembroService.Object, claseService.Object);

            var request = new MiembroXClaseRequest
            {
                MiembroId = 999,
                ClaseId = 2,
                FechaInscripcion = DateTime.Today
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.UpdateMiembroXClase(1, request));
            Assert.Contains("Miembro", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task UpdateMiembroXClase_ThrowsBadRequestException_WhenClaseInvalid()
        {
            var command = new Mock<MiembroXClaseCommand>(null);
            var query = new Mock<MiembroXClaseQuery>(null);
            var mapper = new Mock<MiembroXClaseMapper>(null, null);
            var miembroService = new Mock<MiembroService>(null, null, null, null, null, null, null);
            var claseService = new Mock<ClaseService>(null, null, null, null, null);

            var existing = new MiembroXClase
            {
                MiembroXClaseId = 1,
                MiembroId = 1,
                ClaseId = 2,
                FechaInscripcion = DateTime.Today.AddDays(-5),
                Miembro = new Miembro
                {
                    Id = 1,
                    TipoDeMiembroId = 1,
                    DNI = 12345678,
                    Nombre = "Ana",
                    Direccion = "Calle Falsa 123",
                    Telefono = "123456789",
                    FechaNacimiento = DateTime.Today.AddYears(-30),
                    Email = "ana@example.com",
                    Foto = "foto.jpg",
                    Eliminado = false
                },
                Clase = new Clase
                {
                    ClaseId = 2,
                    ActividadId = 1,
                    EntrenadorId = 1,
                    Fecha = DateTime.Today,
                    HoraInicio = new TimeOnly(9, 0),
                    HoraFin = new TimeOnly(10, 0),
                    Cupo = 5
                }
            };

            query.Setup(q => q.GetMiembroXClaseById(1)).ReturnsAsync(existing);
            miembroService.Setup(s => s.GetMiembroById(It.IsAny<int>())).ReturnsAsync(new MiembroResponse
            {
                Id = 1,
                TipoDeMiembroId = 1,
                TipoDeMiembro = new TipoDeMiembroResponse { Descripcion = "Regular", PorcentajeDescuento = 0 },
                Nombre = "Ana",
                Direccion = "Calle Falsa 123",
                Telefono = "123456789",
                FechaNacimiento = DateTime.Today.AddYears(-30),
                Email = "ana@example.com",
                Foto = "foto.jpg",
                Eliminado = false,
                Entrenador = null
            });
            claseService.Setup(s => s.GetClaseById(It.IsAny<int>())).ReturnsAsync((ClaseResponse?)null);

            var service = new MiembroXClaseService(command.Object, query.Object, mapper.Object, miembroService.Object, claseService.Object);

            var request = new MiembroXClaseRequest
            {
                MiembroId = 1,
                ClaseId = 999,
                FechaInscripcion = DateTime.Today
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.UpdateMiembroXClase(1, request));
            Assert.Contains("Clase", ex.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}