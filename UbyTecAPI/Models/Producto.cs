using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UbyTecAPI.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }  // Clave primaria

        public int? AfiliadoID { get; set; } // Relación con Afiliado, permitido como nulo

        public string? NombreProducto { get; set; } // Nombre del producto, permitido como nulo
        public string? Categoria { get; set; } // Categoría del producto, permitido como nulo
        public string? Foto { get; set; } // Nombre del archivo de la foto, permitido como nulo
        public decimal? Precio { get; set; } // Precio del producto, permitido como nulo

        // Relación con Afiliado
         [JsonIgnore]
        public Afiliado? Afiliado { get; set; } // Afiliado relacionado, permitido como nulo
    }
}
