using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IngSoftProyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciasController : ControllerBase
    {
        private readonly AsistenciaService _asistenciaService;
        public AsistenciasController(AsistenciaService asistenciaService)
        {
            _asistenciaService = asistenciaService;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(List<AsistenciaResponse>), 200)]
        public async Task<ActionResult<List<AsistenciaResponse>>> GetAllAsistencias()
        {
            var result = await _asistenciaService.GetAllAsistencias();
            return new JsonResult(result) { StatusCode = 200 };
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AsistenciaResponse), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AsistenciaResponse>> GetAsistenciaeById(int id)
        {
            try
            {
                var result = await _asistenciaService.GetAsistenciaById(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(AsistenciaResponse), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AsistenciaResponse>> PostAsistencia(AsistenciaRequest request)
        {
            try
            {
                var result = await _asistenciaService.AddAsistencia(request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AsistenciaResponse), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AsistenciaResponse>> PutAsistencia(int id, AsistenciaRequest request)
        {
            try
            {
                var result = await _asistenciaService.UpdateAsistencia(id, request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }



    }
}
