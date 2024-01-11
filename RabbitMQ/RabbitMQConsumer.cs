using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public class RabbitMQConsumer(IModel channel)
    {
        private readonly IModel _channel = channel;

        public void Receive(string queueName, Action<string> messageHandler)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Set up a consumer to handle incoming messages
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Process the received message
                messageHandler(message);

                Console.WriteLine($" [x] Received '{message}' from '{queueName}'");
            };

            // Start consuming messages
            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}
