using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
using MongoDB.Driver;

namespace eCommerce.OrdersMicroservice.DataAccessLayer.RepositoryContracts;

public interface IOrdersRepository
{
  /// <summary>
  /// Retrieves all Orders asynchronously
  /// </summary>
  /// <returns>Returns all orders from the orders collection</returns>
  Task<IEnumerable<Order>> GetOrders();


  /// <summary>
  /// Retrieves all Orders based on the specified condition asynchronously
  /// </summary>
  /// <param name="filter">The condition to filter orders</param>
  /// <returns>Returning a collection of matching orders</returns>
  Task<IEnumerable<Order?>> GetOrdersByCondition(FilterDefinition<Order> filter);


  /// <summary>
  /// Retrieves a single order based on the specified condition asynchronously
  /// </summary>
  /// <param name="filter">The condition to filter Orders</param>
  /// <returns>Returning matching order</returns>
  Task<Order?> GetOrderByCondition(FilterDefinition<Order> filter);


  /// <summary>
  /// Adds a new Order into the Orders collection asynchronously
  /// </summary>
  /// <param name="order">The order to be added</param>
  /// <returns>Returnes the added Order object or null if unsuccessful</returns>
  Task<Order?> AddOrder(Order order);


  /// <summary>
  /// Updates an existing order asynchronously
  /// </summary>
  /// <param name="order">The order to be added</param>
  /// <returns>Returns the updated order object; or null if not found</returns>
  Task<Order?> UpdateOrder(Order order);


  /// <summary>
  /// Deletes the order asynchronously
  /// </summary>
  /// <param name="orderID">The Order ID based on which we need to delete the order</param>
  /// <returns>Returns true if the deletion is successful, false otherwise</returns>
  Task<bool> DeleteOrder(Guid orderID);
}
