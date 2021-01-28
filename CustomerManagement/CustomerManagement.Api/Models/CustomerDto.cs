using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CustomerManagement.Data.Entities;

namespace CustomerManagement.Api.Models
{
    public class CustomerDto
    {
        public int Id { get; set; }

        [Required] [StringLength(50)] public string FirstName { get; set; }

        [Required] [StringLength(50)] public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required] [StringLength(50)] [Phone] public string PhoneNumber { get; set; }

        public static implicit operator Customer(CustomerDto customerDto)
        {
            return new Customer
            {
                Id = customerDto.Id,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Email = customerDto.Email,
                PhoneNumber = customerDto.PhoneNumber
            };
        }

        public static implicit operator CustomerDto(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            };
        }

        public static List<CustomerDto> PopulateDtoList(List<Customer> customers)
        {
            var customersDto = new List<CustomerDto>();

            foreach (var customer in customers) customersDto.Add(customer);

            return customersDto;
        }
    }
}