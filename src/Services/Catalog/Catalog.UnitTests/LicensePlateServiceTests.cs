using Catalog.API.Data;
using Catalog.API.Services;
using Catalog.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Catalog.UnitTests
{
    public class LicensePlateServiceTests
    {
        private readonly Mock<ILicensePlateRepository> _licensePlateRepositoryMock;
        private readonly ILicensePlateService _licensePlateService;

        public LicensePlateServiceTests() 
        {
            _licensePlateRepositoryMock = new Mock<ILicensePlateRepository>();

            _licensePlateService = new LicensePlateService(_licensePlateRepositoryMock.Object);
        }
        [Fact]
        public void GetAllPlates_ReturnsListOfPlates()
        {
            // Arrange
            var expectedPlates = new List<Plate>
            {
                new() { Id = Guid.NewGuid(), Registration = "LK93 XTY", Letters = "LK", Numbers = 93, PurchasePrice = 100.57M, SalePrice = 125.00M },
                new() {  Id = Guid.NewGuid(), Registration = "MX93 XTY", Letters = "MX", Numbers = 93, PurchasePrice = 570.93M, SalePrice = 624.00M },
                new() {  Id = Guid.NewGuid(), Registration = "LK32 XTY", Letters = "LK", Numbers = 32, PurchasePrice = 989.57M, SalePrice = 1245.00M }

            };
            _licensePlateRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expectedPlates);

            // Act
            var actualPlates = _licensePlateService.GetAllAsync();

            // Assert
            Assert.NotNull(actualPlates.Result);
            Assert.Equal(expectedPlates.Count, actualPlates.Result.ToList().Count);
            Assert.Equal(expectedPlates, actualPlates.Result.ToList());
        }

        [Fact]
        public void AddPlate_IsSuccessful()
        {
            // Arrange
            var newPlate = new Plate() { Id = Guid.NewGuid(), Registration = "LK93 XTY", Letters = "LK", Numbers = 93, PurchasePrice = 100.57M, SalePrice = 125.00M };

            _licensePlateRepositoryMock.Setup(x => x.AddLicensePlate(newPlate));

            // Act
            var actualPlates = _licensePlateService.GetAllAsync();

            // Assert
            _licensePlateRepositoryMock.Verify(x => x.AddLicensePlate(newPlate), Times.Once());
        }
    }
}