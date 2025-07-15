using System.Collections.Generic;

namespace UbyTecAPI.Models
{
    public class Administrador
    {
        public int AdministradorID { get; set; }
        public string NumeroCedula { get; set; }
        public string NombreCompleto { get; set; }
        public string DireccionProvincia { get; set; }
        public string DireccionCanton { get; set; }
        public string DireccionDistrito { get; set; }
        public string Usuario { get; set; }
        public string PasswordHash { get; set; }

        // Relación de uno a muchos para los teléfonos
        public ICollection<TelefonoAdministrador> Telefonos { get; set; } = new List<TelefonoAdministrador>();
    }

    public class TelefonoAdministrador
    {
        public int TelefonoAdministradorID { get; set; }
        public string Numero { get; set; }
        public int AdministradorID { get; set; }  // Relación con el ID del administrador
    }
}
