using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Mapper
{
    public class MembresiaXMiembroMapper
    {
        private readonly MiembroMapper _miembroMapper;
        private readonly MembresiaMapper _membresiaMapper;
        private readonly PagoMapper _pagoMapper;
        private readonly GenericMapper _genericResponseMapper;
        public MembresiaXMiembroMapper(MiembroMapper miembroMapper, MembresiaMapper membresiaMapper,PagoMapper pagoMapper ,GenericMapper genericResponseMapper)
        {
            _miembroMapper = miembroMapper;
            _membresiaMapper = membresiaMapper;
            _pagoMapper = pagoMapper;
            _genericResponseMapper = genericResponseMapper;
        }
        public virtual async Task<MembresiaXMiembroResponse>GetMembresiaXMiembroResponse(MembresiaXMiembro  membresiaXMiembro)
        {
            return new MembresiaXMiembroResponse
            {
                MembresiaXMiembroId = membresiaXMiembro.MembresiaXMiembroId,
                Miembro = await _miembroMapper.GetMiembroResponse(membresiaXMiembro.Miembro),
                Membresia = await _membresiaMapper.GetMembresiaResponse(membresiaXMiembro.Membresia),
                FechaInicio = membresiaXMiembro.FechaInicio,
                FechaFin = membresiaXMiembro.FechaFin,
                EstadoMembresia = await _genericResponseMapper.GetGenericResponse(membresiaXMiembro.EstadoMembresia),
                Pago=await _pagoMapper.GetPagoResponse(membresiaXMiembro.Pago),
            };
        }
        public virtual async Task<List<MembresiaXMiembroResponse>> GetMembresiaXMiembroResponseList(List<MembresiaXMiembro> membresiasXMiembros)
        {
            var membresiasXMiembrosResponse = new List<MembresiaXMiembroResponse>();
            foreach (var membresiaXMiembro in membresiasXMiembros)
            {
                membresiasXMiembrosResponse.Add(await GetMembresiaXMiembroResponse(membresiaXMiembro));
            }
            return membresiasXMiembrosResponse;
        }
    }
}
