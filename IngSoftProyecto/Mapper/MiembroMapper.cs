using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Mapper
{
    public class MiembroMapper
    {
        private readonly TipoDeMiembroMapper _tipoDeMiembroMapper = new TipoDeMiembroMapper();
        public async Task<List<MiembroResponse>> GetAllMiembrosResponse(List<Miembro> miembros)
        {
            List<MiembroResponse> response = new List<MiembroResponse>();
            foreach (var miembro in miembros)
            {
                response.Add(await GetMiembroResponse(miembro));
            }
            return response;
        }

        public async Task<MiembroResponse> GetMiembroResponse(Miembro miembro)
        {
            MiembroResponse response = new MiembroResponse
            {
                TipoDeMiembroId = miembro.TipoDeMiembroId,
                EntrenadorId = miembro.EntrenadorId,
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
