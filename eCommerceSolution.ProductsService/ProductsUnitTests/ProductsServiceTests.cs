using AutoFixture;
using AutoMapper;
using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.DataAccessLayer.Entities;
using eCommerce.DataAccessLayer.RepositoryContracts;
using eCommerce.ProductsService.BusinessLogicLayer.RabbitMQ;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Linq.Expressions;

namespace eCommerce.ProductsMicroService.UnitTests;

public class ProductsServiceTests
{
  private readonly Mock<IProductsRepository> _productsRepositoryMock;
  private readonly Mock<IMapper> _mapperMock;
  private readonly Mock<IValidator<ProductAddRequest>> _productAddRequestValidatorMock;
  private readonly Mock<IValidator<ProductUpdateRequest>> _productUpdateRequestValidatorMock;
  private readonly Mock<IRabbitMQPublisher> _rabbitMQPublisherMock;
  private readonly eCommerce.BusinessLogicLayer.Services.ProductsService _productsService;
  private readonly Fixture _fixture;

  public ProductsServiceTests()
  {
    _productsRepositoryMock = new Mock<IProductsRepository>();
    _mapperMock = new Mock<IMapper>();
    _productAddRequestValidatorMock = new Mock<IValidator<ProductAddRequest>>();
    _productUpdateRequestValidatorMock = new Mock<IValidator<ProductUpdateRequest>>();
    _rabbitMQPublisherMock = new Mock<IRabbitMQPublisher>();
    _fixture = new Fixture();

    _productsService = new eCommerce.BusinessLogicLayer.Services.ProductsService(
        _productAddRequestValidatorMock.Object,
        _productUpdateRequestValidatorMock.Object,
        _mapperMock.Object,
        _productsRepositoryMock.Object,
        _rabbitMQPublisherMock.Object);
  }



