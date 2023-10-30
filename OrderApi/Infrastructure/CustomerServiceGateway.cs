using System;
using RestSharp;
using SharedModels;
using Polly;
using Polly.Retry;

namespace OrderApi.Infrastructure
{
    public class CustomerServiceGateway : IServiceGateway<CustomerDto>
    {
        string customerServiceBaseUrl;
        private readonly AsyncRetryPolicy _retryPolicy;

        public CustomerServiceGateway(string baseUrl)
        {
            customerServiceBaseUrl = baseUrl;

            _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(10, retryAttempt =>
                TimeSpan.FromSeconds(3), onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount}. Attemp failed, trying again...");
                }
            );
        }

        public async Task<CustomerDto> GetAsync(int id)
        {
            RestClient c = new RestClient(customerServiceBaseUrl);

            var request = new RestRequest(id.ToString());
            var resp = await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await c.GetAsync<CustomerDto>(request);
                return response;

            });
            return resp;       
        }
    }
}
