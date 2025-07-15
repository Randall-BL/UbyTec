using FeedbackApi.Models;
using FeedbackApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace FeedbackApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly MongoDbContext _dbContext;

        public FeedbackController(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Obtener todos los feedbacks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var feedbacks = await _dbContext.FeedbackCollection.Find(_ => true).ToListAsync();
            return Ok(feedbacks);
        }

        // Obtener un feedback por Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var feedback = await _dbContext.FeedbackCollection.Find(f => f.Id == id).FirstOrDefaultAsync();
            return feedback == null ? NotFound($"No se encontró feedback con el ID {id}") : Ok(feedback);
        }

        // Crear un nuevo feedback
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Feedback feedback)
        {
            // Validar que los campos requeridos estén presentes
            if (feedback.PedidoID == null || feedback.ClienteID == null || feedback.Total == null)
            {
                return BadRequest("Los campos 'PedidoID', 'ClienteID' y 'Total' son obligatorios.");
            }

            // Generar el próximo Id autoincrementable
            feedback.Id = _dbContext.GetNextSequenceValue("Feedback");

            // Insertar el feedback en la colección
            await _dbContext.FeedbackCollection.InsertOneAsync(feedback);

            return CreatedAtAction(nameof(GetById), new { id = feedback.Id }, feedback);
        }

        // Actualizar un feedback existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Feedback updatedFeedback)
        {
            // Asegurarse de que el Id del documento sea el correcto
            updatedFeedback.Id = id;

            // Reemplazar el documento existente
            var result = await _dbContext.FeedbackCollection.ReplaceOneAsync(f => f.Id == id, updatedFeedback);

            return result.ModifiedCount > 0 ? NoContent() : NotFound($"No se encontró feedback con el ID {id} para actualizar.");
        }

        // Eliminar un feedback
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _dbContext.FeedbackCollection.DeleteOneAsync(f => f.Id == id);

            return result.DeletedCount > 0 ? NoContent() : NotFound($"No se encontró feedback con el ID {id} para eliminar.");
        }


        // Obtener feedback por PedidoID
        [HttpGet("pedido/{pedidoId}")]
        public async Task<IActionResult> GetByPedidoId(int pedidoId)
        {
            var feedback = await _dbContext.FeedbackCollection.Find(f => f.PedidoID == pedidoId).ToListAsync();
            return feedback.Count == 0 ? NotFound($"No se encontraron feedbacks para el PedidoID {pedidoId}") : Ok(feedback);
        }

    }
}
