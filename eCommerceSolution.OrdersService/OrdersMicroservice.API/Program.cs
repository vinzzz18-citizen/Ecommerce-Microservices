using eCommerce.OrdersMicroservice.DataAccessLayer;
using eCommerce.OrdersMicroservice.BusinessLogicLayer;
using FluentValidation.AspNetCore;
using eCommerce.OrdersMicroservice.API.Middleware;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.HttpClients;
using Polly;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.Policies;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//Add DAL and BLL services
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer(builder.Configuration);

builder.Services.AddControllers();

//FluentValidations
builder.Services.AddFluentValidationAutoValidation();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cors
builder.Services.AddCors(options => {
  options.AddDefaultPolicy(builder =>
  {
    builder.WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader();
  });
});

builder.Services.AddTransient<IUsersMicroservicePolicies, UsersMicroservicePolicies>();
builder.Services.AddTransient<IProductsMicroservicePolicies, ProductsMicroservicePolicies>();
builder.Services.AddTransient<IPollyPolicies, PollyPolicies>();


builder.Services.AddHttpClient<UsersMicroserviceClient>(client =>
{
  client.BaseAddress = new Uri($"http://{builder.Configuration["UsersMicroserviceName"]}:{builder.Configuration["UsersMicroservicePort"]}");
})
  .AddPolicyHandler(
   builder.Services.BuildServiceProvider().GetRequiredService<IUsersMicroservicePolicies>().GetCombinedPolicy())
   ;



builder.Services.AddHttpClient<ProductsMicroserviceClient>(client => {
  client.BaseAddress = new Uri($"http://{builder.Configuration["ProductsMicroserviceName"]}:{builder.Configuration["ProductsMicroservicePort"]}");
})
  .AddPolicyHandler(
   builder.Services.BuildServiceProvider().GetRequiredService<IProductsMicroservicePolicies>().GetFallbackPolicy())

  .AddPolicyHandler(
   builder.Services.BuildServiceProvider().GetRequiredService<IProductsMicroservicePolicies>().GetBulkheadIsolationPolicy())

  ;



var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();

//Cors
app.UseCors();

//Swagger
app.UseSwagger();
app.UseSwaggerUI();

//Auth
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//Endpoints
app.MapControllers();


app.Run();
