using System;

namespace HelloWorldMensageria.Models
{
    public class HelloWorld
    {
        public HelloWorld(string message, Guid serviceId)
        {
            Message = message;
            ServiceId = serviceId;
            RequestId = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }

        public HelloWorld() { }
        public string Message { get; private set; }
        public Guid ServiceId { get; private set; }
        public Guid RequestId { get; private set; }
        public DateTime Timestamp { get; private set; }
    }
}