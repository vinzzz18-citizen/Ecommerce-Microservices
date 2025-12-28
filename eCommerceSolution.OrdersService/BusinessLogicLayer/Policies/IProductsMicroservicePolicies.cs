using Polly;

namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.Policies;

public interface IProductsMicroservicePolicies
{
  IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy();
  IAsyncPolicy<HttpResponseMessage> GetBulkheadIsolationPolicy();
}
