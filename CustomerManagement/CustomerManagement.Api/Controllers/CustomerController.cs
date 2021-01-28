using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagement.Api.Common.Exception;
using CustomerManagement.Api.Models;
using CustomerManagement.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger _logger;

        public CustomerController(ICustomerRepository customerRepository,
            ILogger<CustomerController> logger)
        {
            _logger = logger;
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> GetCustomers()
        {
            var customers = await _customerRepository.GetCustomers();
            return Ok(customers);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> AddCustomer(CustomerDto newCustomer)
        {
            var existingCustomers = await _customerRepository.GetCustomers();
            await ValidateCustomer(newCustomer, existingCustomers);

            var customer = await _customerRepository.AddCustomer(newCustomer);
            return Ok(customer);
        }

        private async Task ValidateCustomer(CustomerDto customer, List<CustomerDto> existingCustomers)
        {
            existingCustomers.Remove(existingCustomers.SingleOrDefault(c => c.Id == customer.Id));

            if (existingCustomers.Exists(c => c.Email == customer.Email) ||
                existingCustomers.Exists(c => c.PhoneNumber == customer.PhoneNumber))
                throw new UniqueEntityRuleException("Phone number or Email Address is not Unique");
        }

        [HttpPut]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(CustomerDto updatedCustomer)
        {
            var existingCustomers = await _customerRepository.GetCustomers();
            await ValidateCustomer(updatedCustomer, existingCustomers);

            var customer = await _customerRepository.UpdateCustomer(updatedCustomer, false);
            return Ok(customer);
        }

        [HttpPut("resolvename")]
        public async Task<ActionResult<CustomerDto>> ResolveCustomerName(CustomerDto updatedCustomer)
        {
            var existingCustomers = await _customerRepository.GetCustomers();
            await ValidateCustomer(updatedCustomer, existingCustomers);
            await ValidateCustomerNames(updatedCustomer, existingCustomers);

            var customer = await _customerRepository.UpdateCustomer(updatedCustomer, true);
            return Ok(customer);
        }

        private async Task ValidateCustomerNames(CustomerDto customer, List<CustomerDto> existingCustomers)
        {
            if (existingCustomers.Exists(c => c.FirstName == customer.FirstName) &&
                existingCustomers.Exists(c => c.LastName == customer.LastName))
                throw new UpdateEntityRuleException("This name already exists");
        }

        [HttpDelete("{customerId}")]
        public async Task<ActionResult<CustomerDto>> DeleteCustomer(int customerId)
        {
            await _customerRepository.DeleteCustomer(customerId);
            return Ok();
        }

        [HttpGet("duplicate")]
        public async Task<ActionResult<List<CustomerDto>>> GetDuplicateCustomers()
        {
            var customers = await _customerRepository.GetDuplicateCustomers();
            return Ok(customers);
        }
    }
}