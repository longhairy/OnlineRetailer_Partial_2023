using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Data;
using OrderApi.Infrastructure;
using OrderApi.Models;
using RestSharp;
using SharedModels;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IRepository<Order> repository;

        private readonly IConverter<Order, OrderDto> orderConverter;
        private IMessagePublisher messagePublisher;
        IServiceGateway<ProductDto> productServiceGateway;
        IServiceGateway<CustomerDto> customerServiceGateway;

        public OrdersController(
            IRepository<Order> repos, 
            IConverter<Order, OrderDto> orderConverter,
            IServiceGateway<ProductDto> gateway,
            IServiceGateway<CustomerDto> customerGateway,
            IMessagePublisher publisher)
        {
            repository = repos;
            this.orderConverter = orderConverter;
            productServiceGateway = gateway;
            customerServiceGateway = customerGateway;
            messagePublisher = publisher;

        }

        // GET: orders
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            return repository.GetAll();
        }

        // GET orders/5
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST orders
        [HttpPost]
        public IActionResult Post([FromBody]Order hiddenOrder)
        {
            if (hiddenOrder == null)
            {
                return BadRequest();
            }
            OrderDto order = orderConverter.Convert(hiddenOrder);

            if (order.CustomerId==null||!CustomerExists((int)order.CustomerId))
            {
                return StatusCode(500, "Customer does not exist");
            }
            if (!CustomerHasGoodCreditStanding((int)order.CustomerId))
            {
                return StatusCode(500, "Customer has unpaid orders");
            }


            if (ProductItemsAvailable(order))
            {
                try
                {
                    // Publish OrderStatusChangedMessage. If this operation
                    // fails, the order will not be created
                    messagePublisher.PublishOrderStatusChangedMessage(
                        order.CustomerId, order.OrderLines, "completed");

                    // Create order.
                    order.Status = OrderDto.OrderStatus.completed;
                    var newOrder = repository.Add(orderConverter.Convert(order));
                    return CreatedAtRoute("GetOrder", new { id = newOrder.Id }, newOrder);
                }
                catch
                {
                    return StatusCode(500, "An error happened. Try again.");
                }
            }
            else
            {
                // If there are not enough product items available.
                return StatusCode(500, "Not enough items in stock.");
            }

            //if (ProductItemsAvailable(order))
            //{
            //    // Update the number of items reserved for the ordered products,
            //    // and create a new order.
            //    if (UpdateItemsReserved(order))
            //    {
            //        // Create order.
            //        order.Status = Order.OrderStatus.completed;
            //        var newOrder = repository.Add(order);
            //        return CreatedAtRoute("GetOrder",
            //            new { id = newOrder.Id }, newOrder);
            //    }
            //}

            //// If the order could not be created, "return no content".
            //return NoContent();
        }

        private bool CustomerExists(int customerId)
        {
            var customer = customerServiceGateway.Get(customerId);
            if (customer == null)
            { return false; }
            return true;
        }

        private bool CustomerHasGoodCreditStanding(int customerId)
        {
            var customer = customerServiceGateway.Get(customerId);
            return customer.HasGoodCreditStanding;
        }

        private bool ProductItemsAvailable(OrderDto order)
        {
            foreach (var orderLine in order.OrderLines)
            {
                // Call product service to get the product ordered.
                var orderedProduct = productServiceGateway.Get(orderLine.ProductId);
                if (orderLine.Quantity > orderedProduct.ItemsInStock - orderedProduct.ItemsReserved)
                {
                    return false;
                }
            }
            return true;
        }

        //private bool ProductItemsAvailable(Order order)
        //{
        //    foreach (var orderLine in order.OrderLines)
        //    {
        //        // Call product service to get the product ordered.
        //        // You may need to change the port number in the BaseUrl below
        //        // before you can run the request.
        //        RestClient c = new RestClient("http://productapi/products/");
        //        var request = new RestRequest(orderLine.ProductId.ToString());
        //        var response = c.GetAsync<ProductDto>(request);
        //        response.Wait();
        //        var orderedProduct = response.Result;
        //        if (orderLine.Quantity > orderedProduct.ItemsInStock - orderedProduct.ItemsReserved)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //private bool UpdateItemsReserved(Order order)
        //{
        //    foreach (var orderLine in order.OrderLines)
        //    {
        //        // Call product service to get the product ordered.
        //        // You may need to change the port number in the BaseUrl below
        //        // before you can run the request.
        //        RestClient c = new RestClient("http://productapi/products/");
        //        var request = new RestRequest(orderLine.ProductId.ToString());
        //        var response = c.GetAsync<ProductDto>(request);
        //        response.Wait();
        //        var orderedProduct = response.Result;
        //        orderedProduct.ItemsReserved += orderLine.Quantity;

        //        // Call product service to update the number of items reserved
        //        var updateRequest = new RestRequest(orderedProduct.Id.ToString());
        //        updateRequest.AddJsonBody(orderedProduct);
        //        var updateResponse = c.PutAsync(updateRequest);
        //        updateResponse.Wait();
        //        if (!updateResponse.IsCompletedSuccessfully)
        //            return false;

        //    }
        //    return true;
        //}

    }
}
