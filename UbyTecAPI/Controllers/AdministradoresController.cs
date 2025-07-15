using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UbyTecAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace UbyTecAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradoresController : ControllerBase
    {
        private readonly UbyTecContext _context;

        public AdministradoresController(UbyTecContext context)
        {
            _context = context;
        }

        // Obtener todos los administradores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Administrador>>> GetAdministradores()
        {
            return await _context.Administradores
                .Include(a => a.Telefonos) // Incluir los teléfonos asociados
                .ToListAsync();
        }

        // Obtener un administrador por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Administrador>> GetAdministrador(int id)
        {
            var administrador = await _context.Administradores
                .Include(a => a.Telefonos) // Incluir los teléfonos asociados
                .FirstOrDefaultAsync(a => a.AdministradorID == id);

            if (administrador == null)
            {
                return NotFound();
            }

            return administrador;
        }

        // Agregar un nuevo administrador
        [HttpPost]
        public async Task<ActionResult<Administrador>> PostAdministrador(Administrador administrador)
        {
            // Generar una contraseña aleatoria si no se proporciona una
            if (string.IsNullOrEmpty(administrador.PasswordHash))
            {
                administrador.PasswordHash = GenerateRandomPassword();
            }

            // Convertir la contraseña en base64 antes de guardarla en la base de datos
            administrador.PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(administrador.PasswordHash));

            // Agregar el administrador a la base de datos
            _context.Administradores.Add(administrador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdministrador), new { id = administrador.AdministradorID }, administrador);
        }

        // Actualizar un administrador existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdministrador(int id, Administrador administrador)
        {
            if (id != administrador.AdministradorID)
            {
                return BadRequest();
            }

            // Convertir la contraseña en Base64 si se ha proporcionado una nueva
            if (!string.IsNullOrEmpty(administrador.PasswordHash))
            {
                administrador.PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(administrador.PasswordHash));
            }

            _context.Entry(administrador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Administradores.Any(e => e.AdministradorID == id))
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

        // Eliminar un administrador
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdministrador(int id)
        {
            var administrador = await _context.Administradores
                .Include(a => a.Telefonos) // Incluir los teléfonos asociados
                .FirstOrDefaultAsync(a => a.AdministradorID == id);

            if (administrador == null)
            {
                return NotFound();
            }

            // Eliminar los teléfonos asociados antes de eliminar el administrador
            _context.TelefonosAdministradores.RemoveRange(administrador.Telefonos);
            _context.Administradores.Remove(administrador);
            await _context.SaveChangesAsync();

            return NoContent();
        }
         // Nuevo método para modificar un administrador por su número de cédula
        [HttpPut("modificarPorNumeroCedula/{numeroCedula}")]
        public async Task<IActionResult> PutAdministradorByNumeroCedula(string numeroCedula, Administrador administrador)
        {
            // Buscar el administrador por el número de cédula
            var administradorExistente = await _context.Administradores
                .Include(a => a.Telefonos)
                .FirstOrDefaultAsync(a => a.NumeroCedula == numeroCedula);

            if (administradorExistente == null)
            {
                return NotFound("Administrador no encontrado con el número de cédula especificado.");
            }

            // Actualizar los campos del administrador existente con la nueva información
            administradorExistente.NombreCompleto = administrador.NombreCompleto;
            administradorExistente.DireccionProvincia = administrador.DireccionProvincia;
            administradorExistente.DireccionCanton = administrador.DireccionCanton;
            administradorExistente.DireccionDistrito = administrador.DireccionDistrito;
            administradorExistente.Usuario = administrador.Usuario;

            // Actualizar la contraseña si se proporciona una nueva (hasheada)
            if (!string.IsNullOrEmpty(administrador.PasswordHash))
            {
                administradorExistente.PasswordHash = administrador.PasswordHash;
            }

            // Actualizar la lista de teléfonos
            if (administrador.Telefonos != null && administrador.Telefonos.Any())
            {
                // Eliminar los teléfonos actuales
                _context.TelefonosAdministradores.RemoveRange(administradorExistente.Telefonos);

                // Agregar los nuevos teléfonos
                foreach (var telefono in administrador.Telefonos)
                {
                    telefono.AdministradorID = administradorExistente.AdministradorID; // Asignar el AdministradorID
                    _context.TelefonosAdministradores.Add(telefono);
                }
            }

            // Guardar los cambios en la base de datos
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Administradores.Any(e => e.NumeroCedula == numeroCedula))
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

        // Nuevo método para obtener un administrador por número de cédula
        [HttpGet("buscarPorNumeroCedula/{numeroCedula}")]
        public async Task<ActionResult<Administrador>> GetAdministradorByNumeroCedula(string numeroCedula)
        {
            var administrador = await _context.Administradores
                .Include(a => a.Telefonos) // Incluir los teléfonos asociados
                .FirstOrDefaultAsync(a => a.NumeroCedula == numeroCedula);

            if (administrador == null)
            {
                return NotFound("Administrador no encontrado con el número de cédula especificado.");
            }

            return Ok(administrador);
        }

        // Obtener los teléfonos de un administrador específico
        [HttpGet("{id}/telefonos")]
        public async Task<ActionResult<IEnumerable<TelefonoAdministrador>>> GetTelefonosAdministrador(int id)
        {
            var administrador = await _context.Administradores
                .Include(a => a.Telefonos)
                .FirstOrDefaultAsync(a => a.AdministradorID == id);

            if (administrador == null)
            {
                return NotFound();
            }

            return Ok(administrador.Telefonos);
        }

        // Agregar un teléfono a un administrador específico
        [HttpPost("{id}/telefonos")]
        public async Task<IActionResult> AddTelefonoToAdministrador(int id, TelefonoAdministrador telefono)
        {
            var administrador = await _context.Administradores.FindAsync(id);

            if (administrador == null)
            {
                return NotFound("Administrador no encontrado");
            }

            telefono.AdministradorID = id;
            _context.TelefonosAdministradores.Add(telefono);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdministrador), new { id = administrador.AdministradorID }, telefono);
        }

        // Eliminar un teléfono de un administrador
        [HttpDelete("telefonos/{telefonoId}")]
        public async Task<IActionResult> DeleteTelefono(int telefonoId)
        {
            var telefono = await _context.TelefonosAdministradores.FindAsync(telefonoId);
            if (telefono == null)
            {
                return NotFound("Teléfono no encontrado");
            }

            _context.TelefonosAdministradores.Remove(telefono);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método para generar una contraseña aleatoria
        private string GenerateRandomPassword()
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            for (int i = 0; i < 8; i++)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        [HttpDelete("eliminarPorNumeroCedula/{numeroCedula}")]
        public async Task<IActionResult> DeleteAdministradorByNumeroCedula(string numeroCedula)
        {
            var administrador = await _context.Administradores
                .Include(a => a.Telefonos) // Incluir los teléfonos asociados
                .FirstOrDefaultAsync(a => a.NumeroCedula == numeroCedula);

            if (administrador == null)
            {
                return NotFound("Administrador no encontrado con el número de cédula especificado.");
            }

            // Eliminar los teléfonos asociados antes de eliminar el administrador
            _context.TelefonosAdministradores.RemoveRange(administrador.Telefonos);
            _context.Administradores.Remove(administrador);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("nombres-completos")]
        public async Task<ActionResult<IEnumerable<string>>> GetNombresCompletosAdministradores()
        {
            var nombres = await _context.Administradores
                .Select(a => a.NombreCompleto) // Seleccionar solo el campo `NombreCompleto`
                .ToListAsync();

            return Ok(nombres);
        }
    }
}