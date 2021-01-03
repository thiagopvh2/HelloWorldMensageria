using System;
using System.Diagnostics;

namespace HelloWorldMensageria.Models
{
    public class HelloWorld
    {
        public HelloWorld()
        {
            Message = "Hello World!";
            ServiceId = Process.GetCurrentProcess().Id;
            RequestId = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }
        public string Message { get; private set; }
        public int ServiceId { get; private set; }
        public Guid RequestId { get; private set; }
        public DateTime Timestamp { get; private set; }
    }
}