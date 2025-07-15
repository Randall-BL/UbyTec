using System.Collections.Generic;

namespace UbyTecAPI.Models
{
    public class TipoComercio
    {
        public int TipoComercioID { get; set; }
        public string NombreTipo { get; set; }
        public ICollection<Afiliado>? Afiliados { get; set; } // Relación con Afiliados, permitiendo null
    }
}
