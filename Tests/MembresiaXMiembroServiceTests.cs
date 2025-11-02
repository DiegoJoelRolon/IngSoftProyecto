using System;
using System.Collections.Generic;
using System.Reflection;
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
    public class MembresiaXMiembroServiceTests
    {
        // Helper para inyectar dependencias privadas que no se pasan por constructor
        private static void SetPrivateField(object target, string fieldName, object value)
        {
            var fi = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            fi.SetValue(target, value);
        }

        private static Miembro CreateTestMiembro(int id = 1, string nombre = "Juan")
        {
            return new Miembro
            {
                Id = id,
                TipoDeMiembroId = 1,
                DNI = 12345678,
                FechaNacimiento = DateTime.Today.AddYears(-30),
                Telefono = "000-0000",
                Direccion = "Calle Falsa 123",
                Email = "juan@example.com",
                Foto = "foto.jpg",
                Nombre = nombre,
                Eliminado = false
            };
        }

        private static MiembroResponse CreateTestMiembroResponse(int id = 1, string nombre = "Juan")
        {
            return new MiembroResponse
            {
                Id = id,
                TipoDeMiembroId = 1,
                Entrenador = null,
                Nombre = nombre,
                Direccion = "Calle Falsa 123",
                Telefono = "000-0000",
                FechaNacimiento = DateTime.Today.AddYears(-30),
                TipoDeMiembro = new TipoDeMiembroResponse { Descripcion = "Socio", PorcentajeDescuento = 0 },
                Email = "juan@example.com",
                Foto = "foto.jpg",
                Eliminado = false
            };
        }

        private static Pago CreateTestPago(int id = 1, decimal monto = 100m)
        {
            return new Pago
            {
                PagoId = id,
                Monto = monto,
                FechaPago = DateTime.Now,
                MetodoPago = "Efectivo",
                DescuentoAplicado = 0,
                MembresiasXMiembro = new List<MembresiaXMiembro>()
            };
        }

        private static PagoResponse CreateTestPagoResponse(int id = 1, decimal monto = 100m)
        {
            return new PagoResponse
            {
                PagoId = id,
                Monto = monto,
                FechaPago = DateTime.Now,
                MetodoPago = "Efectivo",
                DescuentoAplicado = 0
            };
        }

        [Fact]
        public async Task GetAllMembresiasXMiembros_ReturnsList()
        {
            var command = new Mock<MembresiaXMiembroCommand>(null);
            var query = new Mock<MembresiaXMiembroQuery>(null);
            var mapper = new Mock<MembresiaXMiembroMapper>(null, null, null, null);

            query.Setup(q => q.GetAllMembresiasXMiembros()).ReturnsAsync(new List<MembresiaXMiembro>());
            mapper.Setup(m => m.GetMembresiaXMiembroResponseList(It.IsAny<List<MembresiaXMiembro>>()))
                .ReturnsAsync(new List<MembresiaXMiembroResponse>());

            var service = new MembresiaXMiembroService(command.Object, query.Object, mapper.Object);

            var result = await service.GetAllMembresiasXMiembros();

            Assert.NotNull(result);
            Assert.IsType<List<MembresiaXMiembroResponse>>(result);
        }

        [Fact]
        public async Task GetMembresiaXMiembroById_ReturnsResponse_WhenIdIsValid()
        {
            var command = new Mock<MembresiaXMiembroCommand>(null);
            var query = new Mock<MembresiaXMiembroQuery>(null);
            var mapper = new Mock<MembresiaXMiembroMapper>(null, null, null, null);

            var fechaInicio = DateTime.Today;
            var fechaFin = fechaInicio.AddDays(30);

            var entity = new MembresiaXMiembro
            {
                MembresiaXMiembroId = 1,
                MiembroId = 1,
                MembresiaId = 1,
                EstadoMembresiaId = 1,
                PagoId = 1,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Miembro = CreateTestMiembro(1, "Juan"),
                Membresia = new Membresia { MembresiaId = 1, DuracionEnDias = 30, CostoBase = 100m },
                Pago = CreateTestPago(1, 100m),
                EstadoMembresia = new EstadoMembresia { EstadoMembresiaId = 1, Descripcion = "Activa" },
                Asistencias = new List<Asistencia>()
            };

            var response = new MembresiaXMiembroResponse
            {
                MembresiaXMiembroId = 1,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Miembro = CreateTestMiembroResponse(1, "Juan"),
                Membresia = new MembresiaResponse { DuracionEnDias = 30, CostoBase = 100m },
                Pago = CreateTestPagoResponse(1, 100m),
                EstadoMembresia = new GenericResponse { Id = 1, Descripcion = "Activa" }
            };

            query.Setup(q => q.GetMembresiaXMiembroById(1)).ReturnsAsync(entity);
            mapper.Setup(m => m.GetMembresiaXMiembroResponse(entity)).ReturnsAsync(response);

            var service = new MembresiaXMiembroService(command.Object, query.Object, mapper.Object);

            var result = await service.GetMembresiaXMiembroById(1);

            Assert.NotNull(result);
            Assert.IsType<MembresiaXMiembroResponse>(result);
            Assert.Equal(fechaInicio, result.FechaInicio);
            Assert.Equal(fechaFin, result.FechaFin);
        }

        [Fact]
        public async Task GetMembresiaXMiembroById_ThrowsNotFoundException_WhenIdInvalid()
        {
            var command = new Mock<MembresiaXMiembroCommand>(null);
            var query = new Mock<MembresiaXMiembroQuery>(null);
            var mapper = new Mock<MembresiaXMiembroMapper>(null, null, null, null);

            query.Setup(q => q.GetMembresiaXMiembroById(It.IsAny<int>())).ReturnsAsync((MembresiaXMiembro?)null);

            var service = new MembresiaXMiembroService(command.Object, query.Object, mapper.Object);

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetMembresiaXMiembroById(0));
            Assert.Equal("Id de MembresiaXMiembro invalido", ex.Message);
        }

        [Fact]
        public async Task AddMembresiaXMiembro_ReturnsResponse_WhenRequestIsValid()
        {
            var command = new Mock<MembresiaXMiembroCommand>(null);
            var query = new Mock<MembresiaXMiembroQuery>(null);
            var mapper = new Mock<MembresiaXMiembroMapper>(null, null, null, null);

            // Dependencias extras que el servicio utiliza en la validación
            var mockMiembroService = new Mock<MiembroService>(null, null, null, null, null, null,null);
            var mockMembresiasService = new Mock<MembresiasService>(null, null, null, null,null);
            var mockEstadoQuery = new Mock<EstadoMembresiaQuery>(null);
            var mockPagoService = new Mock<PagoService>(null, null, null);

            var fechaInicio = DateTime.Today;
            var fechaFin = fechaInicio.AddDays(30);

            // Preparar entidades y respuestas
            var added = new MembresiaXMiembro
            {
                MembresiaXMiembroId = 1,
                MiembroId = 1,
                MembresiaId = 1,
                EstadoMembresiaId = 1,
                PagoId = 1,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Miembro = CreateTestMiembro(),
                Membresia = new Membresia { MembresiaId = 1, DuracionEnDias = 30, CostoBase = 100m },
                Pago = CreateTestPago(1, 100m),
                EstadoMembresia = new EstadoMembresia { EstadoMembresiaId = 1, Descripcion = "Activa" },
                Asistencias = new List<Asistencia>()
            };
            var fetched = new MembresiaXMiembro
            {
                MembresiaXMiembroId = 1,
                MiembroId = 1,
                MembresiaId = 1,
                EstadoMembresiaId = 1,
                PagoId = 1,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Miembro = CreateTestMiembro(),
                Membresia = new Membresia { MembresiaId = 1, DuracionEnDias = 30, CostoBase = 100m },
                Pago = CreateTestPago(1, 100m),
                EstadoMembresia = new EstadoMembresia { EstadoMembresiaId = 1, Descripcion = "Activa" },
                Asistencias = new List<Asistencia>()
            };
            var response = new MembresiaXMiembroResponse
            {
                MembresiaXMiembroId = 1,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Miembro = CreateTestMiembroResponse(),
                Membresia = new MembresiaResponse { DuracionEnDias = 30, CostoBase = 100m },
                Pago = CreateTestPagoResponse(1, 100m),
                EstadoMembresia = new GenericResponse { Id = 1, Descripcion = "Activa" }
            };

            // Setups
            mockMiembroService.Setup(s => s.GetMiembroById(It.IsAny<int>())).ReturnsAsync(CreateTestMiembroResponse());
            mockMembresiasService.Setup(s => s.GetMembresiaById(It.IsAny<int>())).ReturnsAsync(new MembresiaResponse { DuracionEnDias = 30, CostoBase = 100m });
            mockEstadoQuery.Setup(q => q.GetEstadoMembresiaById(It.IsAny<int>())).ReturnsAsync(new EstadoMembresia { EstadoMembresiaId = 1, Descripcion = "Activa" });
            mockPagoService.Setup(p => p.GetPagoById(It.IsAny<int>())).ReturnsAsync(CreateTestPagoResponse(1, 100m));

            command.Setup(c => c.AddMembresiaXMiembro(It.IsAny<MembresiaXMiembro>())).ReturnsAsync(added);
            query.Setup(q => q.GetMembresiaXMiembroById(1)).ReturnsAsync(fetched);
            mapper.Setup(m => m.GetMembresiaXMiembroResponse(fetched)).ReturnsAsync(response);

            var service = new MembresiaXMiembroService(command.Object, query.Object, mapper.Object);

            // inyectar dependencias privadas
            SetPrivateField(service, "_miembroService", mockMiembroService.Object);
            SetPrivateField(service, "_membresiaService", mockMembresiasService.Object);
            SetPrivateField(service, "_estadoMembresiaQuery", mockEstadoQuery.Object);
            SetPrivateField(service, "_pagoService", mockPagoService.Object);

            var request = new MembresiaXMiembroRequest
            {
                MiembroId = 1,
                MembresiaId = 1,
                EstadoMembresiaId = 1,
                PagoId = 1,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            };

            var result = await service.AddMembresiaXMiembro(request);

            Assert.NotNull(result);
            Assert.IsType<MembresiaXMiembroResponse>(result);
            Assert.Equal(fechaInicio, result.FechaInicio);
            Assert.Equal(fechaFin, result.FechaFin);
        }

        [Fact]
        public async Task AddMembresiaXMiembro_ThrowsBadRequestException_WhenEstadoMembresiaInvalid()
        {
            // 1. Mocks de Command, Query y Mapper principales
            var command = new Mock<MembresiaXMiembroCommand>(null);
            var query = new Mock<MembresiaXMiembroQuery>(null);
            var mapper = new Mock<MembresiaXMiembroMapper>(null, null, null, null);

            // 2. Mocks de Servicios de dependencia (usados en CheckMembresiaXMiembroRequest)
            var mockMiembroService = new Mock<MiembroService>(null, null, null, null, null, null,null);
            var mockMembresiasService = new Mock<MembresiasService>(null, null, null, null,null);
            var mockEstadoQuery = new Mock<EstadoMembresiaQuery>(null);
            var mockPagoService = new Mock<PagoService>(null, null, null);

            // 3. Configuración de Mocks para que pasen las validaciones previas al error
            //    (Miembro, Membresia, y Pago son válidos)
            mockMiembroService.Setup(s => s.GetMiembroById(It.IsAny<int>())).ReturnsAsync(CreateTestMiembroResponse());
            mockMembresiasService.Setup(s => s.GetMembresiaById(It.IsAny<int>())).ReturnsAsync(new MembresiaResponse { DuracionEnDias = 30, CostoBase = 100m });
            // ESTADO INVÁLIDO (El caso de prueba) -> devuelve null
            mockEstadoQuery.Setup(q => q.GetEstadoMembresiaById(It.IsAny<int>())).ReturnsAsync((EstadoMembresia?)null);
            mockPagoService.Setup(p => p.GetPagoById(It.IsAny<int>())).ReturnsAsync(CreateTestPagoResponse(1, 100m));

            // 4. Instanciación del servicio y asignación de campos privados (dependencies)
            var service = new MembresiaXMiembroService(command.Object, query.Object, mapper.Object);
            SetPrivateField(service, "_miembroService", mockMiembroService.Object);

            // --- CORRECCIÓN CLAVE ---
            // Debe coincidir con el campo privado en el servicio: "_membresiaService"
            SetPrivateField(service, "_membresiaService", mockMembresiasService.Object);

            SetPrivateField(service, "_estadoMembresiaQuery", mockEstadoQuery.Object);
            SetPrivateField(service, "_pagoService", mockPagoService.Object);

            // 5. Creación del Request
            var request = new MembresiaXMiembroRequest
            {
                MiembroId = 1,
                MembresiaId = 1,
                EstadoMembresiaId = 999, // ID Invalido para forzar la excepción
                PagoId = 1,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(10)
            };

            // 6. Assertions
            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.AddMembresiaXMiembro(request));
            Assert.Contains("EstadoMembresia", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AddMembresiaXMiembro_ThrowsBadRequestException_WhenFechasInvalidas()
        {
            var command = new Mock<MembresiaXMiembroCommand>(null);
            var query = new Mock<MembresiaXMiembroQuery>(null);
            var mapper = new Mock<MembresiaXMiembroMapper>(null, null, null, null);

            var mockMiembroService = new Mock<MiembroService>(null, null, null, null, null, null,null);
            var mockMembresiasService = new Mock<MembresiasService>(null, null, null, null,null);
            var mockEstadoQuery = new Mock<EstadoMembresiaQuery>(null);
            var mockPagoService = new Mock<PagoService>(null, null, null);

            mockMiembroService.Setup(s => s.GetMiembroById(It.IsAny<int>())).ReturnsAsync(CreateTestMiembroResponse());
            mockMembresiasService.Setup(s => s.GetMembresiaById(It.IsAny<int>())).ReturnsAsync(new MembresiaResponse { DuracionEnDias = 30, CostoBase = 100m });
            mockEstadoQuery.Setup(q => q.GetEstadoMembresiaById(It.IsAny<int>())).ReturnsAsync(new EstadoMembresia { EstadoMembresiaId = 1, Descripcion = "Activa" });
            mockPagoService.Setup(p => p.GetPagoById(It.IsAny<int>())).ReturnsAsync(CreateTestPagoResponse(1, 100m));

            var service = new MembresiaXMiembroService(command.Object, query.Object, mapper.Object);
            SetPrivateField(service, "_miembroService", mockMiembroService.Object);
            SetPrivateField(service, "_membresiaService", mockMembresiasService.Object);
            SetPrivateField(service, "_estadoMembresiaQuery", mockEstadoQuery.Object);
            SetPrivateField(service, "_pagoService", mockPagoService.Object);

            var request = new MembresiaXMiembroRequest
            {
                MiembroId = 1,
                MembresiaId = 1,
                EstadoMembresiaId = 1,
                PagoId = 1,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today // igual -> inválido
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.AddMembresiaXMiembro(request));
            Assert.Contains("Fechas invalidas", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task UpdateMembresiaXMiembro_ReturnsResponse_WhenRequestIsValid()
        {
            var command = new Mock<MembresiaXMiembroCommand>(null);
            var query = new Mock<MembresiaXMiembroQuery>(null);
            var mapper = new Mock<MembresiaXMiembroMapper>(null, null, null, null);

            var mockMiembroService = new Mock<MiembroService>(null, null, null, null, null, null,null);
            var mockMembresiasService = new Mock<MembresiasService>(null, null, null, null,null);
            var mockEstadoQuery = new Mock<EstadoMembresiaQuery>(null);
            var mockPagoService = new Mock<PagoService>(null, null, null);

            var fechaInicio = DateTime.Today;
            var fechaFinOld = fechaInicio.AddDays(10);
            var fechaFinNew = fechaInicio.AddDays(30);

            var existing = new MembresiaXMiembro
            {
                MembresiaXMiembroId = 1,
                MiembroId = 1,
                MembresiaId = 1,
                EstadoMembresiaId = 1,
                PagoId = 1,
                FechaInicio = fechaInicio,
                FechaFin = fechaFinOld,
                Miembro = CreateTestMiembro(),
                Membresia = new Membresia { MembresiaId = 1, DuracionEnDias = 10, CostoBase = 50m },
                Pago = CreateTestPago(1, 50m),
                EstadoMembresia = new EstadoMembresia { EstadoMembresiaId = 1, Descripcion = "Activa" },
                Asistencias = new List<Asistencia>()
            };

            var updated = new MembresiaXMiembro
            {
                MembresiaXMiembroId = 1,
                MiembroId = 1,
                MembresiaId = 1,
                EstadoMembresiaId = 1,
                PagoId = 1,
                FechaInicio = fechaInicio,
                FechaFin = fechaFinNew,
                Miembro = existing.Miembro,
                Membresia = new Membresia { MembresiaId = 1, DuracionEnDias = 30, CostoBase = 100m },
                Pago = existing.Pago,
                EstadoMembresia = existing.EstadoMembresia,
                Asistencias = new List<Asistencia>()
            };

            mapper.Setup(m => m.GetMembresiaXMiembroResponse(updated)).ReturnsAsync(new MembresiaXMiembroResponse
            {
                MembresiaXMiembroId = 1,
                FechaInicio = fechaInicio,
                FechaFin = fechaFinNew,
                Miembro = CreateTestMiembroResponse(),
                Membresia = new MembresiaResponse { DuracionEnDias = 30, CostoBase = 100m },
                Pago = CreateTestPagoResponse(1, 50m),
                EstadoMembresia = new GenericResponse { Id = 1, Descripcion = "Activa" }
            });

            // Queried primero para Exists y luego para modificación -> usar Sequence
            query.SetupSequence(q => q.GetMembresiaXMiembroById(1))
                .ReturnsAsync(existing)   // MembresiaXMiembroExists
                .ReturnsAsync(existing);  // lectura para modificar (el comando devolverá updated y luego se vuelve a pedir por id)
            // Después del update el service pedirá de nuevo por id, así que configuramos que devuelva updated cuando sea consultado con el id del resultado
            // Simulamos que el comando devuelve 'updated' y que la siguiente consulta devuelve 'updated'
            command.Setup(c => c.UpdateMembresiaXMiembro(It.IsAny<MembresiaXMiembro>())).ReturnsAsync(updated);
            query.Setup(q => q.GetMembresiaXMiembroById(1)).ReturnsAsync(updated);

            mockMiembroService.Setup(s => s.GetMiembroById(It.IsAny<int>())).ReturnsAsync(CreateTestMiembroResponse());
            mockMembresiasService.Setup(s => s.GetMembresiaById(It.IsAny<int>())).ReturnsAsync(new MembresiaResponse { DuracionEnDias = 30, CostoBase = 100m });
            mockEstadoQuery.Setup(q => q.GetEstadoMembresiaById(It.IsAny<int>())).ReturnsAsync(new EstadoMembresia { EstadoMembresiaId = 1, Descripcion = "Activa" });
            mockPagoService.Setup(p => p.GetPagoById(It.IsAny<int>())).ReturnsAsync(CreateTestPagoResponse(1, 50m));

            var service = new MembresiaXMiembroService(command.Object, query.Object, mapper.Object);
            SetPrivateField(service, "_miembroService", mockMiembroService.Object);
            SetPrivateField(service, "_membresiaService", mockMembresiasService.Object);
            SetPrivateField(service, "_estadoMembresiaQuery", mockEstadoQuery.Object);
            SetPrivateField(service, "_pagoService", mockPagoService.Object);

            var request = new MembresiaXMiembroRequest
            {
                MiembroId = 1,
                MembresiaId = 1,
                EstadoMembresiaId = 1,
                PagoId = 1,
                FechaInicio = fechaInicio,
                FechaFin = fechaFinNew
            };

            var result = await service.UpdateMembresiaXMiembro(1, request);

            Assert.NotNull(result);
            Assert.Equal(fechaFinNew, result.FechaFin);
        }

        [Fact]
        public async Task UpdateMembresiaXMiembro_ThrowsNotFoundException_WhenIdIsInvalid()
        {
            var command = new Mock<MembresiaXMiembroCommand>(null);
            var query = new Mock<MembresiaXMiembroQuery>(null);
            var mapper = new Mock<MembresiaXMiembroMapper>(null, null, null, null);

            query.Setup(q => q.GetMembresiaXMiembroById(It.IsAny<int>())).ReturnsAsync((MembresiaXMiembro?)null);

            var service = new MembresiaXMiembroService(command.Object, query.Object, mapper.Object);

            var request = new MembresiaXMiembroRequest
            {
                MiembroId = 1,
                MembresiaId = 1,
                EstadoMembresiaId = 1,
                PagoId = 1,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(1)
            };

            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateMembresiaXMiembro(999, request));
        }

        [Fact]
        public async Task UpdateMembresiaXMiembro_ThrowsBadRequestException_WhenEstadoMembresiaInvalid()
        {
            var command = new Mock<MembresiaXMiembroCommand>(null);
            var query = new Mock<MembresiaXMiembroQuery>(null);
            var mapper = new Mock<MembresiaXMiembroMapper>(null, null, null, null);

            var existing = new MembresiaXMiembro
            {
                MembresiaXMiembroId = 1,
                MiembroId = 1,
                MembresiaId = 1,
                EstadoMembresiaId = 1,
                PagoId = 1,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(10),
                Miembro = CreateTestMiembro(),
                Membresia = new Membresia { MembresiaId = 1, DuracionEnDias = 10, CostoBase = 50m },
                Pago = CreateTestPago(1, 50m),
                EstadoMembresia = new EstadoMembresia { EstadoMembresiaId = 1, Descripcion = "Activa" },
                Asistencias = new List<Asistencia>()
            };

            query.Setup(q => q.GetMembresiaXMiembroById(1)).ReturnsAsync(existing);

            var mockMiembroService = new Mock<MiembroService>(null, null, null, null, null, null,null);
            var mockMembresiasService = new Mock<MembresiasService>(null, null, null, null,null);
            var mockEstadoQuery = new Mock<EstadoMembresiaQuery>(null);
            var mockPagoService = new Mock<PagoService>(null, null, null);

            mockMiembroService.Setup(s => s.GetMiembroById(It.IsAny<int>())).ReturnsAsync(CreateTestMiembroResponse());
            mockMembresiasService.Setup(s => s.GetMembresiaById(It.IsAny<int>())).ReturnsAsync(new MembresiaResponse { DuracionEnDias = 30, CostoBase = 100m });
            // Estado inválido
            mockEstadoQuery.Setup(q => q.GetEstadoMembresiaById(It.IsAny<int>())).ReturnsAsync((EstadoMembresia?)null);
            mockPagoService.Setup(p => p.GetPagoById(It.IsAny<int>())).ReturnsAsync(CreateTestPagoResponse(1, 50m));

            var service = new MembresiaXMiembroService(command.Object, query.Object, mapper.Object);
            SetPrivateField(service, "_miembroService", mockMiembroService.Object);
            SetPrivateField(service, "_membresiaService", mockMembresiasService.Object);
            SetPrivateField(service, "_estadoMembresiaQuery", mockEstadoQuery.Object);
            SetPrivateField(service, "_pagoService", mockPagoService.Object); // not used but keep consistent

            var request = new MembresiaXMiembroRequest
            {
                MiembroId = 1,
                MembresiaId = 1,
                EstadoMembresiaId = 999,
                PagoId = 1,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(5)
            };

            var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.UpdateMembresiaXMiembro(1, request));
            Assert.Contains("EstadoMembresia", ex.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}