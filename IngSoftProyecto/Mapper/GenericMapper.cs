using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Mapper
{
    public class GenericMapper
    {
        virtual public Task<GenericResponse> GetGenericResponse(TipoDeMembresia tipoDeMembresia)
        {
            GenericResponse response = new GenericResponse
            {
                Descripcion = tipoDeMembresia.Descripcion
            };
            return Task.FromResult(response);
        }

        virtual public Task<List<GenericResponse>> GetAllGenericResponse(List<TipoDeMembresia> tiposDeMembresias)
        {
            List<GenericResponse> responses = new List<GenericResponse>();
            foreach (var item in tiposDeMembresias)
            {
                GenericResponse response = new GenericResponse
                {
                    Descripcion = item.Descripcion
                };
                responses.Add(response);
            }
            return Task.FromResult(responses);
        }
        virtual public Task<GenericResponse> GetGenericResponse(TipoDeAsistencia tipoDeAsistencia)
        {
            GenericResponse response = new GenericResponse
            {
                Descripcion = tipoDeAsistencia.Descripcion
            };
            return Task.FromResult(response);
        }

        virtual public Task<List<GenericResponse>> GetAllGenericResponse(List<TipoDeAsistencia> tiposDeAsistencias)
        {
            List<GenericResponse> responses = new List<GenericResponse>();
            foreach (var item in tiposDeAsistencias)
            {
                GenericResponse response = new GenericResponse
                {
                    Descripcion = item.Descripcion
                };
                responses.Add(response);
            }
            return Task.FromResult(responses);
        }
    }
}
