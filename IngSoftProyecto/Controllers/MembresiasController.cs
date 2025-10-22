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
    public class MembresiasController : ControllerBase
    {
        private readonly MembresiasService _service;

        public MembresiasController(MembresiasService service)
        {
            _service = service;
        }

        // GET: api/Membresias
        [HttpGet]
        [ProducesResponseType(typeof(List<MembresiaResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MembresiaResponse>>> GetMembresias()
        {
            var result = await _service.GetAllMembresias();
            return new JsonResult(result) { StatusCode = 200 };
        }

        // GET: api/Membresias/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MembresiaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MembresiaResponse>> GetMembresia(int id)
        {
            try 
            {
                var membresia = await _service.GetMembresiaById(id);
                return new JsonResult(membresia) { StatusCode = 200 };

            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        // PUT: api/Membresias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<MembresiaResponse>> PutMembresia(int id, MembresiaRequest membresia)
        {
            try
            {
                var result = await _service.UpdateMembresia(id, membresia);
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

        // POST: api/Membresias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(typeof(MembresiaResponse), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MembresiaResponse>> PostMembresia(MembresiaRequest membresia)
        {
            try
            {
                var result =  await _service.AddMembresia(membresia);
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
    }
}
