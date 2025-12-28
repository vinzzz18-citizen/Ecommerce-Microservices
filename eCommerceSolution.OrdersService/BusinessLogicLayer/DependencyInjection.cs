using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.Validators;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.Mappers;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.ServiceContracts;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.Services;
using StackExchange.Redis;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.RabbitMQ;


namespace eCommerce.OrdersMicroservice.BusinessLogicLayer;

public static class DependencyInjection
{
  public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
  {
    //TO DO: Add business logic layer services into the IoC container
    services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();

    services.AddAutoMapper(typeof(OrderAddRequestToOrderMappingProfile).Assembly);

    services.AddScoped<IOrdersService, OrdersService>();

    services.AddStackExchangeRedisCache(options =>
    {
      options.Configuration = $"{configuration["REDIS_HOST"]}:{configuration["REDIS_PORT"]}";
    });


    services.AddTransient<IRabbitMQProductNameUpdateConsumer, RabbitMQProductNameUpdateConsumer>();

    services.AddTransient<IRabbitMQProductDeletionConsumer, RabbitMQProductDeletionConsumer>();

    services.AddHostedService<RabbitMQProductNameUpdateHostedService>();

    services.AddHostedService<RabbitMQProductDeletionHostedService>();

    return services;
  }
}
