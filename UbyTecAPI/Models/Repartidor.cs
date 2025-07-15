using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UbyTecAPI.Models
{
    public class Repartidor
    {
        public int RepartidorID { get; set; }
        public string NombreCompleto { get; set; }
        public string DireccionProvincia { get; set; }
        public string DireccionCanton { get; set; }
        public string DireccionDistrito { get; set; }
        public string CorreoElectronico { get; set; }
        public string Usuario { get; set; }
        public string? PasswordHash { get; set; }

        public ICollection<TelefonoRepartidor> Telefonos { get; set; } = new List<TelefonoRepartidor>();
    }

    public class TelefonoRepartidor
{
    public int TelefonoRepartidorID { get; set; } // Debería ser autogenerado
    public string Numero { get; set; }
    public int RepartidorID { get; set; }  // Relación con el ID del repartidor (sin propiedad de navegación)
}

}
