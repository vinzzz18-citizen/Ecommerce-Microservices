using eCommerce.BusinessLogicLayer.Mappers;
using eCommerce.BusinessLogicLayer.ServiceContracts;
using Microsoft.Extensions.DependencyInjection;
using eCommerce.BusinessLogicLayer.Validators;
using FluentValidation;
using eCommerce.ProductsService.BusinessLogicLayer.RabbitMQ;

namespace eCommerce.ProductsService.BusinessLogicLayer;

public static class DependencyInjection
{
  public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
  {
    //TO DO: Add Business Logic Layer services into the IoC container
    services.AddAutoMapper(typeof(ProductAddRequestToProductMappingProfile).Assembly);

    services.AddValidatorsFromAssemblyContaining
      <ProductAddRequestValidator>();

    services.AddScoped<IProductsService, eCommerce.BusinessLogicLayer.Services.ProductsService>();

    services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();

    return services;
  }
}
