using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Services;
using Microsoft.AspNetCore.Mvc;

namespace IngSoftProyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadesController : ControllerBase
    {
        private readonly ActividadService _actividadService;

        public ActividadesController(ActividadService actividadService)
        {
            _actividadService = actividadService;
        }

        // GET: api/Actividades
        [HttpGet]
        [ProducesResponseType(typeof(List<ActividadResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ActividadResponse>>> GetActividades()
        {
            var result = await _actividadService.GetAllActividades();
            return new JsonResult(result) { StatusCode = 200 };
        }
        // POST: api/Actividades
        [HttpPost]
        [ProducesResponseType(typeof(ActividadResponse), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ActividadResponse>> PostActividad(ActividadRequest request)
        {
            try
            {
                var result = await _actividadService.AddActividad(request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ActividadResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ActividadResponse>> GetActividadById(int id)
        {
            try
            {
                var actividad = await _actividadService.GetActividadById(id);
                return new JsonResult(actividad) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
        /*
        // GET: api/Actividades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Actividad>> GetActividad(int id)
        {
            var actividad = await _context.Actividades.FindAsync(id);

            if (actividad == null)
            {
                return NotFound();
            }

            return actividad;
        }

        // PUT: api/Actividades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActividad(int id, Actividad actividad)
        {
            if (id != actividad.ActividadId)
            {
                return BadRequest();
            }

            _context.Entry(actividad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActividadExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        

        // DELETE: api/Actividades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActividad(int id)
        {
            var actividad = await _context.Actividades.FindAsync(id);
            if (actividad == null)
            {
                return NotFound();
            }

            _context.Actividades.Remove(actividad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActividadExists(int id)
        {
            return _context.Actividades.Any(e => e.ActividadId == id);
        }*/
    }
}
