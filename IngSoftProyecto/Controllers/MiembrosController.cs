using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Services;
using Microsoft.AspNetCore.Mvc;


namespace IngSoftProyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiembrosController : ControllerBase
    {
        private readonly MiembroService _miembroService;

        public MiembrosController(MiembroService miembroService)
        {
            _miembroService = miembroService;
        }

        // GET: api/Miembros
        [HttpGet]
        [ProducesResponseType(typeof(List<MiembroResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MiembroResponse>>> GetMiembros()
        {
            var result = await _miembroService.GetAllMiembros();
            return new JsonResult(result) { StatusCode = 200 };
        }

        // GET: api/Miembros/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MiembroResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MiembroResponse>> GetMiembroById(int id)
        {
            try
            {
                var miembro = await _miembroService.GetMiembroById(id);
                return new JsonResult(miembro) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }


        // POST: api/Miembros
        [HttpPost]
        [ProducesResponseType(typeof(MiembroResponse), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MiembroResponse>> AddMiembro(MiembroRequest request)
        {
            try
            {
                var result = await _miembroService.AddMiembro(request);

                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }


        }
        // PUT: api/Miembros/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MiembroResponse), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MiembroResponse>> UpdateMiembro(int id, MiembroRequest request)
        {
            try
            {
                var result = await _miembroService.UpdateMiembro(id, request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }
        // DELETE: api/Miembros/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MiembroResponse), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMiembro(int id)
        {
            try
            {
                var result = await _miembroService.DeleteMiembro(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
        // RESTORE: api/Miembros/5
        [HttpPut("{id}/Restore")]
        [ProducesResponseType(typeof(MiembroResponse), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RestoreMiembro(int id)
        {
            try
            {
                var result = await _miembroService.RestoreMiembro(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

    }
}
