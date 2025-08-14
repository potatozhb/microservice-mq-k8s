
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using PlatformService.Dtos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly RabbitMQ.Client.IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            Console.WriteLine($"--> try connect to the bus, HostName:{configuration["RabbitMQHost"]}, Port: {int.Parse(configuration["RabbitMQPort"])}");
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQHost"],
                Port = int.Parse(configuration["RabbitMQPort"]),
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("--> Connected to RabbitMQ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the message bus:{ex.Message}");
            }

        }

        public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
        {
            var message = JsonSerializer.Serialize(platformPublishDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, sending message ...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ Connection Closed, Not sending");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
            Console.WriteLine($"--> WE have sent {message}");
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection shutdown");
        }

        public void Dispose()
        {
            Console.WriteLine("--> MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

    }
}