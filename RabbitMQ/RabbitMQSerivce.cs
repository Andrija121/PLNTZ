using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RabbitMQ
{
    public class RabbitMQSerivce
    {
        private readonly RabbitMQConfiguration _configuration;
        private readonly RabbitMQProducer _producer;
        private readonly RabbitMQConsumer _consumer;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQSerivce(RabbitMQConfiguration configuration, RabbitMQProducer producer, RabbitMQConsumer consumer)        {
            _configuration = configuration;
            _producer = producer;
            _consumer = consumer;


            InitializeConnection();
        }
        private void InitializeConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration.Hostname,
                UserName = _configuration.UserName,
                Password = _configuration.Password,
                Port = _configuration.Port,
                VirtualHost = "/",
                Ssl = { ServerName = "rabbitmq-server" },
                RequestedConnectionTimeout = TimeSpan.FromSeconds(10),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                RequestedHeartbeat = TimeSpan.FromSeconds(30),
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
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
            _channel?.Close();
            _connection?.Close();
        }

    }
}
