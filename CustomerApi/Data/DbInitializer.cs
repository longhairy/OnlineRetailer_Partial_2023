using System.Collections.Generic;
using System.Linq;
using CustomerApi.Models;

namespace CustomerApi.Data
{
    public class DbInitializer : IDbInitializer
    {
        // This method will create and seed the database.
        public void Initialize(CustomerApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Customers
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            List<Customer> customers = new List<Customer>
            {
                new Customer {CustomerId = 1, Name = "Brian", Email = "brian@hotmail.com", Phone = "123455567", BillingAddress = "nygade 1", ShippingAddress = "nygade 2", HasGoodCreditStanding = false, NickName = "Long"},
                new Customer {CustomerId = 2, Name = "Wehba", Email = "Wehba@hotmail.com", Phone = "112345567", BillingAddress = "nygade 21", ShippingAddress = "nygade 22", HasGoodCreditStanding = true, NickName = "Long"}
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
}
