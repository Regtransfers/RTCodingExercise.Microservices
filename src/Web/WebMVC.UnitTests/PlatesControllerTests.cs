using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebMVC.Controllers;
using WebMVC.Services;
using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Catalog.Domain;

namespace WebMVC.Tests
{
    public class PlatesControllerTests
    {
        private readonly Mock<ICatalogApiService> _mockCatalogApiService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly PlatesController _controller;

        public PlatesControllerTests()
        {
            // Mock dependencies
            _mockCatalogApiService = new Mock<ICatalogApiService>();
            _mockConfiguration = new Mock<IConfiguration>();

            // Initialize the controller
            _controller = new PlatesController(_mockCatalogApiService.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task AddPlate_ValidPlate_ReturnsOk()
        {
            // Arrange
            var plate = new Plate
            {
                Id = Guid.NewGuid(),
                Registration = "ABC123",
                PurchasePrice = 1000,
                SalePrice = 1500,
                Letters = "ABC",
                Numbers = 123
            };

            _mockCatalogApiService.Setup(service => service.AddPlate(It.IsAny<Plate>())).ReturnsAsync(true);

            // Act
            var result = await _controller.AddPlate(plate);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task ReservePlate_ValidId_ReturnsPartialView()
        {
            // Arrange
            var plateId = Guid.NewGuid();
            var status = "Reserved";

            _mockCatalogApiService.Setup(service => service.ReservePlate(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true);
            _mockCatalogApiService.Setup(service => service.Plates()).ReturnsAsync(new List<Plate>());

            // Act
            var result = await _controller.ReservePlate(plateId, status);

            // Assert
            var partialViewResult = Assert.IsType<PartialViewResult>(result);
            Assert.Equal("_PlatesTable", partialViewResult.ViewName);
        }
    }
}
