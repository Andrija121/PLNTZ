using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
    internal class RabbitMQConsumer
    {
        private readonly IModel _channel;

        public RabbitMQConsumer(IModel channel)
        {
            _channel = channel;
        }

        public void Receive(string queueName, Action<string> messageHandler)
        {
            // Implementation for receiving messages
        }
    }
}
