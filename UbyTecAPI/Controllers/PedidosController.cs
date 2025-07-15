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
    public class PedidoController : ControllerBase
    {
        private readonly UbyTecContext _context;

        public PedidoController(UbyTecContext context)
        {
            _context = context;
        }

        // GET: api/Pedido
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            var pedidos = await _context.Pedidos.ToListAsync();
            return Ok(pedidos);
        }

        // GET: api/Pedido/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound("Pedido no encontrado.");
            }

            return Ok(pedido);
        }

        // POST: api/Pedido
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPedido), new { id = pedido.PedidoID }, pedido);
        }

        // PUT: api/Pedido/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(int id, Pedido pedido)
        {
            if (id != pedido.PedidoID)
            {
                return BadRequest("El ID del pedido no coincide.");
            }

            _context.Entry(pedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Pedidos.Any(e => e.PedidoID == id))
                {
                    return NotFound("Pedido no encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Pedido/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound("Pedido no encontrado.");
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Pedido/moverVentasCompletadas
        [HttpPost("moverVentasCompletadas")]
        public async Task<IActionResult> MoverVentasCompletadas()
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC MoverVentasCompletadas");
                return Ok("Ventas completadas movidas exitosamente a la tabla Pedido.");
            }
            catch (Exception ex)
            {
                // Registrar el error exacto
                Console.WriteLine($"Error en MoverVentasCompletadas: {ex}");
                return StatusCode(500, $"Error al mover las ventas completadas: {ex.Message}");
            }
        }


        // GET: api/Pedidos/cliente/{clienteId}
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidosByClienteId(int clienteId)
        {
            var pedidos = await _context.Pedidos
                .Where(p => p.ClienteID == clienteId)
                .ToListAsync();

            if (pedidos == null || !pedidos.Any())
            {
                return NotFound($"No se encontraron pedidos para el ClienteID: {clienteId}");
            }

            return pedidos;
        }

        // GET: api/Pedidos/afiliado/{afiliadoId}
        [HttpGet("afiliado/{afiliadoId}")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidosByAfiliadoId(int afiliadoId)
        {
            var pedidos = await _context.Pedidos
                .Where(p => p.AfiliadoID == afiliadoId)
                .ToListAsync();

            if (pedidos == null || !pedidos.Any())
            {
                return NotFound($"No se encontraron pedidos para el AfiliadoID: {afiliadoId}");
            }

            return pedidos;
        }


        // Endpoint para obtener reporte de ventas agrupados por cliente, comercio y repartidor
    [HttpGet("ventasGenerales")]
    public async Task<ActionResult<IEnumerable<object>>> GetVentasGenerales()
    {
        var reporte = await _context.Pedidos
            .GroupBy(p => new { p.ClienteID, p.AfiliadoID, p.RepartidorID })
            .Select(g => new 
            {
                NombreCliente = g.FirstOrDefault().NombreCliente,
                NombreComercio = g.FirstOrDefault().NombreComercio,
                NombreRepartidor = g.FirstOrDefault().NombreRepartidor,
                TotalCompras = g.Sum(x => x.Total)
            })
            .ToListAsync();

        if (!reporte.Any())
        {
            return NotFound("No se encontraron datos de ventas generales.");
        }

        return Ok(reporte);
    }

         // GET: api/Pedidos/porAfiliado
        [HttpGet("porAfiliado")]
        public async Task<ActionResult<IEnumerable<IGrouping<int, Pedido>>>> GetPedidosAgrupadosPorAfiliado()
        {
            var pedidosAgrupados = await _context.Pedidos
                .GroupBy(p => p.AfiliadoID)
                .ToListAsync();

            if (!pedidosAgrupados.Any())
            {
                return NotFound("No se encontraron pedidos agrupados por afiliado.");
            }

            return Ok(pedidosAgrupados);
        }
    }
}
