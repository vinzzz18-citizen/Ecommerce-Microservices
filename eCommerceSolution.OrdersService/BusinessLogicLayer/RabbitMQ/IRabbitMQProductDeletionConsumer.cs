namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.RabbitMQ;

public interface IRabbitMQProductDeletionConsumer
{
  void Consume();
  void Dispose();
}

