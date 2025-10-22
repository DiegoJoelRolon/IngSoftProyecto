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
    public class EntrenadoresController : ControllerBase
    {
        private readonly EntrenadorService _entrenadorService;

        public EntrenadoresController(EntrenadorService entrenadorService)
        {
            _entrenadorService = entrenadorService;
        }

        // GET: api/Entrenadores
        [HttpGet]
        [ProducesResponseType(typeof(List<EntrenadorResponse>), 200)]
        public async Task<ActionResult<IEnumerable<EntrenadorResponse>>> GetEntrenadores()
        {
            var result = await _entrenadorService.GetAllEntrenadores();
            return new JsonResult(result) { StatusCode = 200 };

        }

        // GET: api/Entrenadores/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EntrenadorResponse), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EntrenadorResponse>> GetEntrenadorById(int id)
        {
            
            try
            {
                var result = await _entrenadorService.GetEntrenadorById(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }



        }

        // PUT: api/Entrenadores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EntrenadorResponse), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EntrenadorResponse>> PutEntrenador(int id, EntrenadorRequest entrenador)
        {
            try
            {
                var result = await _entrenadorService.UpdateEntrenadorAsync(id, entrenador);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/Entrenadores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(typeof(EntrenadorResponse), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EntrenadorResponse>> PostEntrenador(EntrenadorRequest entrenador)
        {
            try
            {
                var result = await _entrenadorService.AddEntrenadorAsync(entrenador);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

      
    }
}
