using System;
using System.Threading.Tasks;
using Moq;
using WebMVC.Services;
using Xunit;

namespace WebMVC.UnitTests
{
    public class ShopServiceTests
    {
        private readonly Mock<ICatalogApiService> _mockCatalogApiService = new();
        
        [Fact]
        public async Task GivenPendPlateAction_WhenGuidReceived_ThenResultFromApiIsBoolean()
        {
            //Arrange
            _mockCatalogApiService.Setup(x => x.SellPlate(It.IsAny<Guid>())).ReturnsAsync(true);
            var shopService = new ShopService(_mockCatalogApiService.Object);

            //Act
            var result =  await shopService.SellPlate(Guid.NewGuid());

            //Assert

            Assert.NotNull(result);
            Assert.True(result);//because the endpoint returns a bool
        }

        [Fact]
        public async Task GivenPendPlateAction_WhenInvalidGuidIsReceived_ThenThrowsArgumentException()
        {
            // Arrange
            var invalidGuid = Guid.Empty;  // bad guid
            var shopService = new ShopService(_mockCatalogApiService.Object);

            // Act Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await shopService.PendPlate(invalidGuid));
        }
    }
}