namespace UbyTecAPI.Models
{
    public class Pedido
    {
        public int PedidoID { get; set; }  // Clave primaria

        // Identificadores
        public int ClienteID { get; set; }
        public int AfiliadoID { get; set; }
        public int? RepartidorID { get; set; }

        // Información del pedido
        public int CantidadProducto { get; set; } // Cantidad total de productos
        public string NombreProducto { get; set; } // Nombre del producto representativo
        public decimal MontoProducto { get; set; } // Precio unitario del producto
        public decimal Total { get; set; } // Total del pedido

        // Propiedades adicionales para mostrar nombres
        public string NombreCliente { get; set; } // Nombre del cliente
        public string NombreComercio { get; set; } // Nombre del comercio
        public string NombreRepartidor { get; set; } // Nombre del repartidor (puede ser null)
    }
}
