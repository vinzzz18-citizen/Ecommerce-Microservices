namespace eCommerce.ProductsService.BusinessLogicLayer.RabbitMQ;

public interface IRabbitMQPublisher
{
  void Publish<T>(Dictionary<string, object> headers, T message);
}
