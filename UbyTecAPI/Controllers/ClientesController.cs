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
    public class ClientesController : ControllerBase
    {
        private readonly UbyTecContext _context;

        public ClientesController(UbyTecContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.ClienteID }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.ClienteID)
            {
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Clientes.Any(e => e.ClienteID == id))
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

        [HttpGet("byusername/{usuario}")]
        public async Task<ActionResult<Cliente>> GetClienteByUsuario(string usuario)
        {
            var cliente = await _context.Clientes.SingleOrDefaultAsync(c => c.Usuario == usuario);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("bycedula/{numeroCedula}")]
        public async Task<IActionResult> PutClienteByCedula(string numeroCedula, [FromBody] UpdateClienteDto updatedCliente)
        {
            // Buscar el cliente existente
            var cliente = await _context.Clientes.SingleOrDefaultAsync(c => c.NumeroCedula == numeroCedula);

            if (cliente == null)
            {
                return NotFound(new { message = "Cliente no encontrado con el número de cédula proporcionado." });
            }

            // Actualizar campos permitidos
            if (!string.IsNullOrWhiteSpace(updatedCliente.Nombre))
            {
                cliente.Nombre = updatedCliente.Nombre;
            }
            if (!string.IsNullOrWhiteSpace(updatedCliente.Apellidos))
            {
                cliente.Apellidos = updatedCliente.Apellidos;
            }
            if (!string.IsNullOrWhiteSpace(updatedCliente.DireccionProvincia))
            {
                cliente.DireccionProvincia = updatedCliente.DireccionProvincia;
            }
            if (!string.IsNullOrWhiteSpace(updatedCliente.DireccionCanton))
            {
                cliente.DireccionCanton = updatedCliente.DireccionCanton;
            }
            if (!string.IsNullOrWhiteSpace(updatedCliente.DireccionDistrito))
            {
                cliente.DireccionDistrito = updatedCliente.DireccionDistrito;
            }

            // Guardar los cambios
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { message = "Ocurrió un error al actualizar el cliente." });
            }

            return NoContent();
        }

        [HttpDelete("bycedula/{numeroCedula}")]
        public async Task<IActionResult> DeleteClienteByCedula(string numeroCedula)
        {
            // Buscar el cliente por numero de cedula
            var cliente = await _context.Clientes.SingleOrDefaultAsync(c => c.NumeroCedula == numeroCedula);

            if (cliente == null)
            {
                return NotFound(new { message = "Cliente no encontrado con el número de cédula proporcionado." });
            }

            // Eliminar el cliente
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
