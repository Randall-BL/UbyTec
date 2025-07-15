using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UbyTecAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UbyTecAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoRepartidorController : ControllerBase
    {
        private readonly UbyTecContext _context;

        public EstadoRepartidorController(UbyTecContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoRepartidor>>> GetEstadoRepartidores()
        {
            return await _context.EstadoRepartidor.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoRepartidor>> GetEstadoRepartidor(int id)
        {
            var estadoRepartidor = await _context.EstadoRepartidor.FindAsync(id);

            if (estadoRepartidor == null)
            {
                return NotFound();
            }

            return estadoRepartidor;
        }

        [HttpPost]
        public async Task<ActionResult<EstadoRepartidor>> PostEstadoRepartidor(EstadoRepartidor estadoRepartidor)
        {
            _context.EstadoRepartidor.Add(estadoRepartidor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEstadoRepartidor), new { id = estadoRepartidor.RepartidorID }, estadoRepartidor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoRepartidor(int id, EstadoRepartidor estadoRepartidor)
        {
            if (id != estadoRepartidor.RepartidorID)
            {
                return BadRequest();
            }

            _context.Entry(estadoRepartidor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.EstadoRepartidor.Any(e => e.RepartidorID == id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoRepartidor(int id)
        {
            var estadoRepartidor = await _context.EstadoRepartidor.FindAsync(id);
            if (estadoRepartidor == null)
            {
                return NotFound();
            }

            _context.EstadoRepartidor.Remove(estadoRepartidor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
