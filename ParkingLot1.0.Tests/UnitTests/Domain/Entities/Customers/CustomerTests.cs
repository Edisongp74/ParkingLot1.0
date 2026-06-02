using ParkingLot1._0.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLot1._0.Tests.UnitTests.Domain.Entities.Customers
{
    [TestClass]
    public class CustomerTests
    {
        [TestMethod]
        public void Constructor_WithValidData_CreatesCustomer()
        {
            // Arrange
            string firstName = "Juan";
            string lastName = "Pérez";
            string documentNumber = "123456789";

            // Act
            Customer customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                DocumentNumber = documentNumber,
                DocumentType = "CC",
                Phone = "3001234567",
                CustomerType = "Regular"
            };

            // Assert
            Assert.AreEqual(firstName, customer.FirstName);
            Assert.AreEqual(lastName, customer.LastName);
            Assert.AreEqual(documentNumber, customer.DocumentNumber);
            Assert.AreEqual("Juan Pérez", customer.FullName);
        }

        [TestMethod]
        public void HasActiveMonthlyPass_WhenHasActivePass_ReturnsTrue()
        {
            // Arrange
            Customer customer = new Customer
            {
                FirstName = "Juan",
                LastName = "Pérez"
            };
            customer.MonthlyPasses.Add(new MonthlyPass
            {
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(20),
                Status = "Active"
            });

            // Act
            bool hasActivePass = customer.HasActiveMonthlyPass();

            // Assert
            Assert.IsTrue(hasActivePass);
        }

        [TestMethod]
        public void CanAddVehicle_WhenHasMaxVehicles_ReturnsFalse()
        {
            // Arrange
            Customer customer = new Customer
            {
                FirstName = "Juan",
                LastName = "Pérez"
            };
            customer.Vehicles.Add(new Vehicle { LicensePlate = "ABC123" });
            customer.Vehicles.Add(new Vehicle { LicensePlate = "DEF456" });
            customer.Vehicles.Add(new Vehicle { LicensePlate = "GHI789" });

            // Act
            bool canAdd = customer.CanAddVehicle();

            // Assert
            Assert.IsFalse(canAdd);
        }
    }
}
