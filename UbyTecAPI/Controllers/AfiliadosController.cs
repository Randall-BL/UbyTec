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
    public class AfiliadosController : ControllerBase
    {
        private readonly UbyTecContext _context;

        public AfiliadosController(UbyTecContext context)
        {
            _context = context;
        }

        // Obtener todos los afiliados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Afiliado>>> GetAfiliados()
        {
            return await _context.Afiliados
                .Include(a => a.Telefonos) // Incluir los teléfonos asociados
                .Include(a => a.Productos) // Incluir los productos asociados
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Afiliado>> GetAfiliado(int id)
        {
            var afiliado = await _context.Afiliados
                .Include(a => a.Telefonos) // Incluir los teléfonos asociados
                .Include(a => a.Productos) // Incluir los productos asociados
                .FirstOrDefaultAsync(a => a.AfiliadoID == id);

            if (afiliado == null)
            {
                return NotFound();
            }

            return afiliado;
        }

        // Agregar un nuevo afiliado
        [HttpPost]
        public async Task<ActionResult<Afiliado>> PostAfiliado(Afiliado afiliado)
        {
            
            // Validar si la cédula ya está registrada
            var afiliadoExistenteCedula = await _context.Afiliados
                .FirstOrDefaultAsync(a => a.NumeroCedulaJuridica == afiliado.NumeroCedulaJuridica);

            if (afiliadoExistenteCedula != null)
            {
                return Conflict("Ya existe un afiliado con esta cédula jurídica.");
            }
            
            // Generar una contraseña aleatoria si no se proporciona una
            if (string.IsNullOrEmpty(afiliado.Password))
            {
                afiliado.Password = GenerateRandomPassword();
            }

            // Enviar el correo electrónico al afiliado con la contraseña sin encriptar
            EnviarCorreoContrasena(afiliado.CorreoElectronico, afiliado.Password);

            // Convertir la contraseña en base64 antes de guardarla en la base de datos
            afiliado.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(afiliado.Password));

            // Agregar el afiliado a la base de datos
            _context.Afiliados.Add(afiliado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAfiliado), new { id = afiliado.AfiliadoID }, afiliado);
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
            string body = $"Gracias por registrarse en nuestro sistema. Su contraseña es: {password}";

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

        // Iniciar sesión con el correo y contraseña
        [HttpGet("iniciarSesion")]
        public async Task<ActionResult<Afiliado>> IniciarSesionAfiliado(string correoElectronico, string password)
        {
            var afiliado = await _context.Afiliados
                .FirstOrDefaultAsync(a => a.CorreoElectronico == correoElectronico);

            if (afiliado == null)
            {
                return NotFound("Correo electrónico no registrado");
            }

            // Convertir la contraseña ingresada en Base64
            string hashedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

            if (afiliado.Password != hashedPassword)
            {
                return Unauthorized("Contraseña incorrecta");
            }

            return Ok(afiliado);
        }

        // Actualizar un afiliado existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAfiliado(int id, Afiliado afiliado)
        {
            if (id != afiliado.AfiliadoID)
            {
                return BadRequest();
            }

            _context.Entry(afiliado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Afiliados.Any(e => e.AfiliadoID == id))
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

        // Eliminar un afiliado
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAfiliado(int id)
        {
            var afiliado = await _context.Afiliados
                .Include(a => a.Telefonos) // Incluir los teléfonos asociados
                .FirstOrDefaultAsync(a => a.AfiliadoID == id);

            if (afiliado == null)
            {
                return NotFound();
            }

            try
            {
                // Desactivar el trigger antes de la eliminación
                await _context.Database.ExecuteSqlRawAsync("DISABLE TRIGGER trg_TelefonoDelete ON TelefonosAfiliados");

                // Eliminar los teléfonos asociados antes de eliminar el afiliado
                _context.TelefonosAfiliados.RemoveRange(afiliado.Telefonos);
                _context.Afiliados.Remove(afiliado);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el afiliado: {ex.Message}");
            }
            finally
            {
                // Volver a habilitar el trigger después de la operación
                await _context.Database.ExecuteSqlRawAsync("ENABLE TRIGGER trg_TelefonoDelete ON TelefonosAfiliados");
            }

            return NoContent();
        }


        // Obtener los teléfonos de un afiliado específico
        [HttpGet("{id}/telefonos")]
        public async Task<ActionResult<IEnumerable<TelefonoAfiliado>>> GetTelefonosAfiliado(int id)
        {
            var afiliado = await _context.Afiliados
                .Include(a => a.Telefonos)
                .FirstOrDefaultAsync(a => a.AfiliadoID == id);

            if (afiliado == null)
            {
                return NotFound();
            }

            return Ok(afiliado.Telefonos);
        }

        // Agregar un teléfono a un afiliado específico
        [HttpPost("{id}/telefonos")]
        public async Task<IActionResult> AddTelefonoToAfiliado(int id, TelefonoAfiliado telefono)
        {
            var afiliado = await _context.Afiliados.FindAsync(id);

            if (afiliado == null)
            {
                return NotFound("Afiliado no encontrado");
            }

            telefono.AfiliadoID = id;
            _context.TelefonosAfiliados.Add(telefono);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAfiliado), new { id = afiliado.AfiliadoID }, telefono);
        }

        // Eliminar un teléfono de un afiliado
        [HttpDelete("telefonos/{telefonoId}")]
        public async Task<IActionResult> DeleteTelefono(int telefonoId)
        {
            var telefono = await _context.TelefonosAfiliados.FindAsync(telefonoId);
            if (telefono == null)
            {
                return NotFound("Teléfono no encontrado");
            }

            _context.TelefonosAfiliados.Remove(telefono);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // Obtener un afiliado por cédula jurídica
        [HttpGet("buscarPorCedulaJuridica/{numeroCedulaJuridica}")]
        public async Task<ActionResult<Afiliado>> GetAfiliadoByCedulaJuridica(string numeroCedulaJuridica)
        {
            var afiliado = await _context.Afiliados
                .Include(a => a.Telefonos) // Incluir los teléfonos asociados
                .FirstOrDefaultAsync(a => a.NumeroCedulaJuridica == numeroCedulaJuridica);

            if (afiliado == null)
            {
                return NotFound("Afiliado no encontrado con el número de cédula jurídica especificado.");
            }

            return Ok(afiliado);
        }

        // Modificar un afiliado por cédula jurídica
        [HttpPut("modificarPorCedulaJuridica/{numeroCedulaJuridica}")]
        public async Task<IActionResult> PutAfiliadoByCedulaJuridica(string numeroCedulaJuridica, Afiliado afiliado)
        {
            var afiliadoExistente = await _context.Afiliados
                .Include(a => a.Telefonos)
                .FirstOrDefaultAsync(a => a.NumeroCedulaJuridica == numeroCedulaJuridica);

            if (afiliadoExistente == null)
            {
                return NotFound("Afiliado no encontrado con el número de cédula jurídica especificado.");
            }

            // Actualizar los campos del afiliado existente con la nueva información
            afiliadoExistente.NombreComercio = afiliado.NombreComercio;
            afiliadoExistente.TipoComercio = afiliado.TipoComercio;
            afiliadoExistente.DireccionProvincia = afiliado.DireccionProvincia;
            afiliadoExistente.DireccionCanton = afiliado.DireccionCanton;
            afiliadoExistente.DireccionDistrito = afiliado.DireccionDistrito;
            afiliadoExistente.CorreoElectronico = afiliado.CorreoElectronico;
            afiliadoExistente.NumeroSINPE = afiliado.NumeroSINPE;
            afiliadoExistente.Administrador = afiliado.Administrador;
            afiliadoExistente.Estado = afiliado.Estado;

            // Actualizar la contraseña si se proporciona una nueva (hasheada)
            if (!string.IsNullOrEmpty(afiliado.Password))
            {
                afiliadoExistente.Password = afiliado.Password;
            }

            // Actualizar la lista de teléfonos
            if (afiliado.Telefonos != null && afiliado.Telefonos.Any())
            {
                // Eliminar los teléfonos actuales
                _context.TelefonosAfiliados.RemoveRange(afiliadoExistente.Telefonos);

                // Agregar los nuevos teléfonos
                foreach (var telefono in afiliado.Telefonos)
                {
                    telefono.AfiliadoID = afiliadoExistente.AfiliadoID; // Asignar el AfiliadoID
                    _context.TelefonosAfiliados.Add(telefono);
                }
            }

            // Guardar los cambios en la base de datos
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Afiliados.Any(e => e.NumeroCedulaJuridica == numeroCedulaJuridica))
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

        // Nuevo método para obtener un afiliado por nombre de comercio
        [HttpGet("buscarPorNombreComercio/{nombreComercio}")]
        public async Task<ActionResult<Afiliado>> GetAfiliadoPorNombreComercio(string nombreComercio)
        {
            var afiliado = await _context.Afiliados
                .Include(a => a.Telefonos) // Incluir los teléfonos asociados
                .FirstOrDefaultAsync(a => a.NombreComercio == nombreComercio);

            if (afiliado == null)
            {
                return NotFound("Afiliado no encontrado con el nombre de comercio especificado.");
            }

            return Ok(afiliado);
        }

        [HttpGet("productosPorUbicacion")]
        public async Task<ActionResult<IEnumerable<object>>> GetAfiliadosConProductosPorUbicacion(
            string provincia, string canton, string distrito)
        {
            var afiliados = await _context.Afiliados
                .Where(a => a.Productos.Any() &&
                            a.DireccionProvincia.ToLower() == provincia.ToLower() &&
                            a.DireccionCanton.ToLower() == canton.ToLower() &&
                            a.DireccionDistrito.ToLower() == distrito.ToLower())
                .Select(a => new
                {
                    AfiliadoID = a.AfiliadoID,
                    NombreComercio = a.NombreComercio,
                    DireccionProvincia = a.DireccionProvincia,
                    DireccionCanton = a.DireccionCanton,
                    DireccionDistrito = a.DireccionDistrito,
                    Telefono1 = a.Telefonos.Select(t => t.NumeroTelefono).FirstOrDefault()
                }).ToListAsync();

            Console.WriteLine("Afiliados encontrados: ", afiliados.Count);
            foreach (var afiliado in afiliados)
            {
                Console.WriteLine($"NombreComercio: {afiliado.NombreComercio}, Provincia: {afiliado.DireccionProvincia}, Telefono1: {afiliado.Telefono1}");
            }
            return Ok(afiliados);
        }

       [HttpGet("{afiliadoID}/productos")]
        public async Task<ActionResult<object>> GetProductosPorAfiliado(int afiliadoID)
        {
            var afiliado = await _context.Afiliados
                .Include(a => a.Productos)
                .FirstOrDefaultAsync(a => a.AfiliadoID == afiliadoID);

            if (afiliado == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                nombreComercio = afiliado.NombreComercio,
                productos = afiliado.Productos.Select(p => new
                {
                    p.ProductoID,
                    p.NombreProducto,
                    p.Categoria,
                    p.Precio
                })
            });
        }

        [HttpGet("estado/{estado}")]
        public async Task<ActionResult<IEnumerable<object>>> GetAfiliadosPorEstado(string estado)
        {
            var afiliados = await _context.Afiliados
                .Where(a => a.Estado == estado) // Filtrar afiliados por el estado recibido
                .Select(a => new 
                {
                    NombreComercio = a.NombreComercio,
                    NumeroCedulaJuridica = a.NumeroCedulaJuridica,
                    TipoComercio = a.TipoComercio,
                    DireccionProvincia = a.DireccionProvincia,
                    DireccionCanton = a.DireccionCanton,
                    DireccionDistrito = a.DireccionDistrito,
                    CorreoElectronico = a.CorreoElectronico,
                    NumeroSINPE = a.NumeroSINPE,
                    Estado = a.Estado,
                    Telefonos = a.Telefonos.Select(t => new 
                    {
                        TelefonoAfiliadoID = t.TelefonoAfiliadoID,
                        NumeroTelefono = t.NumeroTelefono
                    }).ToList(),
                    Productos = a.Productos.Select(p => new 
                    {
                        ProductoID = p.ProductoID,
                        NombreProducto = p.NombreProducto,
                        Categoria = p.Categoria,
                        Foto = p.Foto,
                        Precio = p.Precio
                    }).ToList()
                }).ToListAsync();

            return Ok(afiliados);
        }


        // PUT: api/afiliados/aceptar/{numeroCedulaJuridica}
        // Método para aceptar un afiliado (cambia el estado a "aceptado")
        [HttpPut("aceptar/{numeroCedulaJuridica}")]
        public async Task<IActionResult> AceptarAfiliado(string numeroCedulaJuridica)
        {
            var afiliado = await _context.Afiliados
                .FirstOrDefaultAsync(a => a.NumeroCedulaJuridica == numeroCedulaJuridica);

            if (afiliado == null)
            {
                return NotFound("Afiliado no encontrado con el número de cédula jurídica especificado.");
            }

            try
            {
                afiliado.Estado = "aceptado";
                _context.Afiliados.Update(afiliado);
                await _context.SaveChangesAsync();

                return Ok("Afiliado aceptado con éxito.");
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error al aceptar el afiliado: {dbEx.Message}");
                return StatusCode(500, $"Database update error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aceptar el afiliado: {ex.Message}");
                return StatusCode(500, $"Error interno al aceptar el afiliado: {ex.Message}");
            }
        }

        // PUT: api/afiliados/rechazar/{numeroCedulaJuridica}
        // Método para rechazar un afiliado (cambia el estado con una razón del rechazo)
        [HttpPut("rechazar/{numeroCedulaJuridica}")]
        public async Task<IActionResult> RechazarAfiliado(string numeroCedulaJuridica, [FromBody] string comentario)
        {
            var afiliado = await _context.Afiliados
                .FirstOrDefaultAsync(a => a.NumeroCedulaJuridica == numeroCedulaJuridica);

            if (afiliado == null)
            {
                return NotFound("Afiliado no encontrado con el número de cédula jurídica especificado.");
            }

            if (string.IsNullOrWhiteSpace(comentario))
            {
                return BadRequest("Se requiere un comentario para rechazar al afiliado.");
            }

            try
            {
                afiliado.Estado = $"Rechazado: {comentario}";
                _context.Afiliados.Update(afiliado);
                await _context.SaveChangesAsync();

                return Ok("Afiliado rechazado con éxito.");
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error al rechazar el afiliado: {dbEx.Message}");
                return StatusCode(500, $"Database update error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al rechazar el afiliado: {ex.Message}");
                return StatusCode(500, $"Error interno al rechazar el afiliado: {ex.Message}");
            }
        }


        // GET: api/afiliados/estadoVacio
        [HttpGet("estadoVacio")]
        public async Task<ActionResult<IEnumerable<object>>> ObtenerAfiliadosConEstadoVacio()
        {
            // Consultar afiliados con estado vacío y seleccionar los campos específicos
            var afiliadosConEstadoVacio = await _context.Afiliados
                .Where(a => a.Estado == "")
                .Select(a => new
                {
                    a.NumeroCedulaJuridica,
                    a.NombreComercio,
                    a.TipoComercio,
                    a.CorreoElectronico
                })
                .ToListAsync();

            if (afiliadosConEstadoVacio == null || afiliadosConEstadoVacio.Count == 0)
            {
                return NotFound("No se encontraron afiliados con el estado vacío.");
            }

            return Ok(afiliadosConEstadoVacio);
        }

        // Método para enviar correo con comentario de rechazo
        private void EnviarCorreoRechazo(string correo, string comentario)
        {
            var fromAddress = new MailAddress("Yitanr@gmail.com", "Sistema UbyTec");
            var toAddress = new MailAddress(correo);
            const string fromPassword = "gtsk jhyg ooal tiox";
            const string subject = "Afiliación Rechazada";
            string body = $"Lamentamos informarle que su solicitud de afiliación ha sido rechazada. Motivo: {comentario}";

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
        [HttpDelete("eliminarPorCedulaJuridica/{numeroCedulaJuridica}")]
    public async Task<IActionResult> DeleteAfiliadoByCedulaJuridica(string numeroCedulaJuridica)
{
    try
    {
        // Desactivar el trigger de TelefonosAfiliados antes de realizar cambios
        await _context.Database.ExecuteSqlRawAsync("DISABLE TRIGGER trg_TelefonoDelete ON TelefonosAfiliados");

        // Buscar el afiliado por su número de cédula jurídica
        var afiliado = await _context.Afiliados
            .Include(a => a.Telefonos)
            .Include(a => a.Productos)
            .FirstOrDefaultAsync(a => a.NumeroCedulaJuridica == numeroCedulaJuridica);

        if (afiliado == null)
        {
            // Reactivar el trigger antes de salir
            await _context.Database.ExecuteSqlRawAsync("ENABLE TRIGGER trg_TelefonoDelete ON TelefonosAfiliados");
            return NotFound("Afiliado no encontrado con el número de cédula jurídica especificado.");
        }

        // Eliminar los detalles de pedidos relacionados con el afiliado usando SQL crudo
        await _context.Database.ExecuteSqlRawAsync(
            "DELETE FROM DetallesPedido WHERE AfiliadoID = {0}",
            afiliado.AfiliadoID
        );

        // Eliminar los detalles de productos relacionados en ProductoDetalle usando SQL crudo
        foreach (var producto in afiliado.Productos)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM ProductosDetalle WHERE ProductoID = {0}",
                producto.ProductoID
            );
        }

        // Eliminar los productos asociados
        if (afiliado.Productos?.Any() == true)
        {
            _context.Productos.RemoveRange(afiliado.Productos);
        }

        // Eliminar los teléfonos asociados
        if (afiliado.Telefonos?.Any() == true)
        {
            _context.TelefonosAfiliados.RemoveRange(afiliado.Telefonos);
        }

        // Eliminar el afiliado
        _context.Afiliados.Remove(afiliado);

        // Guardar cambios en la base de datos
        await _context.SaveChangesAsync();

        return NoContent();
    }
    catch (DbUpdateException dbEx)
    {
        // Manejar errores específicos de la base de datos
        Console.WriteLine($"Error al actualizar la base de datos: {dbEx.InnerException?.Message ?? dbEx.Message}");
        return StatusCode(500, $"Error al actualizar la base de datos: {dbEx.InnerException?.Message ?? dbEx.Message}");
    }
    catch (Exception ex)
    {
        // Manejar errores generales
        Console.WriteLine($"Error interno: {ex.InnerException?.Message ?? ex.Message}");
        return StatusCode(500, $"Error interno: {ex.InnerException?.Message ?? ex.Message}");
    }
    finally
    {
        // Asegurarse de reactivar el trigger incluso si ocurre un error
        try
        {
            await _context.Database.ExecuteSqlRawAsync("ENABLE TRIGGER trg_TelefonoDelete ON TelefonosAfiliados");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al reactivar el trigger: {ex.Message}");
        }
    }
}

    }
}