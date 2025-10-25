using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;

namespace IngSoftProyecto.Services
{
    public class MiembroXClaseService
    {
        private readonly MiembroXClaseCommand _miembroXClaseCommand;
        private readonly MiembroXClaseQuery _miembroXClaseQuery;
        private readonly MiembroXClaseMapper _miembroXClaseMapper;
        private readonly MiembroService _miembroService;
        private readonly ClaseService _claseService;

        public MiembroXClaseService(MiembroXClaseCommand miembroXClaseCommand, MiembroXClaseQuery miembroXClaseQuery, MiembroXClaseMapper miembroXClaseMapper, MiembroService miembroService, ClaseService claseService)
        {
            _miembroXClaseCommand = miembroXClaseCommand;
            _miembroXClaseQuery = miembroXClaseQuery;
            _miembroXClaseMapper = miembroXClaseMapper;
            _miembroService = miembroService;
            _claseService = claseService;
        }
        virtual public async Task<MiembroXClaseResponse> AddMiembroXClase(MiembroXClaseRequest request)
        {
            await CheckMiembroXClaseRequest(request);
            var miembroXClase = new Models.MiembroXClase
            {
                MiembroId = request.MiembroId,
                ClaseId = request.ClaseId,
                FechaInscripcion = request.FechaInscripcion
            };
            var result = await _miembroXClaseCommand.AddMiembroXClase(miembroXClase);
            return await _miembroXClaseMapper.GetMiembroXClaseResponse(await _miembroXClaseQuery.GetMiembroXClaseById(result.MiembroXClaseId));
        }
        virtual public async Task<List<MiembroXClaseResponse>> GetAllMiembrosXClases()
        {
            var miembrosXClases = await _miembroXClaseQuery.GetAllMiembrosXClases();
            return await _miembroXClaseMapper.GetMiembroXClaseResponseList(miembrosXClases);
        }
        virtual public async Task<MiembroXClaseResponse> GetMiembroXClaseById(int miembroXClaseId)
        {
            await MiembroXClaseExists(miembroXClaseId);
            var miembroXClase = await _miembroXClaseQuery.GetMiembroXClaseById(miembroXClaseId);
            return await _miembroXClaseMapper.GetMiembroXClaseResponse(miembroXClase);
        }
        virtual public async Task<MiembroXClaseResponse> UpdateMiembroXClase(int miembroXClaseId, MiembroXClaseRequest request)
        {
            await MiembroXClaseExists(miembroXClaseId);
            await CheckMiembroXClaseRequest(request);
            var miembroXClase = await _miembroXClaseQuery.GetMiembroXClaseById(miembroXClaseId);
            miembroXClase.MiembroId = request.MiembroId;
            miembroXClase.ClaseId = request.ClaseId;
            miembroXClase.FechaInscripcion = request.FechaInscripcion;
            var result = await _miembroXClaseCommand.UpdateMiembroXClase(miembroXClase);
            return await _miembroXClaseMapper.GetMiembroXClaseResponse(await _miembroXClaseQuery.GetMiembroXClaseById(result.MiembroXClaseId));
        }

        private async Task MiembroXClaseExists(int id)
        {
            var miembroXClase = await _miembroXClaseQuery.GetMiembroXClaseById(id);
            if (miembroXClase == null)
            {
                throw new NotFoundException($"Id de MiembroXClase invalido");
            }
        }
        private async Task CheckMiembroXClaseRequest(MiembroXClaseRequest request)
        {
            if (_miembroService.GetMiembroById(request.MiembroId)==null)
            {
                throw new BadRequestException("Id de Miembro invalido");
            }
            if (_claseService.GetClaseById(request.ClaseId)==null)
            {
                throw new BadRequestException("Id de Clase invalido");
            }
            if (request.FechaInscripcion > DateTime.Now)
            {
                throw new BadRequestException("Fecha de inscripcion invalida");
            }

        }

    }
}
