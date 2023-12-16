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
            
        }
    }
}
