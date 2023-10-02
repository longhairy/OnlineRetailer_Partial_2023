using System;
using RestSharp;
using SharedModels;

namespace OrderApi.Infrastructure
{
    public class CustomerServiceGateway : IServiceGateway<CustomerDto>
    {
        string customerServiceBaseUrl;

        public CustomerServiceGateway(string baseUrl)
        {
            customerServiceBaseUrl = baseUrl;
        }

        public async Task<CustomerDto> GetAsync(int id)
        {
            RestClient c = new RestClient(customerServiceBaseUrl);

            var request = new RestRequest(id.ToString());
            return await c.GetAsync<CustomerDto>(request);
        }
    }
}
