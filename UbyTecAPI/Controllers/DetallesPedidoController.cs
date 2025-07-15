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
    public class DetallesPedidoController : ControllerBase
    {
        private readonly UbyTecContext _context;

        public DetallesPedidoController(UbyTecContext context)
        {
            _context = context;
        }

        // GET: api/DetallesPedido
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePedido>>> GetDetallesPedido()
        {
            return await _context.DetallesPedido
                .Include(dp => dp.Productos) // Incluye la colección de productos
                .ToListAsync();
        }

        // GET: api/DetallesPedido/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePedido>> GetDetallePedido(int id)
        {
            var detallePedido = await _context.DetallesPedido
                .Include(dp => dp.Productos) // Incluye la colección de productos
                .FirstOrDefaultAsync(dp => dp.DetalleID == id);

            if (detallePedido == null)
            {
                return NotFound();
            }

            return detallePedido;
        }

        // POST: api/DetallesPedido
       [HttpPost]
        public async Task<ActionResult<DetallePedido>> PostDetallePedido(DetallePedido detallePedido)
{
    // Validar que el AfiliadoID exista
    var afiliado = await _context.Afiliados.FirstOrDefaultAsync(a => a.AfiliadoID == detallePedido.AfiliadoID);
    if (afiliado == null)
    {
        return BadRequest("El AfiliadoID proporcionado no existe.");
    }

    // Obtener repartidores disponibles en la provincia del afiliado
    var repartidoresDisponibles = await _context.Repartidores
        .Where(r => r.DireccionProvincia == afiliado.DireccionProvincia &&
                    !_context.DetallesPedido.Any(dp =>
                        dp.RepartidorID == r.RepartidorID &&
                        dp.Estado != "Completado")) // Repartidor ya asignado a pedido activo
        .ToListAsync();

    if (!repartidoresDisponibles.Any())
    {
        return Conflict("No hay repartidores disponibles en la misma provincia.");
    }

    // Seleccionar un repartidor aleatorio
    var random = new Random();
    var repartidorSeleccionado = repartidoresDisponibles[random.Next(repartidoresDisponibles.Count)];

    // Asignar el RepartidorID seleccionado al DetallePedido
    detallePedido.RepartidorID = repartidorSeleccionado.RepartidorID;

    // Agregar el DetallePedido y su colección de Productos
    _context.DetallesPedido.Add(detallePedido);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetDetallePedido), new { id = detallePedido.DetalleID }, detallePedido);
}


        // PUT: api/DetallesPedido/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetallePedido(int id, [FromBody] DetallePedido detallePedido)
        {
            if (id != detallePedido.DetalleID)
            {
                return BadRequest("El ID del DetallePedido no coincide.");
            }

            // Verificar si el DetallePedido existe
            var detalleExistente = await _context.DetallesPedido.FindAsync(id);
            if (detalleExistente == null)
            {
                return NotFound("El DetallePedido no existe.");
            }

            try
            {
                // Llamar al procedimiento almacenado para actualizar el estado
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateDetallePedido @DetalleID = {0}, @Estado = {1}",
                    id,
                    detallePedido.Estado
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el DetallePedido: {ex.Message}");
            }

            return NoContent();
        }

        // DELETE: api/DetallesPedido/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetallePedido(int id)
        {
            var detallePedido = await _context.DetallesPedido
                .Include(dp => dp.Productos) // Incluye la colección de productos
                .FirstOrDefaultAsync(dp => dp.DetalleID == id);

            if (detallePedido == null)
            {
                return NotFound();
            }

            // Elimina los productos relacionados primero
            _context.ProductosDetalle.RemoveRange(detallePedido.Productos);

            // Luego elimina el detalle de pedido
            _context.DetallesPedido.Remove(detallePedido);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("porCliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<DetallePedido>>> GetDetallesPedidoPorCliente(int clienteId)
        {
            var detallesPedido = await _context.DetallesPedido
                .Where(dp => dp.ClienteID == clienteId && dp.Estado != "Completado") // Filtrar por ClienteID y estado
                .Include(dp => dp.Productos) // Incluir productos asociados
                .Select(dp => new
                {
                    dp.DetalleID,
                    dp.AfiliadoID,
                    dp.ClienteID,
                    dp.RepartidorID,
                    dp.Estado,
                    Productos = dp.Productos.Select(p => new
                    {
                        p.ProductoID,
                        p.Cantidad,
                        p.Precio
                    })
                })
                .ToListAsync();

            if (!detallesPedido.Any())
            {
                return NotFound("No hay pedidos para el cliente con el estado especificado.");
            }

            return Ok(detallesPedido);
        }

        [HttpGet("porAfiliado/{afiliadoId}")]
    public async Task<ActionResult<IEnumerable<DetallePedido>>> GetDetallesPedidoPorAfiliado(int afiliadoId)
    {
        var detallesPedido = await _context.DetallesPedido
            .Where(dp => dp.AfiliadoID == afiliadoId) // Filtrar por AfiliadoID
            .Include(dp => dp.Productos) // Incluir los productos relacionados
            .Select(dp => new
            {
                dp.DetalleID,
                dp.AfiliadoID,
                dp.ClienteID,
                dp.RepartidorID,
                dp.Estado,
                Productos = dp.Productos.Select(p => new
                {
                    p.ProductoID,
                    p.Cantidad,
                    p.Precio
                })
            })
            .ToListAsync();

        if (!detallesPedido.Any())
        {
            return NotFound("No hay pedidos para el afiliado especificado.");
        }

        return Ok(detallesPedido);
    }

    }
    
}

