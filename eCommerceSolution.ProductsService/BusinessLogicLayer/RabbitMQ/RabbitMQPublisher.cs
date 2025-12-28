using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace eCommerce.ProductsService.BusinessLogicLayer.RabbitMQ;

public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
{
  private readonly IConfiguration _configuration;
  private readonly IModel _channel;
  private readonly IConnection _connection;

  public RabbitMQPublisher(IConfiguration configuration)
  {
    _configuration = configuration;

    Console.WriteLine($"RabbitMQ_HostName: {_configuration["RabbitMQ_HostName"]}");
    Console.WriteLine($"RabbitMQ_UserName: {_configuration["RabbitMQ_UserName"]}");
    Console.WriteLine($"RabbitMQ_Password: {_configuration["RabbitMQ_Password"]}");
    Console.WriteLine($"RabbitMQ_Port: {_configuration["RabbitMQ_Port"]}");

    string hostName = _configuration["RabbitMQ_HostName"]!;
    string userName = _configuration["RabbitMQ_UserName"]!;
    string password = _configuration["RabbitMQ_Password"]!;
    string port = _configuration["RabbitMQ_Port"]!;

    


    ConnectionFactory connectionFactory = new ConnectionFactory()
    {
      HostName = hostName,
      UserName = userName,
      Password = password,
      Port = Convert.ToInt32(port)
    };
    _connection = connectionFactory.CreateConnection();

    _channel = _connection.CreateModel();
  }


  public void Publish<T>(Dictionary<string, object> headers, T message)
  {
    string messageJson = JsonSerializer.Serialize(message);
    byte[] messageBodyInBytes = Encoding.UTF8.GetBytes(messageJson);

    //Create exchange
    string exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;
    _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Headers, durable: true);

    //Publish message
    var basicProperties = _channel.CreateBasicProperties();
    basicProperties.Headers = headers;

    _channel.BasicPublish(exchange: exchangeName, routingKey: string.Empty, basicProperties: basicProperties, body: messageBodyInBytes);
  }

  public void Dispose()
  {
    _channel.Dispose();
    _connection.Dispose();
  }
}
