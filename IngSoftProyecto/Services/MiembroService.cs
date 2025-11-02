using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace IngSoftProyecto.Services
{
    public class MiembroService
    {
        private readonly MiembroQuery _miembroQuery;
        private readonly MiembroCommand _miembroCommand;
        private readonly TipoDeMiembroQuery _tipoDeMiembroQuery;
        private readonly MiembroMapper _miembroMapper;
        private readonly EntrenadorService _entrenadorService;

        public MiembroService(MiembroQuery miembroQuery, MiembroCommand miembroCommand, MiembroMapper miembroMapper, TipoDeMiembroQuery tipoDeMiembroQuery, TipoDeMiembroCommand tipoDeMiembroCommand, TipoDeMiembroMapper tipoDeMiembroMapper,EntrenadorService entrenadorService)
        {
            _miembroQuery = miembroQuery;
            _miembroCommand = miembroCommand;
            _tipoDeMiembroQuery = tipoDeMiembroQuery;
            _miembroMapper = miembroMapper;
            _entrenadorService = entrenadorService;
        }
        public virtual async Task<List<MiembroResponse>> GetAllMiembros()
        {
            return await _miembroMapper.GetAllMiembrosResponse((await _miembroQuery.GetAllMiembros()));
        }
        public virtual async Task<MiembroResponse?> GetMiembroById(int id)
        {
            await MiembroExists(id);

            var miembro = await _miembroQuery.GetMiembroById(id);
            if (miembro == null)
                return null;
            return await _miembroMapper.GetMiembroResponse(miembro);
        }
        public virtual async Task<MiembroResponse> AddMiembro(MiembroRequest request)
        {
            await CheckMiembroRequest(request);

            var miembro = new Miembro
            {
                DNI= request.DNI,
                TipoDeMiembroId = request.TipoDeMiembroId,
                EntrenadorId = request.EntrenadorId,
                Nombre = request.Nombre,
                Direccion = request.Direccion,
                Telefono = request.Telefono,
                FechaNacimiento = request.FechaNacimiento,
                Email = request.Email,
                Foto = request.Foto,
                Eliminado = false
            };
            var result = await _miembroCommand.AddMiembro(miembro);
            return await _miembroMapper.GetMiembroResponse(await _miembroQuery.GetMiembroById(result.Id));
        }
        public virtual async Task<MiembroResponse> UpdateMiembro(int id, MiembroRequest request)
        {
            await MiembroExists(id);
            await CheckMiembroRequest(request,id);

            var miembro = await _miembroQuery.GetMiembroById(id);
            miembro.DNI= request.DNI;
            miembro.TipoDeMiembroId = request.TipoDeMiembroId;
            miembro.EntrenadorId = request.EntrenadorId;
            miembro.Nombre = request.Nombre;
            miembro.Direccion = request.Direccion;
            miembro.Telefono = request.Telefono;
            miembro.FechaNacimiento = request.FechaNacimiento;
            miembro.Email = request.Email;
            miembro.Foto = request.Foto;
            var result = await _miembroCommand.UpdateMiembro(miembro);
            return await _miembroMapper.GetMiembroResponse(await _miembroQuery.GetMiembroById(result.Id));
        }
        public virtual async Task<MiembroResponse> DeleteMiembro(int id)
        {
            await MiembroExists(id);

            var miembro = await _miembroQuery.GetMiembroById(id);
            miembro.Eliminado = true;
            var result = await _miembroCommand.UpdateMiembro(miembro);
            return await _miembroMapper.GetMiembroResponse(await _miembroQuery.GetMiembroById(result.Id));
        }
        public virtual async Task<MiembroResponse> RestoreMiembro(int id)
        {
            await MiembroExists(id);
            var miembro = await _miembroQuery.GetMiembroById(id);

            miembro.Eliminado = false;
            var result = await _miembroCommand.UpdateMiembro(miembro);
            return await _miembroMapper.GetMiembroResponse(await _miembroQuery.GetMiembroById(result.Id));
        }


        //validations
        private async Task<bool> MiembroExists(int id)
        {
            if (id <= 0 || await _miembroQuery.GetMiembroById(id) == null)
                throw new NotFoundException("Id de miembro invalido");
            return true;
        }
        public async Task<bool> CheckMiembroRequest(MiembroRequest request, int? miembroId = null)
        {
            if (await _tipoDeMiembroQuery.GetTipoDeMiembroById(request.TipoDeMiembroId) == null)
                throw new NotFoundException("Tipo de miembro no encontrado");
            if (string.IsNullOrEmpty(request.Nombre) || string.IsNullOrWhiteSpace(request.Nombre))
                throw new BadRequestException("Nombre invalido");
            if (string.IsNullOrEmpty(request.Direccion) || string.IsNullOrWhiteSpace(request.Direccion))
                throw new BadRequestException("Direccion invalida");
            if (string.IsNullOrEmpty(request.Telefono) || string.IsNullOrWhiteSpace(request.Telefono))
                throw new BadRequestException("Telefono invalido");
            if (request.FechaNacimiento > DateTime.Now)
                throw new BadRequestException("FechaNacimiento invalida");
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@") || !request.Email.ToLower().Contains(".com"))
                throw new BadRequestException("Email invalido");
            if (string.IsNullOrEmpty(request.Foto) || string.IsNullOrWhiteSpace(request.Foto))
                throw new BadRequestException("Foto invalida");

            // Validar DNI
            if (string.IsNullOrEmpty(request.DNI.ToString()) || string.IsNullOrWhiteSpace(request.DNI.ToString()))
                throw new BadRequestException("DNI es obligatorio.");
            // 1. NUEVA VALIDACIÓN: Unicidad del DNI
            if (await _miembroQuery.MiembroExistsByDNI(request.DNI, miembroId))
                throw new BadRequestException("El DNI ya se encuentra registrado para otro miembro.");

            // Validar Email (Tu validación de formato ya existe, solo agregamos la unicidad)
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@") || !request.Email.ToLower().Contains("."))
                throw new BadRequestException("Email inválido o formato incorrecto.");
            // 2. NUEVA VALIDACIÓN: Unicidad del Email
            if (await _miembroQuery.MiembroExistsByEmail(request.Email, miembroId))
                throw new BadRequestException("El Email ya se encuentra registrado para otro miembro.");

            if (request.EntrenadorId.HasValue && request.EntrenadorId.Value > 0)
            {
                if (await _entrenadorService.GetEntrenadorById(request.EntrenadorId.Value) == null)
                {
                    throw new NotFoundException("El EntrenadorId especificado no existe.");
                }
            }

            if (!Regex.IsMatch(request.DNI.ToString(), @"^\d+$"))
                throw new BadRequestException("DNI debe contener solo números.");

            // ...

            // 5. NUEVA VALIDACIÓN: Formato de Teléfono (Ejemplo: solo números y un largo mínimo/máximo)
            if (!Regex.IsMatch(request.Telefono, @"^\d{8,15}$")) // Ajustar la regex a tu formato local (ej: 8 a 15 dígitos)
                throw new BadRequestException("Teléfono inválido. Solo se permiten entre 8 y 15 dígitos.");
            return true;


        }
    }
}
