using MongoDB.Bson.Serialization.Attributes;

namespace FeedbackApi.Models
{
    public class Feedback
    {
        [BsonId]
        public int Id { get; set; } // Id autoincrementable

        [BsonElement("pedidoID")]
        public int? PedidoID { get; set; } // Puede ser nulo

        [BsonElement("clienteID")]
        public int? ClienteID { get; set; } // Puede ser nulo

        [BsonElement("afiliadoID")]
        public int? AfiliadoID { get; set; } // Puede ser nulo

        [BsonElement("repartidorID")]
        public int? RepartidorID { get; set; } // Puede ser nulo

        [BsonElement("cantidadProducto")]
        public int? CantidadProducto { get; set; } // Puede ser nulo

        [BsonElement("nombreProducto")]
        public string? NombreProducto { get; set; } // Puede ser nulo

        [BsonElement("montoProducto")]
        public double? MontoProducto { get; set; } // Puede ser nulo

        [BsonElement("total")]
        public double? Total { get; set; } // Puede ser nulo

        [BsonElement("nombreCliente")]
        public string? NombreCliente { get; set; } // Puede ser nulo

        [BsonElement("nombreComercio")]
        public string? NombreComercio { get; set; } // Puede ser nulo

        [BsonElement("nombreRepartidor")]
        public string? NombreRepartidor { get; set; } // Puede ser nulo

        [BsonElement("comentario")]
        public string? Comentario { get; set; } // Puede ser nulo
    }
}
