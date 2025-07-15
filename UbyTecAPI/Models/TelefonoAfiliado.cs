using System.Text.Json.Serialization;

namespace UbyTecAPI.Models
{
    public class TelefonoAfiliado
    {
        public int TelefonoAfiliadoID { get; set; }
        public int? AfiliadoID { get; set; }
        public string NumeroTelefono { get; set; }

        [JsonIgnore]
        public Afiliado? Afiliado { get; set; }  // Ignora la propiedad para evitar ciclos
    }
}
