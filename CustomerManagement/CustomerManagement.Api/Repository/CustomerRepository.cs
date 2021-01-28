using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagement.Api.Common.Exception;
using CustomerManagement.Api.Models;
using CustomerManagement.Data;
using CustomerManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Api.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerManagementDbContext _context;

        public CustomerRepository(CustomerManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerDto>> GetCustomers()
        {
            var customers = await _context.Customers.ToListAsync();

            return CustomerDto.PopulateDtoList(customers);
        }

        public async Task<CustomerDto> AddCustomer(Customer newCustomer)
        {
            await _context.Customers.AddAsync(newCustomer);
            await _context.SaveChangesAsync();
            return newCustomer;
        }

        public async Task<CustomerDto> UpdateCustomer(Customer updatedCustomer, bool isResolvingNameConflict)
        {
            var existingCustomer = await _context.Customers.SingleOrDefaultAsync(c => c.Id == updatedCustomer.Id);

            if (existingCustomer == null) throw new NotFoundException("Customer not found");
            existingCustomer.FirstName = updatedCustomer.FirstName;
            existingCustomer.LastName = updatedCustomer.LastName;

            if (!isResolvingNameConflict)
            {
                existingCustomer.PhoneNumber = updatedCustomer.PhoneNumber;
                existingCustomer.Email = updatedCustomer.Email;
            }

            await _context.SaveChangesAsync();
            return existingCustomer;
        }

        public async Task DeleteCustomer(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            _context.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CustomerDto>> GetDuplicateCustomers()
        {
            var customers = await GetCustomers();

            return customers.GroupBy(c => new {c.FirstName, c.LastName}).Where(g => g.Count() > 1).SelectMany(c => c)
                .ToList();
        }
    }
}