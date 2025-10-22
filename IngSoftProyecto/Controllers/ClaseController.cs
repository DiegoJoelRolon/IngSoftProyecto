using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Services;
using Microsoft.AspNetCore.Mvc;

namespace IngSoftProyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaseController : ControllerBase
    {
        private readonly ClaseService _claseService;
        public ClaseController(ClaseService claseService)
        {
            _claseService = claseService;
        }
        // GET: api/<ClaseController>
        [HttpGet]
        [ProducesResponseType(typeof(List<ClaseResponse>),200)]
        public async Task<ActionResult<List<ClaseResponse>>>GetAllClases()
        {
            var result = await _claseService.GetAllClases();
            return new JsonResult(result) { StatusCode = 200 };
        }

        // GET api/<ClaseController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClaseResponse),200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClaseResponse>> GetClaseById(int id)
        {
            try
            {
                var result = await _claseService.GetClaseById(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        // POST api/<ClaseController>
        [HttpPost]
        [ProducesResponseType(typeof(ClaseResponse), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClaseResponse>> PostClase(ClaseRequest request)
        {
            try
            {
                var result = await _claseService.AddClaseAsync(request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT api/<ClaseController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ClaseResponse), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClaseResponse>> PutClase(int id, ClaseRequest request)
        {
            try
            {
                var result = await _claseService.UpdateClase(id,request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
