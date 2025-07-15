using FeedbackApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FeedbackApi.Services
{
    /// <summary>
    /// Contexto para interactuar con la base de datos MongoDB.
    /// </summary>
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Constructor del contexto que inicializa la conexión a MongoDB.
        /// </summary>
        /// <param name="configuration">Configuración de la aplicación (IConfiguration).</param>
        public MongoDbContext(IConfiguration configuration)
        {
            // Leer cadena de conexión y nombre de la base de datos desde la configuración
            var connectionString = configuration.GetValue<string>("MongoDB:ConnectionString");
            var databaseName = configuration.GetValue<string>("MongoDB:DatabaseName");

            // Crear el cliente y seleccionar la base de datos
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        /// <summary>
        /// Colección para manejar los documentos de Feedback.
        /// </summary>
        public IMongoCollection<Feedback> FeedbackCollection =>
            _database.GetCollection<Feedback>("Feedback");

        /// <summary>
        /// Colección para manejar los contadores autoincrementables.
        /// </summary>
        public IMongoCollection<BsonDocument> CountersCollection =>
            _database.GetCollection<BsonDocument>("Counters");

        /// <summary>
        /// Obtiene el próximo valor autoincrementable para una colección específica.
        /// </summary>
        /// <param name="collectionName">El nombre de la colección (por ejemplo, "Feedback").</param>
        /// <returns>El siguiente valor de secuencia como un entero.</returns>
        public int GetNextSequenceValue(string collectionName)
        {
            // Crear el filtro para buscar el contador basado en el nombre de la colección
            var filter = Builders<BsonDocument>.Filter.Eq("_id", collectionName);

            // Incrementar el valor de secuencia en 1
            var update = Builders<BsonDocument>.Update.Inc("sequence_value", 1);

            // Opciones para crear el documento si no existe y retornar el nuevo valor
            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                ReturnDocument = ReturnDocument.After, // Retornar el documento actualizado
                IsUpsert = true // Crear el documento si no existe
            };

            // Ejecutar la operación de actualización en la colección Counters
            var counter = CountersCollection.FindOneAndUpdate(filter, update, options);

            // Retornar el nuevo valor de secuencia
            return counter["sequence_value"].AsInt32;
        }
    }
}
