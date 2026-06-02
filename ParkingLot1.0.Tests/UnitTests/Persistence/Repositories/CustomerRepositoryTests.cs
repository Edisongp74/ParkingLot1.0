using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Persistence.Contexts;
using ParkingLot1._0.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLot1._0.Tests.UnitTests.Persistence.Repositories
{
    [TestClass]
    public class CustomerRepositoryTests : BaseTests
    {
        private ApplicationDbContext _context;
        private CustomerRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _context = BuildContext();
            _repository = new CustomerRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }

        [TestMethod]
        public async Task AddAsync_WithValidCustomer_PersistsEntityAfterSaveChanges()
        {
            // Arrange
            Customer customer = new Customer
            {
                FirstName = "Juan",
                LastName = "Pérez",
                DocumentNumber = "123456789",
                DocumentType = "CC",
                Phone = "3001234567",
                CustomerType = "Regular"
            };

            // Act
            int customerId = await _repository.AddAsync(customer);
            await SaveChangesAsync(_context);

            Customer? persistedCustomer = await _context.Customers.FindAsync(customerId);

            // Assert
            Assert.IsNotNull(persistedCustomer);
            Assert.AreEqual("Juan", persistedCustomer.FirstName);
            Assert.AreEqual("Pérez", persistedCustomer.LastName);
            Assert.AreEqual("123456789", persistedCustomer.DocumentNumber);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenCustomerExists_ReturnsCustomer()
        {
            // Arrange
            Customer customer = new Customer
            {
                FirstName = "María",
                LastName = "García",
                DocumentNumber = "987654321",
                DocumentType = "CC",
                Phone = "3009876543",
                CustomerType = "VIP"
            };
            _context.Customers.Add(customer);
            await SaveChangesAsync(_context);

            // Act
            Customer? result = await _repository.GetByIdAsync(customer.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(customer.Id, result.Id);
            Assert.AreEqual("María", result.FirstName);
            Assert.AreEqual("García", result.LastName);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenCustomerDoesNotExist_ReturnsNull()
        {
            // Arrange
            int missingId = 999;

            // Act
            Customer? result = await _repository.GetByIdAsync(missingId);

            // Assert
            Assert.IsNull(result);
        }
    }
}
