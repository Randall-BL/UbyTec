using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UbyTecAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace UbyTecAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepartidoresController : ControllerBase
    {
        private readonly UbyTecContext _context;

        public RepartidoresController(UbyTecContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Repartidor>>> GetRepartidores()
        {
            return await _context.Repartidores
                .Include(r => r.Telefonos) // Incluir los teléfonos asociados
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Repartidor>> GetRepartidor(int id)
        {
            var repartidor = await _context.Repartidores
                .Include(r => r.Telefonos) // Incluir los teléfonos asociados
                .FirstOrDefaultAsync(r => r.RepartidorID == id);

            if (repartidor == null)
            {
                return NotFound();
            }

            return repartidor;
        }

        // Nuevo método para obtener un repartidor por usuario
        [HttpGet("buscarPorUsuario/{usuario}")]
        public async Task<ActionResult<Repartidor>> GetRepartidorPorUsuario(string usuario)
        {
            var repartidor = await _context.Repartidores
                .Include(r => r.Telefonos) // Incluir los teléfonos asociados
                .FirstOrDefaultAsync(r => r.Usuario == usuario);

            if (repartidor == null)
            {
                return NotFound("Repartidor no encontrado con el usuario especificado.");
            }

            return Ok(repartidor);
        }

        [HttpPost]
        public async Task<ActionResult<Repartidor>> PostRepartidor(Repartidor repartidor)
        {
            // Generar una contraseña aleatoria si no se proporciona una
            if (string.IsNullOrEmpty(repartidor.PasswordHash))
            {
                string plainPassword = GenerateRandomPassword();
                repartidor.PasswordHash = plainPassword;

                // Enviar el correo electrónico al repartidor con la contraseña sin hashear
                EnviarCorreoContrasena(repartidor.CorreoElectronico, plainPassword);

                // Convertir la contraseña en base64 antes de guardarla en la base de datos
                repartidor.PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainPassword));
            }

            // Agregar el repartidor a la base de datos sin asignar explícitamente el RepartidorID
            _context.Repartidores.Add(repartidor);
            await _context.SaveChangesAsync();

            // Agregar los teléfonos asociados si existen
            if (repartidor.Telefonos != null && repartidor.Telefonos.Any())
            {
                foreach (var telefono in repartidor.Telefonos)
                {
                    telefono.TelefonoRepartidorID = 0; // Para que sea autogenerado por la base de datos
                    telefono.RepartidorID = repartidor.RepartidorID; // Asignar el RepartidorID recién generado
                    _context.TelefonosRepartidores.Add(telefono);
                }
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetRepartidor), new { id = repartidor.RepartidorID }, repartidor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRepartidor(int id, Repartidor repartidor)
        {
            if (id != repartidor.RepartidorID)
            {
                return BadRequest();
            }

            _context.Entry(repartidor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Repartidores.Any(e => e.RepartidorID == id))
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

        // Nuevo método para actualizar un repartidor por usuario
        [HttpPut("modificarPorUsuario/{usuario}")]
        public async Task<IActionResult> PutRepartidorPorUsuario(string usuario, Repartidor repartidor)
        {
            var repartidorExistente = await _context.Repartidores
                .Include(r => r.Telefonos) // Incluir los teléfonos actuales
                .FirstOrDefaultAsync(r => r.Usuario == usuario);

            if (repartidorExistente == null)
            {
                return NotFound("Repartidor no encontrado con el usuario especificado.");
            }

            // Actualizar los campos del repartidor existente con la nueva información
            repartidorExistente.NombreCompleto = repartidor.NombreCompleto;
            repartidorExistente.DireccionProvincia = repartidor.DireccionProvincia;
            repartidorExistente.DireccionCanton = repartidor.DireccionCanton;
            repartidorExistente.DireccionDistrito = repartidor.DireccionDistrito;
            repartidorExistente.CorreoElectronico = repartidor.CorreoElectronico;
            repartidorExistente.Usuario = repartidor.Usuario;

            // Actualizar la contraseña si se proporciona una nueva (hasheada en Base64)
            if (!string.IsNullOrEmpty(repartidor.PasswordHash))
            {
                repartidorExistente.PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(repartidor.PasswordHash));
            }

            // Actualizar la lista de teléfonos
            if (repartidor.Telefonos != null && repartidor.Telefonos.Any())
            {
                // Eliminar los teléfonos actuales
                _context.TelefonosRepartidores.RemoveRange(repartidorExistente.Telefonos);

                // Agregar los nuevos teléfonos
                foreach (var telefono in repartidor.Telefonos)
                {
                    telefono.RepartidorID = repartidorExistente.RepartidorID; // Asignar el RepartidorID
                    _context.TelefonosRepartidores.Add(telefono);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Repartidores.Any(e => e.Usuario == usuario))
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
        public async Task<IActionResult> DeleteRepartidor(int id)
        {
            var repartidor = await _context.Repartidores
                .Include(r => r.Telefonos) // Incluir los teléfonos asociados
                .FirstOrDefaultAsync(r => r.RepartidorID == id);

            if (repartidor == null)
            {
                return NotFound();
            }

            // Eliminar los teléfonos asociados antes de eliminar el repartidor
            _context.TelefonosRepartidores.RemoveRange(repartidor.Telefonos);
            _context.Repartidores.Remove(repartidor);
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

        // Método para enviar el correo electrónico con la contraseña
        private void EnviarCorreoContrasena(string correo, string password)
        {
            var fromAddress = new MailAddress("Yitanr@gmail.com", "Sistema UbyTec");
            var toAddress = new MailAddress(correo);
            const string fromPassword = "gtsk jhyg ooal tiox";
            const string subject = "Registro Exitoso - Tu Contraseña";
            string body = $"Gracias por registrarse en nuestro sistema como repartidor. Su contraseña es: {password}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
        [HttpDelete("eliminarPorUsuario/{usuario}")]
        public async Task<IActionResult> DeleteRepartidorPorUsuario(string usuario)
        {
            Console.WriteLine($"Intentando eliminar repartidor con usuario: {usuario}");

            // Busca el repartidor incluyendo los teléfonos asociados
            var repartidor = await _context.Repartidores
                .Include(r => r.Telefonos)
                .FirstOrDefaultAsync(r => r.Usuario == usuario);

            if (repartidor == null)
            {
                Console.WriteLine("Repartidor no encontrado.");
                return NotFound("Repartidor no encontrado con el usuario especificado.");
            }

            try
            {
                // Eliminar los teléfonos asociados
                if (repartidor.Telefonos != null && repartidor.Telefonos.Any())
                {
                    _context.TelefonosRepartidores.RemoveRange(repartidor.Telefonos);
                }

                // Eliminar el repartidor
                _context.Repartidores.Remove(repartidor);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                Console.WriteLine("Repartidor y datos relacionados eliminados exitosamente.");
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el repartidor: {ex.Message}");
                return StatusCode(500, "Error interno al eliminar el repartidor.");
            }
        }
    }
}
