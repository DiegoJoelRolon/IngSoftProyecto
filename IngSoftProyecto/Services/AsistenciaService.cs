using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;
using System.Threading.Tasks;

namespace IngSoftProyecto.Services
{
    public class AsistenciaService
    {
        private readonly AsistenciaCommand _asistenciaCommand;
        private readonly AsistenciaQuery _asistenciaQuery;
        private readonly AsistenciaMaper _asistenciaMapper;
        private readonly TipoDeAsistenciaService _tipoDeAsistenciaService;
        private readonly MembresiaXMiembroService _membresiaXMiembroService;
        private readonly MiembroXClaseService _miembroXClaseService;

        public AsistenciaService(AsistenciaCommand asistenciaCommand, AsistenciaQuery asistenciaQuery,TipoDeAsistenciaService tipoDeAsistenciaService,MiembroXClaseService miembroXClaseService, MembresiaXMiembroService membresiaXMiembroService , AsistenciaMaper asistenciaMapper)
        {
            _asistenciaCommand = asistenciaCommand;
            _asistenciaQuery = asistenciaQuery;
            _tipoDeAsistenciaService = tipoDeAsistenciaService;
            _miembroXClaseService = miembroXClaseService;
            _membresiaXMiembroService = membresiaXMiembroService;
            _asistenciaMapper = asistenciaMapper;
        }

        virtual public async Task<AsistenciaResponse> AddAsistencia(AsistenciaRequest request)
        {
            await CheckAsistenciaRequest(request);
            var asistencia = new Asistencia
            {
                MembresiaXMiembroId = request.MembresiaXMiembroId,
                MiembroXClaseId = request.MiembroXClaseId,
                TipoDeAsistenciaId = request.TipoDeAsistenciaId,
                Fecha = request.Fecha,
            };
            var result = await _asistenciaCommand.AddAsistencia(asistencia);
            return await _asistenciaMapper.GetAsistenciaResponse(await _asistenciaQuery.GetAsistenciaById(result.AsistenciaId));
        }

        virtual public async Task<List<AsistenciaResponse>> GetAllAsistencias()
        {
            var asistencias = await _asistenciaQuery.GetAllAsistencias();
            return await _asistenciaMapper.GetAsistenciaResponseList(asistencias);
        }
        virtual public async Task<AsistenciaResponse> GetAsistenciaById(int asistenciaId)
        {
            await AsistenciaExists(asistenciaId);
            var asistencia = await _asistenciaQuery.GetAsistenciaById(asistenciaId);
            return await _asistenciaMapper.GetAsistenciaResponse(asistencia);
        }
        virtual public async Task<AsistenciaResponse> UpdateAsistencia(int asistenciaId, AsistenciaRequest request)
        {
            await AsistenciaExists(asistenciaId);
            await CheckAsistenciaRequest(request);
            var asistencia = await _asistenciaQuery.GetAsistenciaById(asistenciaId);
            asistencia.MembresiaXMiembroId = request.MembresiaXMiembroId;
            asistencia.MiembroXClaseId = request.MiembroXClaseId;
            asistencia.TipoDeAsistenciaId = request.TipoDeAsistenciaId;
            asistencia.Fecha = request.Fecha;
            var result = await _asistenciaCommand.UpdateAsistencia(asistencia);
            return await _asistenciaMapper.GetAsistenciaResponse(await _asistenciaQuery.GetAsistenciaById(result.AsistenciaId));
        }

        private async Task<bool> AsistenciaExists(int id)
        {
            if (id <= 0 || await _asistenciaQuery.GetAsistenciaById(id) == null)
                throw new NotFoundException("Id de Asistencia invalido");
            return true;
        }
        private async Task<bool> CheckAsistenciaRequest(AsistenciaRequest request)
        {
            if (await _membresiaXMiembroService.GetMembresiaXMiembroById(request.MembresiaXMiembroId)==null)
            {
                throw new BadRequestException("Ids invalidos en la solicitud de MembresiaXMiembro");
            }
            if (await _miembroXClaseService.GetMiembroXClaseById(request.MiembroXClaseId)==null)
            {
                throw new BadRequestException("Ids invalidos en la solicitud de MiembroXClase");
            }
            if (await _tipoDeAsistenciaService.GetTipoDeAsistenciaById(request.TipoDeAsistenciaId) == null)
            {
                throw new BadRequestException("Ids invalidos en la solicitud de TipoDeAsistencia");
            }
            return true;
        }
    }
}
