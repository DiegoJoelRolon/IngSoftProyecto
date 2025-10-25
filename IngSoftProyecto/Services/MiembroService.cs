using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Services
{
    public class MiembroService
    {
        private readonly MiembroQuery _miembroQuery;
        private readonly MiembroCommand _miembroCommand;
        private readonly TipoDeMiembroQuery _tipoDeMiembroQuery;
        private readonly MiembroMapper _miembroMapper;

        public MiembroService(MiembroQuery miembroQuery, MiembroCommand miembroCommand, MiembroMapper miembroMapper, TipoDeMiembroQuery tipoDeMiembroQuery, TipoDeMiembroCommand tipoDeMiembroCommand, TipoDeMiembroMapper tipoDeMiembroMapper)
        {
            _miembroQuery = miembroQuery;
            _miembroCommand = miembroCommand;
            _tipoDeMiembroQuery = tipoDeMiembroQuery;
            _miembroMapper = miembroMapper;
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
            await CheckMiembroRequest(request);

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
        public async Task<bool> CheckMiembroRequest(MiembroRequest request)
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
            return true;


        }
    }
}
