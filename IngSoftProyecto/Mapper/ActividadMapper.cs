using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Mapper
{
    public class ActividadMapper
    {
        virtual public Task<List<ActividadResponse>>GetAllActividadesResponse(List<Actividad>actividades)
        {
            List<ActividadResponse> response = new List<ActividadResponse>();
            foreach (var actividad in actividades)
            {
                response.Add(new ActividadResponse
                {
                    Nombre = actividad.Nombre,
                    Descripcion = actividad.Descripcion
                });

            }
            return Task.FromResult(response);

        }
        virtual public Task<ActividadResponse> GetActividadResponse(Actividad actividad)
        {
            ActividadResponse response = new ActividadResponse
            {
                Nombre = actividad.Nombre,
                Descripcion = actividad.Descripcion
            };
            return Task.FromResult(response);
        }
    }
}
