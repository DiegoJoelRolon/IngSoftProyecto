using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Mapper
{
    public class MiembroMapper
    {
        private readonly EntrenadorMapper _entrenadorMapper;
        private readonly TipoDeMiembroMapper _tipoDeMiembroMapper;
        public MiembroMapper(EntrenadorMapper entrenadorMapper,TipoDeMiembroMapper tipoDeMiembroMapper)
        {
            _entrenadorMapper = entrenadorMapper;
            _tipoDeMiembroMapper = tipoDeMiembroMapper;
        }

        virtual public async Task<List<MiembroResponse>> GetAllMiembrosResponse(List<Miembro> miembros)
        {
            List<MiembroResponse> response = new List<MiembroResponse>();
            foreach (var miembro in miembros)
            {
                response.Add(await GetMiembroResponse(miembro));
            }
            return response;
        }

        virtual public async Task<MiembroResponse> GetMiembroResponse(Miembro miembro)
        {
            MiembroResponse response = new MiembroResponse
            {

                Id = miembro.Id,
                TipoDeMiembroId = miembro.TipoDeMiembroId,
                Entrenador= miembro.Entrenador != null
                     ? await _entrenadorMapper.GetEntrenadorResponse(miembro.Entrenador): null,
                Nombre = miembro.Nombre,
                Direccion = miembro.Direccion,
                Telefono = miembro.Telefono,
                FechaNacimiento = miembro.FechaNacimiento,
                TipoDeMiembro = await _tipoDeMiembroMapper.GetTipoDeMiembroResponse(miembro.TipoDeMiembro),
                Email = miembro.Email,
                Foto = miembro.Foto,
                Eliminado = miembro.Eliminado
            };
            return response;
        }
    }
}
