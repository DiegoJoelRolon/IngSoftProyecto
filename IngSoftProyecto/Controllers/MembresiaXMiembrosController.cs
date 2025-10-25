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
    public class MembresiaXMiembrosController : ControllerBase
    {
        private readonly MembresiaXMiembroService _membresiaXMiembroService;

        public MembresiaXMiembrosController(MembresiaXMiembroService membresiaXMiembroService)
        {
            _membresiaXMiembroService = membresiaXMiembroService;
        }

        // GET: api/MembresiaXMiembroes
        [HttpGet]
        [ProducesResponseType(typeof(List<MembresiaXMiembroResponse>), 200)]
        public async Task<ActionResult<IEnumerable<MembresiaXMiembroResponse>>> GetAllMembresiasXMiembros()
        {
            return await _membresiaXMiembroService.GetAllMembresiasXMiembros();
        }

        // GET: api/MembresiaXMiembroes/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MembresiaXMiembroResponse),200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MembresiaXMiembroResponse>> GetMembresiaXMiembroById(int id)
        {
            try
            {
                var result = await _membresiaXMiembroService.GetMembresiaXMiembroById(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }

        }

        // PUT: api/MembresiaXMiembroes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MembresiaXMiembroResponse), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MembresiaXMiembroResponse>> PutMembresiaXMiembro(int id, MembresiaXMiembroRequest request)
        {
            try
            {
                var result = await _membresiaXMiembroService.UpdateMembresiaXMiembro(id, request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/MembresiaXMiembroes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(typeof(MembresiaXMiembroResponse), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MembresiaXMiembroResponse>> PostMembresiaXMiembro(MembresiaXMiembroRequest request)
        {
            try
            {
                var result = await _membresiaXMiembroService.AddMembresiaXMiembro(request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
