namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;

public record OrderItemResponse(Guid ProductID, decimal UnitPrice, int Quantity, decimal TotalPrice, string? ProductName, string? Category)
{
  public OrderItemResponse() : this(default, default, default, default, default, default)
  {
  }
}
