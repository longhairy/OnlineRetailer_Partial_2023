using SharedModels;

namespace OrderApi.Models
{
    public class OrderConverter : IConverter<Order, OrderDto>
    {
        public Order Convert(OrderDto sharedOrder)
        {
            return new Order
            {
                //Id = sharedProduct.Id,
                //Name = sharedProduct.Name,
                //Price = sharedProduct.Price,
                //ItemsInStock = sharedProduct.ItemsInStock,
                //ItemsReserved = sharedProduct.ItemsReserved
            };
        }

        public OrderDto Convert(Order hiddenOrder)
        {
            return new OrderDto
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

