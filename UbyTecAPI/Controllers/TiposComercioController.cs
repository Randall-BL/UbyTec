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
    public class TiposComercioController : ControllerBase
    {
        private readonly UbyTecContext _context;

        public TiposComercioController(UbyTecContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoComercio>>> GetTiposComercio()
        {
            return await _context.TiposComercio.ToListAsync();
        }

        // Nuevo método para obtener todos los nombres de los tipos de comercio
        [HttpGet("nombres")]
        public async Task<ActionResult<IEnumerable<string>>> GetNombresTiposComercio()
        {
            var nombres = await _context.TiposComercio
                .Select(tc => tc.NombreTipo)
                .ToListAsync();
            return Ok(nombres);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoComercio>> GetTipoComercio(int id)
        {
            var tipoComercio = await _context.TiposComercio.FindAsync(id);

            if (tipoComercio == null)
            {
                return NotFound();
            }

            return tipoComercio;
        }

        [HttpPost]
        public async Task<ActionResult<TipoComercio>> PostTipoComercio(TipoComercio tipoComercio)
        {
            // Verificar si ya existe un tipo de comercio con el mismo nombre (sin importar mayúsculas/minúsculas)
            var nombreExistente = await _context.TiposComercio
                .AnyAsync(tc => tc.NombreTipo.ToLower() == tipoComercio.NombreTipo.ToLower());

            if (nombreExistente)
            {
                return BadRequest("El nombre del tipo de comercio ya existe.");
            }

            _context.TiposComercio.Add(tipoComercio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTipoComercio), new { id = tipoComercio.TipoComercioID }, tipoComercio);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoComercio(int id, TipoComercio tipoComercio)
        {
            if (id != tipoComercio.TipoComercioID)
            {
                return BadRequest();
            }

            // Verificar si ya existe otro tipo de comercio con el mismo nombre (sin importar mayúsculas/minúsculas)
            var nombreExistente = await _context.TiposComercio
                .AnyAsync(tc => tc.TipoComercioID != id && tc.NombreTipo.ToLower() == tipoComercio.NombreTipo.ToLower());

            if (nombreExistente)
            {
                return BadRequest("El nombre del tipo de comercio ya existe.");
            }

            _context.Entry(tipoComercio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TiposComercio.Any(e => e.TipoComercioID == id))
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
        public async Task<IActionResult> DeleteTipoComercio(int id)
        {
            var tipoComercio = await _context.TiposComercio.FindAsync(id);
            if (tipoComercio == null)
            {
                return NotFound();
            }

            _context.TiposComercio.Remove(tipoComercio);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("eliminarPorNombre/{nombre}")]
        public async Task<IActionResult> DeleteTipoComercioPorNombre(string nombre)
        {
            // Busca el tipo de comercio por su nombre
            var tipoComercio = await _context.TiposComercio
                .Include(tc => tc.Afiliados) // Incluye la relación con los negocios
                .FirstOrDefaultAsync(tc => tc.NombreTipo == nombre);

            if (tipoComercio == null)
            {
                return NotFound("Tipo de comercio no encontrado.");
            }

            try
            {
                // Quitar la relación con los negocios asociados al tipo de comercio
                if (tipoComercio.Afiliados != null)
                {
                    foreach (var negocio in tipoComercio.Afiliados)
                    {
                        negocio.TipoComercio = null; // Eliminar la referencia al tipo de comercio
                    }
                }

                // Eliminar el tipo de comercio
                _context.TiposComercio.Remove(tipoComercio);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el tipo de comercio: {ex.Message}");
                return StatusCode(500, "Error interno al eliminar el tipo de comercio.");
            }
        }
    }
}
