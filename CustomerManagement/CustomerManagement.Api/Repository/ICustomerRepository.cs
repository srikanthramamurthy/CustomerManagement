using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerManagement.Api.Models;
using CustomerManagement.Data.Entities;

namespace CustomerManagement.Api.Repository
{
    public interface ICustomerRepository
    {
        Task<List<CustomerDto>> GetCustomers();

        Task<CustomerDto> AddCustomer(Customer newCustomer);
        Task<CustomerDto> UpdateCustomer(Customer updatedCustomer, bool isResolvingNameConflict);
        Task DeleteCustomer(int customerId);

        Task<List<CustomerDto>> GetDuplicateCustomers();
    }
}