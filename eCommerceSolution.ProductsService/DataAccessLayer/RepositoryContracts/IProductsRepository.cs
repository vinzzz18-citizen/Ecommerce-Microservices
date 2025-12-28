using eCommerce.DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace eCommerce.DataAccessLayer.RepositoryContracts;

/// <summary>
/// Represents a repository for managing 'products' table
/// </summary>
public interface IProductsRepository
{
  /// <summary>
  /// Retrieves all products asynchronously
  /// </summary>
  /// <returns>Returns all products from the table</returns>
  Task<IEnumerable<Product>> GetProducts();


  /// <summary>
  /// Retrieves all products based on the specified condition asynchronously.
  /// </summary>
  /// <param name="conditionExpression">The condition to filter products</param>
  /// <returns>Returning a collection of matching products</returns>
  Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression);


  /// <summary>
  /// Retrieves a single product based on the specified condition asynchronously
  /// </summary>
  /// <param name="conditionExpression">The condition to filter the product</param>
  /// <returns>Returns a single product or null if not found</returns>
  Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression);


  /// <summary>
  /// Adds a new product into the products table asynchronously
  /// </summary>
  /// <param name="product">The product to be added</param>
  /// <returns>Returns the added product object or null if unsuccessful</returns>
  Task<Product?> AddProduct(Product product);


  /// <summary>
  /// Updates an existing product asynchronously.
  /// </summary>
  /// <param name="product">The product to be updated</param>
  /// <returns>Returns the updated product; or null if not found</returns>
  Task<Product?> UpdateProduct(Product product);



  /// <summary>
  /// Deletes the product asynchronously
  /// </summary>
  /// <param name="productID">The product ID to be deleted</param>
  /// <returns>Returns true if the deletion is successful, false otherwise.</returns>
  Task<bool> DeleteProduct(Guid productID);
}
