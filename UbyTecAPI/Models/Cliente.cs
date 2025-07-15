namespace UbyTecAPI.Models
{
    public class Cliente
    {
        public int ClienteID { get; set; }
        public string NumeroCedula { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string DireccionProvincia { get; set; }
        public string DireccionCanton { get; set; }
        public string DireccionDistrito { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Usuario { get; set; }
        public byte[] PasswordHash { get; set; }
    }


    public class UpdateClienteDto
    {
        public string NumeroCedula { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string DireccionProvincia { get; set; }
        public string DireccionCanton { get; set; }
        public string DireccionDistrito { get; set; }
    }
}
