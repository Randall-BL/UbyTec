using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UbyTecAPI.Models
{
    public class DetallePedido
{
    [Key]
    public int DetalleID { get; set; }

    [Required]
    public int AfiliadoID { get; set; }

    [Required]
    public int ClienteID { get; set; }

    public int? RepartidorID { get; set; } // Ahora permite valores NULL

    [Required]
    [StringLength(50)]
    public string Estado { get; set; } // Estado del pedido ("Recibido", "En proceso", "Completado")

    public ICollection<ProductoDetalle> Productos { get; set; } = new List<ProductoDetalle>();

    [System.Text.Json.Serialization.JsonIgnore] // Evitar ciclos en la serialización
    public Afiliado? Afiliado { get; set; }

    [System.Text.Json.Serialization.JsonIgnore] // Evitar ciclos en la serialización
    public Cliente? Cliente { get; set; }

    [System.Text.Json.Serialization.JsonIgnore] // Evitar ciclos en la serialización
    public Repartidor? Repartidor { get; set; }
}



    public class ProductoDetalle
    {
        [Key]
        public int ProductoDetalleID { get; set; }

        [Required]
        public int ProductoID { get; set; }

        [Required]
        public int DetallePedidoID { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        public decimal Precio { get; set; }

        [System.Text.Json.Serialization.JsonIgnore] // Evitar ciclos en la serialización
        public Producto? Producto { get; set; }

        [System.Text.Json.Serialization.JsonIgnore] // Evitar ciclos en la serialización
        public DetallePedido? DetallePedido { get; set; }
    }

}
