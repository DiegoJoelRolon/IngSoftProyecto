using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Models;
namespace IngSoftProyecto.Mapper
{
    public class EntrenadorMapper
    {
        virtual public Task<EntrenadorResponse>GetEntrenadorResponse(Entrenador entrenador)
        {
            EntrenadorResponse response = new EntrenadorResponse
            {
                Id = entrenador.Id,
                Nombre = entrenador.Nombre,
                DNI= entrenador.DNI,
                FechaNacimiento = entrenador.FechaNacimiento,
                Telefono = entrenador.Telefono,
                Direccion = entrenador.Direccion,
                Email = entrenador.Email,
                Foto = entrenador.Foto,
                Eliminado = entrenador.Eliminado,
                Certificacion = entrenador.Certificacion,
                Activo = entrenador.Activo


            };
            return Task.FromResult(response);
        }
        virtual public Task<List<EntrenadorResponse>>GetAllEntrenadoresResponse(List<Entrenador> entrenadores)
        {
            List<EntrenadorResponse> entrenadoresResponse = new List<EntrenadorResponse>();
            foreach (var entrenador in entrenadores)
            {
                EntrenadorResponse response = new EntrenadorResponse
                {
                    Id = entrenador.Id,
                    Nombre = entrenador.Nombre,
                    DNI= entrenador.DNI,
                    FechaNacimiento = entrenador.FechaNacimiento,
                    Telefono = entrenador.Telefono,
                    Direccion = entrenador.Direccion,
                    Email = entrenador.Email,
                    Foto = entrenador.Foto,
                    Eliminado = entrenador.Eliminado,
                    Certificacion = entrenador.Certificacion,
                    Activo = entrenador.Activo
                };
                entrenadoresResponse.Add(response);
            }
            return Task.FromResult(entrenadoresResponse);
        }
    }
}
