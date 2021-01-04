using HelloWorldMensageria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace HelloWorldMensageria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        private readonly ILogger<HelloWorldController> _logger;

        public HelloWorldController(ILogger<HelloWorldController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetHelloWorld()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "helloworld",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var messageToBeReceived = "";
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var helloWorld = JsonSerializer.Deserialize<HelloWorld>(message);
                    messageToBeReceived = $"Mensagem:{helloWorld.Message} | Id da requisição:{helloWorld.RequestId} | Id do serviço:{helloWorld.ServiceId} | Timestamp:{helloWorld.Timestamp}";
                    _logger.LogInformation(messageToBeReceived);
                };
                channel.BasicConsume(queue: "helloworld",
                                     autoAck: true,
                                     consumer: consumer);

                return new ObjectResult(new { mensagem = messageToBeReceived});
            }
        }
    }
}