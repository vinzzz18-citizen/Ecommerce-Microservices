using Polly;

namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.Policies;

public interface IUsersMicroservicePolicies
{
  IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy();
}
