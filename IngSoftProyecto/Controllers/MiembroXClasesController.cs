using IngSoftProyecto.Context;
using IngSoftProyecto.Exceptions;
using IngSoftProyecto.Models;
using IngSoftProyecto.Models.DTOs.Request;
using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngSoftProyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiembroXClasesController : ControllerBase
    {
        private readonly MiembroXClaseService _service ;

        public MiembroXClasesController(MiembroXClaseService service )
        {
            _service = service;
        }

        // GET: api/MiembroXClases
        [HttpGet]
        [ProducesResponseType(typeof(List<ClaseResponse>), 200)]
        public async Task<ActionResult<IEnumerable<MiembroXClaseResponse>>> GetAllMiembrosXClases()
        {
            var result = await _service.GetAllMiembrosXClases();
            return new JsonResult(result) { StatusCode = 200 };
        }

        // GET: api/MiembroXClases/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MiembroXClaseResponse), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MiembroXClaseResponse>> GetMiembroXClaseById(int id)
        {
            try
            {
                var result = await _service.GetMiembroXClaseById(id);
                return new JsonResult(result) { StatusCode = 200 };

            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }

        }

        // PUT: api/MiembroXClases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MiembroXClaseResponse), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MiembroXClaseResponse>> PutMiembroXClase(int id, MiembroXClaseRequest request)
        {
            try
            {
                var result = await _service.UpdateMiembroXClase(id, request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/MiembroXClases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(typeof(MiembroXClaseResponse), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MiembroXClaseResponse>> PostMiembroXClase(MiembroXClaseRequest request)
        {
            try
            {
                var result = await _service.AddMiembroXClase(request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
