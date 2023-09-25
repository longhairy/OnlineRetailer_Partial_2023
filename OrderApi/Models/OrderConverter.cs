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
                OrderLines = sharedOrder.OrderLines.Select(Convert).ToList()
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
                OrderLines = hiddenOrder.OrderLines.Select(Convert).ToList()
            };
        }

        private OrderLine Convert(SharedModels.OrderLine sharedOrderLine)
        {
            return new OrderLine
            {
                Id = sharedOrderLine.Id,
                OrderId = sharedOrderLine.OrderId,
                ProductId = sharedOrderLine.ProductId,
                Quantity = sharedOrderLine.Quantity
            };
        }

        private SharedModels.OrderLine Convert(OrderLine hiddenOrderLine)
        {
            return new SharedModels.OrderLine
            {
                Id = hiddenOrderLine.Id,
                OrderId = hiddenOrderLine.OrderId,
                ProductId = hiddenOrderLine.ProductId,
                Quantity = hiddenOrderLine.Quantity
            };
        }
    }
}


