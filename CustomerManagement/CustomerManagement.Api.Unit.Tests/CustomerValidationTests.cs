using System.Collections.Generic;
using CustomerManagement.Api.Common.Exception;
using CustomerManagement.Api.Controllers;
using CustomerManagement.Api.Models;
using CustomerManagement.Api.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CustomerManagement.Api.Unit.Tests
{
    public class When_An_NewCustomer_Is_Saved
    {
        private static readonly List<CustomerDto> _Customers = new List<CustomerDto>
        {
            new CustomerDto
            {
                Id = 1,
                FirstName = "Homer",
                LastName = "Simpson",
                Email = "HomerSimpson@springfield.com",
                PhoneNumber = "00001"
            },
            new CustomerDto
            {
                Id = 1,
                FirstName = "Marge",
                LastName = "Simpson",
                Email = "MargeSimpson@springfield.com",
                PhoneNumber = "00002"
            },
            new CustomerDto
            {
                Id = 1,
                FirstName = "Bart",
                LastName = "Simpson",
                Email = "BartSimpson@springfield.com",
                PhoneNumber = "00003"
            }
        };

        [Fact]
        public async void And_An_Existing_Customer_Do_Not_Have_The_Same_Details_Then_No_Exception_Is_Thrown()
        {
            var newCustomer = new CustomerDto
            {
                FirstName = "Lisa",
                LastName = "Simpson",
                Email = "LisaSimpson@springfield.com",
                PhoneNumber = "00004"
            };


            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockLogger = new Mock<ILogger<CustomerController>>();
            var customerController = new CustomerController(mockCustomerRepository.Object, mockLogger.Object);
            mockCustomerRepository.Setup(cr => cr.GetCustomers()).ReturnsAsync(_Customers);

            mockCustomerRepository.Setup(cr => cr.AddCustomer(newCustomer)).ReturnsAsync(newCustomer);

            await customerController.AddCustomer(newCustomer);
        }

        [Fact]
        public async void And_An_Existing_Customer_Have_The_Same_Details_Then_Exception_Must_Be_Thrown()
        {
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockLogger = new Mock<ILogger<CustomerController>>();
            var customerController = new CustomerController(mockCustomerRepository.Object, mockLogger.Object);

            mockCustomerRepository.Setup(cr => cr.GetCustomers()).ReturnsAsync(_Customers);

            var newCustomer = new CustomerDto
            {
                FirstName = "Bart",
                LastName = "Simpson",
                Email = "BartSimpson@springfield.com",
                PhoneNumber = "00001"
            };

            await Assert.ThrowsAsync<UniqueEntityRuleException>(() => customerController.AddCustomer(newCustomer));
        }
    }
}