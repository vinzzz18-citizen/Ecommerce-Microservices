namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.RabbitMQ;

public interface IRabbitMQProductNameUpdateConsumer
{
  void Consume();
  void Dispose();
}
