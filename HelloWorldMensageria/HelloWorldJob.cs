using HelloWorldMensageria.Models;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorldMensageria
{
    public static class HelloWorldJob
    {
        public static void SendHelloWorldsEvery5Seconds(this Startup startup)
        {
            var helloWorld = new HelloWorld();
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "helloworld",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = JsonSerializer.Serialize(helloWorld);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "helloworld",
                                     basicProperties: null,
                                     body: body);
            }
            Thread.Sleep(Convert.ToInt32(TimeSpan.FromSeconds(5).TotalMilliseconds));
            Task.Run(() => startup.SendHelloWorldsEvery5Seconds());
        }
    }
}
