using System.ComponentModel.DataAnnotations;

namespace UbyTecAPI.Models
{
    public class EstadoRepartidor
    {
        [Key]  // Define explícitamente que esta es la clave primaria
        public int EstadoRepartidorID { get; set; }

        public int RepartidorID { get; set; }
        public bool Disponible { get; set; }

        public Repartidor Repartidor { get; set; }
    }
}
