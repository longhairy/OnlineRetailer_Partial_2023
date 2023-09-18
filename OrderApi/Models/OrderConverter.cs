using SharedModels;

namespace OrderApi.Models
{
    public class OrderConverter : IConverter<Order, OrderDto>
    {
        public Order Convert(OrderDto sharedOrder)
        {
            return new Order
            {
                Id = sharedOrder.Id,
                Date = sharedOrder.Date,
                CustomerId = sharedOrder.CustomerId,
                Status = (Order.OrderStatus)sharedOrder.Status,
                OrderLines = (IList<OrderLine>)sharedOrder.OrderLines
            };
        }

        public OrderDto Convert(Order hiddenOrder)
        {
            return new OrderDto
            {
                Id = hiddenOrder.Id,
                Date = hiddenOrder.Date,
                CustomerId = hiddenOrder.CustomerId,
                Status = (OrderDto.OrderStatus)hiddenOrder.Status,
                OrderLines = (IList<SharedModels.OrderLine>)hiddenOrder.OrderLines
            };
        }
    }
}

