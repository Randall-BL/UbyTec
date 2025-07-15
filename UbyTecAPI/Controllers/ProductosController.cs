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
    public class ProductosController : ControllerBase
    {
        private readonly UbyTecContext _context;

        public ProductosController(UbyTecContext context)
        {
            _context = context;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos.Include(p => p.Afiliado).ToListAsync();
        }

        // GET: api/Productos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos.Include(p => p.Afiliado).FirstOrDefaultAsync(p => p.ProductoID == id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            // Validar si el afiliado existe
            var afiliado = await _context.Afiliados.FindAsync(producto.AfiliadoID);
            if (afiliado == null)
            {
                return BadRequest("Afiliado no encontrado.");
            }

            // Guardar el producto
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.ProductoID }, producto);
        }

        // PUT: api/Productos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.ProductoID)
            {
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
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

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.ProductoID == id);
        }

        [HttpGet("buscarPorNombre")]
        public async Task<ActionResult<Producto>> BuscarProductoPorNombre(string nombreProducto)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.NombreProducto == nombreProducto);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }
        [HttpPut("afiliado/{afiliadoID}/nombre/{nombreProducto}")]
        public async Task<IActionResult> UpdateProducto(int afiliadoID, string nombreProducto, Producto updatedProducto)
        {
            var existingProducto = await _context.Productos
                .FirstOrDefaultAsync(p => p.AfiliadoID == afiliadoID && p.NombreProducto == nombreProducto);

            if (existingProducto == null)
            {
                return NotFound("No se encontró un producto con ese nombre para el afiliado actual.");
            }

            // Actualizar los valores del producto existente
            existingProducto.Categoria = updatedProducto.Categoria;
            existingProducto.Foto = updatedProducto.Foto;
            existingProducto.Precio = updatedProducto.Precio;

            _context.Entry(existingProducto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }
            [HttpGet("buscarPorAfiliadoYNombre")]
        public IActionResult BuscarPorAfiliadoYNombre(int afiliadoID, string nombreProducto)
        {
            try
            {
                var producto = _context.Productos
                    .FirstOrDefault(p => p.AfiliadoID == afiliadoID && p.NombreProducto == nombreProducto);
                
                if (producto == null)
                {
                    return NotFound("Producto no encontrado.");
                }

                return Ok(producto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al buscar el producto: {ex.Message}");
            }
        }
    }
}
