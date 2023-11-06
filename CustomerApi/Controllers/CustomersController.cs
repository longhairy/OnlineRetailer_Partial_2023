using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CustomerApi.Data;
using CustomerApi.Models;
using SharedModels;

namespace CustomerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IRepository<Customer> repository;
        private readonly IConverter<Customer, CustomerDto> customerConverter;

        public CustomersController(IRepository<Customer> repos, IConverter<Customer, CustomerDto> converter)
        {
            repository = repos;
            customerConverter = converter;
        }

        // GET Customers
        [HttpGet]
        public IEnumerable<CustomerDto> Get()
        {
            var customerDtoList = new List<CustomerDto>();
            foreach (var customer in repository.GetAll())
            {
                var customerDto = customerConverter.Convert(customer);
                customerDtoList.Add(customerDto);
            }
            return customerDtoList;
        }

        // GET Customer/5
        [HttpGet("{id}", Name= "GetCustomer")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            var customerDto = customerConverter.Convert(item);
            return new ObjectResult(customerDto);
        }

        // POST Customers
        [HttpPost]
        public IActionResult Post([FromBody] CustomerDto customerDto)
        {
            if (customerDto == null)
            {
                return BadRequest();
            }

            var customer = customerConverter.Convert(customerDto);
            var newCustomer = repository.Add(customer);

            return CreatedAtRoute("GetCustomer", new { id = newCustomer.CustomerId },
                customerConverter.Convert(newCustomer));
        }

        // PUT Customers/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CustomerDto customerDto)
        {
            if (customerDto == null || customerDto.CustomerId != id)
            {
                return BadRequest();
            }

            var modifiedCustomer = repository.Get(id);

            if (modifiedCustomer == null)
            {
                return NotFound();
            }
            modifiedCustomer.Name = customerDto.Name;
            modifiedCustomer.Email = customerDto.Email;
            modifiedCustomer.Phone = customerDto.Phone;
            modifiedCustomer.BillingAddress = customerDto.BillingAddress;
            modifiedCustomer.ShippingAddress = customerDto.ShippingAddress;

            repository.Edit(modifiedCustomer);
            return new NoContentResult();
        }

        // DELETE Customers/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (repository.Get(id) == null)
            {
                return NotFound();
            }

            repository.Remove(id);
            return new NoContentResult();
        }

    }
}