  [Fact]
  public async Task AddProduct_ShouldAddValidProduct()
  {
    // Arrange
    var productAddRequest = _fixture.Create<ProductAddRequest>();
    var product = _fixture.Create<Product>();
    var productResponse = _fixture.Create<ProductResponse>();

    _productAddRequestValidatorMock
        .Setup(validator => validator.ValidateAsync(It.IsAny<ProductAddRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new FluentValidation.Results.ValidationResult());

    _mapperMock.Setup(m => m.Map<Product>(It.IsAny<ProductAddRequest>())).Returns(product);
    _productsRepositoryMock.Setup(repo => repo.AddProduct(It.IsAny<Product>())).ReturnsAsync(product);
    _mapperMock.Setup(m => m.Map<ProductResponse>(It.IsAny<Product>())).Returns(productResponse);

    // Act
    var result = await _productsService.AddProduct(productAddRequest);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<ProductResponse>();
    result.Should().Be(productResponse);
    _productsRepositoryMock.Verify(repo => repo.AddProduct(It.IsAny<Product>()), Times.Once);
  }



  [Fact]
  public async Task AddProduct_ShouldThrowArgumentExceptionWhenInvalidProduct()
  {
    // Arrange
    var productAddRequest = _fixture.Create<ProductAddRequest>();
    var validationResult = new FluentValidation.Results.ValidationResult(new[] { new ValidationFailure("Property", "Error") });

    _productAddRequestValidatorMock
        .Setup(validator => validator.ValidateAsync(It.IsAny<ProductAddRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(validationResult);

    // Act
    Func<Task> act = async () => await _productsService.AddProduct(productAddRequest);

    // Assert
    await act.Should().ThrowAsync<ArgumentException>().WithMessage("Error");
    _productsRepositoryMock.Verify(repo => repo.AddProduct(It.IsAny<Product>()), Times.Never);
  }

  [Fact]
  public async Task AddProduct_ShouldThrowArgumentNullExceptionWhenRequestIsNull()
  {
    // Act
    Func<Task> act = async () => await _productsService.AddProduct(null);

    // Assert
    await act.Should().ThrowAsync<ArgumentNullException>();
    _productsRepositoryMock.Verify(repo => repo.AddProduct(It.IsAny<Product>()), Times.Never);
  }



  [Fact]
  public async Task GetProducts_ShouldReturnAllProducts()
  {
    // Arrange
    var products = _fixture.CreateMany<Product>(3);
    var productResponses = _fixture.CreateMany<ProductResponse>(3);

    _productsRepositoryMock.Setup(repo => repo.GetProducts()).ReturnsAsync(products);
    _mapperMock.Setup(m => m.Map<IEnumerable<ProductResponse>>(It.IsAny<IEnumerable<Product>>()))
               .Returns(productResponses);

    // Act
    var result = await _productsService.GetProducts();

    // Assert
    result.Should().NotBeNull();
    result.Count.Should().Be(3);
  }



  [Fact]
  public async Task GetProductByCondition_ShouldReturnProductWhenConditionMatches()
  {
    // Arrange
    var product = _fixture.Create<Product>();
    var productResponse = _fixture.Create<ProductResponse>();

    _productsRepositoryMock.Setup(repo => repo.GetProductByCondition(It.IsAny<Expression<Func<Product, bool>>>()))
                           .ReturnsAsync(product);
    _mapperMock.Setup(m => m.Map<ProductResponse>(product)).Returns(productResponse);

    // Act
    var result = await _productsService.GetProductByCondition(p => p.ProductID == product.ProductID);

    // Assert
    result.Should().NotBeNull();
    result.Should().Be(productResponse);
  }



  [Fact]
  public async Task UpdateProduct_ShouldUpdateExistingProduct()
  {
    // Arrange
    var productUpdateRequest = _fixture.Create<ProductUpdateRequest>();
    var existingProduct = _fixture.Create<Product>();
    var updatedProduct = _fixture.Create<Product>();
    var updatedProductResponse = _fixture.Create<ProductResponse>();

    _productsRepositoryMock.Setup(repo => repo.GetProductByCondition(It.IsAny<Expression<Func<Product, bool>>>()))
                           .ReturnsAsync(existingProduct);

    _productUpdateRequestValidatorMock.Setup(validator => validator.ValidateAsync(It.IsAny<ProductUpdateRequest>(), It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(new ValidationResult());

    _mapperMock.Setup(m => m.Map<Product>(productUpdateRequest)).Returns(updatedProduct);
    _productsRepositoryMock.Setup(repo => repo.UpdateProduct(updatedProduct)).ReturnsAsync(updatedProduct);
    _mapperMock.Setup(m => m.Map<ProductResponse>(updatedProduct)).Returns(updatedProductResponse);

    // Act
    var result = await _productsService.UpdateProduct(productUpdateRequest);

    // Assert
    result.Should().NotBeNull();
    result.Should().Be(updatedProductResponse);
    _productsRepositoryMock.Verify(repo => repo.UpdateProduct(updatedProduct), Times.Once);
  }



  [Fact]
  public async Task DeleteProduct_ShouldReturnTrueWhenProductIsDeleted()
  {
    // Arrange
    var productID = _fixture.Create<Guid>();
    var existingProduct = _fixture.Create<Product>();

    _productsRepositoryMock.Setup(repo => repo.GetProductByCondition(It.IsAny<Expression<Func<Product, bool>>>()))
                           .ReturnsAsync(existingProduct);
    _productsRepositoryMock.Setup(repo => repo.DeleteProduct(productID)).ReturnsAsync(true);

    // Act
    var result = await _productsService.DeleteProduct(productID);

    // Assert
    result.Should().BeTrue();
  }




  [Fact]
  public async Task AddProduct_ShouldThrowArgumentExceptionWhenValidationFails()
  {
    // Arrange
    var productAddRequest = _fixture.Create<ProductAddRequest>();
    var validationFailures = new List<ValidationFailure>
    {
        new ValidationFailure("ProductName", "Product name is required"),
        new ValidationFailure("UnitPrice", "Unit price must be greater than zero")
    };
    var validationResult = new ValidationResult(validationFailures);

    _productAddRequestValidatorMock
        .Setup(validator => validator.ValidateAsync(It.IsAny<ProductAddRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(validationResult);

    // Act
    Func<Task> act = async () => await _productsService.AddProduct(productAddRequest);

    // Assert
    await act.Should().ThrowAsync<ArgumentException>()
             .WithMessage("Product name is required, Unit price must be greater than zero");
    _productsRepositoryMock.Verify(repo => repo.AddProduct(It.IsAny<Product>()), Times.Never);
  }




  [Fact]
  public async Task GetProductByCondition_ShouldReturnNullWhenNoProductMatchesCondition()
  {
    // Arrange
    _productsRepositoryMock.Setup(repo => repo.GetProductByCondition(It.IsAny<Expression<Func<Product, bool>>>()))
                           .ReturnsAsync((Product)null);

    // Act
    var result = await _productsService.GetProductByCondition(p => p.ProductID == Guid.NewGuid());

    // Assert
    result.Should().BeNull();
  }




  [Fact]
  public async Task UpdateProduct_ShouldThrowArgumentExceptionWhenProductDoesNotExist()
  {
    // Arrange
    var productUpdateRequest = _fixture.Create<ProductUpdateRequest>();

    _productsRepositoryMock.Setup(repo => repo.GetProductByCondition(It.IsAny<Expression<Func<Product, bool>>>()))
                           .ReturnsAsync((Product)null);

    // Act
    Func<Task> act = async () => await _productsService.UpdateProduct(productUpdateRequest);

    // Assert
    await act.Should().ThrowAsync<ArgumentException>()
             .WithMessage("Invalid Product ID");
    _productsRepositoryMock.Verify(repo => repo.UpdateProduct(It.IsAny<Product>()), Times.Never);
  }




  [Fact]
  public async Task UpdateProduct_ShouldThrowArgumentExceptionWhenValidationFails()
  {
    // Arrange
    var productUpdateRequest = _fixture.Create<ProductUpdateRequest>();
    var existingProduct = _fixture.Create<Product>();
    var validationFailures = new List<ValidationFailure>
    {
        new ValidationFailure("ProductName", "Product name is required"),
        new ValidationFailure("UnitPrice", "Unit price must be greater than zero")
    };
    var validationResult = new ValidationResult(validationFailures);

    _productsRepositoryMock.Setup(repo => repo.GetProductByCondition(It.IsAny<Expression<Func<Product, bool>>>()))
                           .ReturnsAsync(existingProduct);
    _productUpdateRequestValidatorMock.Setup(validator => validator.ValidateAsync(It.IsAny<ProductUpdateRequest>(), It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(validationResult);

    // Act
    Func<Task> act = async () => await _productsService.UpdateProduct(productUpdateRequest);

    // Assert
    await act.Should().ThrowAsync<ArgumentException>()
             .WithMessage("Product name is required, Unit price must be greater than zero");
    _productsRepositoryMock.Verify(repo => repo.UpdateProduct(It.IsAny<Product>()), Times.Never);
  }




  [Fact]
  public async Task DeleteProduct_ShouldReturnFalseWhenProductDoesNotExist()
  {
    // Arrange
    var productID = _fixture.Create<Guid>();

    _productsRepositoryMock.Setup(repo => repo.GetProductByCondition(It.IsAny<Expression<Func<Product, bool>>>()))
                           .ReturnsAsync((Product)null);

    // Act
    var result = await _productsService.DeleteProduct(productID);

    // Assert
    result.Should().BeFalse();
    _productsRepositoryMock.Verify(repo => repo.DeleteProduct(It.IsAny<Guid>()), Times.Never);
  }

}

