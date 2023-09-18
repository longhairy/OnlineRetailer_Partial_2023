using SharedModels;

namespace CustomerApi.Models
{
    public class CustomerConverter : IConverter<Customer, CustomerDto>
    {
        public Customer Convert(CustomerDto sharedCustomer)
        {
            return new Customer
            {
                //Id = sharedProduct.Id,
                //Name = sharedProduct.Name,
                //Price = sharedProduct.Price,
                //ItemsInStock = sharedProduct.ItemsInStock,
                //ItemsReserved = sharedProduct.ItemsReserved
            };
        }

        public CustomerDto Convert(Customer hiddenCustomer)
        {
            return new CustomerDto
            {
                //Id = hiddenProduct.Id,
                //Name = hiddenProduct.Name,
                //Price = hiddenProduct.Price,
                //ItemsInStock = hiddenProduct.ItemsInStock,
                //ItemsReserved = hiddenProduct.ItemsReserved
            };
        }
    }
}

