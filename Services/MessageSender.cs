using System;
using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace UserService.Services
{
    public class MessageSender : IMessageSender
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _queueName;
        private readonly string _username;
        private IConnection _connection;


        public MessageSender()
        {

#pragma warning disable CS8601 // Possible null reference assignment.
            _hostname = Environment.GetEnvironmentVariable("RabbitMQHost");
            _username = Environment.GetEnvironmentVariable("RabbitMQUsername");
            _password = Environment.GetEnvironmentVariable("RabbitMQPassword");
            _queueName = Environment.GetEnvironmentVariable("RabbitMQQueueName");
#pragma warning restore CS8601 // Possible null reference assignment.
            CreateConnection();
        }

        public void SendMessage(string userId)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    string message = userId;
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
                }
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}
