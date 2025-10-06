using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
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
        private readonly MiembroMapper _miembroMapper = new MiembroMapper();

        public MiembroService(MiembroQuery miembroQuery, MiembroCommand miembroCommand, MiembroMapper miembroMapper, TipoDeMiembroQuery tipoDeMiembroQuery, TipoDeMiembroCommand tipoDeMiembroCommand, TipoDeMiembroMapper tipoDeMiembroMapper)
        {
            _miembroQuery = miembroQuery;
            _miembroMapper = miembroMapper;
            _miembroCommand = miembroCommand;
            
        }
        public async Task<List<MiembroResponse>> GetAllMiembros()
        {
            return await _miembroMapper.GetAllMiembrosResponse((await _miembroQuery.GetAllMiembros()));
        }
        public async Task <MiembroResponse?> GetMiembroById(int id)
        {
            var miembro = await _miembroQuery.GetMiembroById(id);
            if (miembro == null)
                return null;
            return await _miembroMapper.GetMiembroResponse(miembro);
        }
        public async Task<MiembroResponse>AddMiembro(MiembroRequest request)
        {
            var miembro = new Miembro
            {
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
            return await _miembroMapper.GetMiembroResponse(await _miembroQuery.GetMiembroById(result.MiembroId));
        }


    }
}
