using Moq;
using ParkingLot1._0.Application.Features.Customers.Commands.CreateCustomer;
using ParkingLot1._0.Application.Features.Customers.Commands.DeleteCustomer;
using ParkingLot1._0.Application.Features.Customers.Queries.GetAllCustomers;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLot1._0.Tests.UnitTests.Application.Features.Customers.Commands
{
    [TestClass]
    public class CreateCustomerCommandHandlerTests
    {
        private Mock<ICustomerRepository> _repository;
        private CreateCustomerCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _repository = new Mock<ICustomerRepository>();
            _handler = new CreateCustomerCommandHandler(_repository.Object);
        }

        [TestMethod]
        public async Task Handle_WithValidCommand_CreatesCustomerAndReturnsId()
        {
            // Arrange
            CreateCustomerCommand command = new CreateCustomerCommand
            {
                FirstName = "Juan",
                LastName = "Pérez",
                DocumentNumber = "123456789",
                DocumentType = "CC",
                Phone = "3001234567",
                CustomerType = "Regular"
            };

            _repository.Setup(r => r.AddAsync(It.IsAny<Customer>()))
                                    .ReturnsAsync(1);

            // Act
            int resultId = await _handler.Handle(command);

            // Assert
            Assert.AreEqual(1, resultId);
            _repository.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
        }

        [TestMethod]
        public async Task Handle_WhenRepositoryThrowsException_Rethrows()
        {
            // Arrange
            CreateCustomerCommand command = new CreateCustomerCommand
            {
                FirstName = "Juan",
                LastName = "Pérez",
                DocumentNumber = "123456789",
                DocumentType = "CC",
                Phone = "3001234567",
                CustomerType = "Regular"
            };

            _repository.Setup(r => r.AddAsync(It.IsAny<Customer>()))
                                    .ThrowsAsync(new InvalidOperationException("Error al crear el cliente"));

            // Act & Assert
            await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () => await _handler.Handle(command));

            _repository.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
        }
    }

    [TestClass]
    public class GetAllCustomersHandlerTests
    {
        private Mock<ICustomerRepository> _repository;
        private GetAllCustomersQueryHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _repository = new Mock<ICustomerRepository>();
            _handler = new GetAllCustomersQueryHandler(_repository.Object);
        }

        [TestMethod]
        public async Task Handle_ReturnsListOfCustomers()
        {
            // Arrange
            List<Customer> customers = new List<Customer>
            {
                new Customer { Id = 1, FirstName = "Juan", LastName = "Pérez" },
                new Customer { Id = 2, FirstName = "María", LastName = "García" }
            };

            _repository.Setup(r => r.GetAllAsync())
                                    .ReturnsAsync(customers);

            GetAllCustomersQuery query = new GetAllCustomersQuery();

            // Act
            List<Customer> result = await _handler.Handle(query);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Juan", result[0].FirstName);
            Assert.AreEqual("María", result[1].FirstName);
            _repository.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }

    [TestClass]
    public class DeleteCustomerCommandHandlerTests
    {
        private Mock<ICustomerRepository> _repository;
        private DeleteCustomerCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _repository = new Mock<ICustomerRepository>();
            _handler = new DeleteCustomerCommandHandler(_repository.Object);
        }

        [TestMethod]
        public async Task Handle_WhenCustomerExists_DeletesCustomer()
        {
            // Arrange
            Customer customer = new Customer { Id = 1, FirstName = "Juan", LastName = "Pérez" };

            _repository.Setup(r => r.GetByIdAsync(1))
                                    .ReturnsAsync(customer);

            _repository.Setup(r => r.DeleteAsync(1))
                                    .Returns(Task.CompletedTask);

            DeleteCustomerCommand command = new DeleteCustomerCommand { Id = 1 };

            // Act
            await _handler.Handle(command);

            // Assert
            _repository.Verify(r => r.GetByIdAsync(1), Times.Once);
            _repository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task Handle_WhenCustomerDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            _repository.Setup(r => r.GetByIdAsync(999))
                                    .ReturnsAsync((Customer?)null);

            DeleteCustomerCommand command = new DeleteCustomerCommand { Id = 999 };

            // Act & Assert
            await Assert.ThrowsExactlyAsync<NotFoundException>(async () => await _handler.Handle(command));

            _repository.Verify(r => r.GetByIdAsync(999), Times.Once);
            _repository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
