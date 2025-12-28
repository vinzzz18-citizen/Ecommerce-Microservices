namespace eCommerce.ProductsService.BusinessLogicLayer.RabbitMQ;

public record ProductNameUpdateMessage(Guid ProductID, string? NewName);
