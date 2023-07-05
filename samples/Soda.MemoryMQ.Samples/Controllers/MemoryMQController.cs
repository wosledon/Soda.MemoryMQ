using Microsoft.AspNetCore.Mvc;
using Soda.MemoryMQ.Interfacure;

namespace Soda.MemoryMQ.Samples.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    public class MemoryMQController : ControllerBase
    {
        private readonly IMessageProducer<string> _producer;

        public MemoryMQController(IMessageProducer<string> producer)
        {
            _producer = producer;
        }

        [HttpGet]
        public async Task<IActionResult> Producer(string channel, string value)
        {
            await _producer.ProduceAsync(channel, value);

            return Ok();
        }
    }
}