using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IngSoftProyecto.Context;
using IngSoftProyecto.Models;
using IngSoftProyecto.Services;
using IngSoftProyecto.Models.DTOs.Response;
using IngSoftProyecto.Models.DTOs.Request;

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
        public async Task<ActionResult<IEnumerable<MiembroResponse>>> GetMiembros()
        {
            var result = await _miembroService.GetAllMiembros();
            return new JsonResult(result);
        }

        // GET: api/Miembros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MiembroResponse>> GetMiembro(int id)
        {
            var miembro = await _miembroService.GetMiembroById(id);

            if (miembro == null)
            {
                return NotFound();
            }

            return new JsonResult(miembro);
        }

        
        // POST: api/Miembros
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MiembroResponse>> PostMiembro(MiembroRequest request)
        {
            var result = await _miembroService.AddMiembro(request);
            
            return new JsonResult(result) { StatusCode = 201 };

        }

        

        /*private bool MiembroExists(int id)
        {
            return _context.Miembros.Any(e => e.MiembroId == id);
        }*/
    }
}
