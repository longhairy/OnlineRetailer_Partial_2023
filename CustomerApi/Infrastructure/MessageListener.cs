using System;
using System.Threading;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using CustomerApi.Data;
using CustomerApi.Models;
using SharedModels;

namespace CustomerApi.Infrastructure
{
    public class MessageListener
    {
        IServiceProvider provider;
        string connectionString;

        // The service provider is passed as a parameter, because the class needs
        // access to the product repository. With the service provider, we can create
        // a service scope that can provide an instance of the product repository.
        public MessageListener(IServiceProvider provider, string connectionString)
        {
            this.provider = provider;
            this.connectionString = connectionString;
        }

        public void Start()
        {
            using (var bus = RabbitHutch.CreateBus(connectionString))
            {
                bus.PubSub.Subscribe<OrderStatusChangedMessage>("customerApiHkCompleted", 
                    HandleOrderCompleted, x => x.WithTopic("completed"));

                // Add code to subscribe to other OrderStatusChanged events:
                // * cancelled
                // * shipped
                // * paid
                // Implement an event handler for each of these events.
                // Be careful that each subscribe has a unique subscription id
                // (this is the first parameter to the Subscribe method). If they
                // get the same subscription id, they will listen on the same
                // queue.

                // Block the thread so that it will not exit and stop subscribing.
                lock (this)
                {
                    Monitor.Wait(this);
                }
            }

        }

        private void HandleOrderCompleted(OrderStatusChangedMessage message)
        {
            // A service scope is created to get an instance of the customer repository.
            // When the service scope is disposed, the customer repository instance will
            // also be disposed.
            
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var customerRepos = services.GetService<IRepository<Customer>>();

                //updates creditStanding of customer
                // Beware that this operation is not idempotent.
                
                var customer = customerRepos.Get((int)message.CustomerId);
                customer.HasGoodCreditStanding = false;
                customerRepos.Edit(customer);


                
            }
            
        }

    }
}
