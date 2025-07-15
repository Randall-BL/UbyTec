using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UbyTecAPI.Models
{
    public class Afiliado
    {
        public int AfiliadoID { get; set; }
        public string NumeroCedulaJuridica { get; set; }
        public string NombreComercio { get; set; }
        public string TipoComercio { get; set; }
        public string DireccionProvincia { get; set; }
        public string DireccionCanton { get; set; }
        public string DireccionDistrito { get; set; }
        public string CorreoElectronico { get; set; }
        public string NumeroSINPE { get; set; }
        public string Administrador { get; set; }

        public string? Password { get; set; }

        public string Estado { get; set; }

        // Nueva relación para tener múltiples teléfonos
        public ICollection<TelefonoAfiliado> Telefonos { get; set; } = new List<TelefonoAfiliado>();
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
