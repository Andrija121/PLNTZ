using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
    internal class RabbitMQProducer
    {
        private readonly IModel _channel;

        public RabbitMQProducer(IModel channel)
        {
            _channel = channel;
        }

        public void Send(string queueName, string message)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Convert the message to bytes
            var body = Encoding.UTF8.GetBytes(message);

            // Publish the message to the queue
            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

            Console.WriteLine($" [x] Sent '{message}' to '{queueName}'");
        }
    }
}
