using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
    internal class RabbitMQSerivce
    {
        private readonly RabbitMQConfiguration _configuration;
        private readonly RabbitMQProducer _producer;
        private readonly RabbitMQConsumer _consumer;

        public RabbitMQSerivce(RabbitMQConfiguration configuration, RabbitMQProducer producer, RabbitMQConsumer consumer)        {
            _configuration = configuration;
            _producer = producer;
            _consumer = consumer;

            InitializeConnection();
        }
        private void InitializeConnection()
        {
            // Implementation for initializing RabbitMQ connection
        }

        public void Send(string queueName, string message)
        {
            _producer.Send(queueName, message);
        }
        public void Receive(string queueName, Action<string> messageHandler)
        {
            _consumer.Receive(queueName, messageHandler);
        }

        public void CloseConnection()
        {
            // Implementation for closing the connection
        }

    }
}
