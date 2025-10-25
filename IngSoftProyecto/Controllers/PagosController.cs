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
    public class PagosController : ControllerBase
    {
        private readonly PagoService _pagoService;

        public PagosController(PagoService pagoService)
        {
            _pagoService = pagoService;
        }

        // GET: api/Pagos
        [HttpGet]
        [ProducesResponseType(typeof(List<PagoResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PagoResponse>>> GetPagos()
        {
            var result= await _pagoService.GetAllPagos();
            return new JsonResult(result) { StatusCode = 200 };
        }

        // GET: api/Pagos/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PagoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagoResponse>> GetPagoById(int id)
        {
            try
            {
                var result = await _pagoService.GetPagoById(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        // PUT: api/Pagos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PagoResponse), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutPago(int id, PagoRequest request)
        {
            try
            {
                var result = await _pagoService.UpdatePago(id, request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/Pagos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(typeof(PagoResponse), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pago>> PostPago(PagoRequest request)
        {
            try
            {
                var result = await _pagoService.AddPago(request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
