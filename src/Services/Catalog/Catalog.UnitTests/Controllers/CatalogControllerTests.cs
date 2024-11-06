using Xunit;
using Moq;
using Catalog.API.Controllers;
using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CatalogControllerTests
{
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly CatalogController _controller;

    public CatalogControllerTests()
    {
        _mockContext = new Mock<ApplicationDbContext>();
        _controller = new CatalogController(_mockContext.Object);
    }

    [Fact]
    public async Task GetPlates_ReturnsPlatesWithCorrectMarkup()
    {
        // Arrange
        var plates = new List<Plate>
        {
            new Plate { Id = 1, Plate = "ABC123", PurchasePrice = 100 },
            new Plate { Id = 2, Plate = "XYZ456", PurchasePrice = 200 }
        }.AsQueryable();
        _mockContext.Setup(m => m.Plates).Returns(plates);

        // Act
        var result = await _controller.GetPlates() as OkObjectResult;
        var returnedPlates = result.Value as IEnumerable<dynamic>;

        // Assert
        Assert.NotNull(returnedPlates);
        Assert.Equal(120, returnedPlates.First().SalePrice); // 20% markup
    }

    [Fact]
    public async Task AddPlate_AddsPlateToDbContext()
    {
        // Arrange
        var plate = new Plate { Plate = "NEW123", PurchasePrice = 150 };

        // Act
        var result = await _controller.AddPlate(plate);

        // Assert
        _mockContext.Verify(m => m.Plates.Add(plate), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(), Times.Once);
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task GetPlatesOrderedByPrice_ReturnsPlatesInAscendingOrder()
    {
        // Arrange
        var plates = new List<Plate>
        {
            new Plate { Id = 1, Plate = "ABC123", PurchasePrice = 200 },
            new Plate { Id = 2, Plate = "XYZ456", PurchasePrice = 100 }
        }.AsQueryable();
        _mockContext.Setup(m => m.Plates).Returns(plates);

        // Act
        var result = await _controller.GetPlatesOrderedByPrice() as OkObjectResult;
        var returnedPlates = result.Value as IEnumerable<dynamic>;

        // Assert
        Assert.Equal(100, returnedPlates.First().PurchasePrice); // Lowest price first
    }

    [Fact]
    public async Task FilterPlates_ReturnsFilteredPlatesBasedOnQuery()
    {
        // Arrange
        var plates = new List<Plate>
        {
            new Plate { Id = 1, Plate = "ABC123", PurchasePrice = 100 },
            new Plate { Id = 2, Plate = "XYZ456", PurchasePrice = 200 }
        }.AsQueryable();
        _mockContext.Setup(m => m.Plates).Returns(plates);

        // Act
        var result = await _controller.FilterPlates("ABC") as OkObjectResult;
        var returnedPlates = result.Value as IEnumerable<dynamic>;

        // Assert
        Assert.Single(returnedPlates);
        Assert.Equal("ABC123", returnedPlates.First().Plate);
    }

    [Fact]
    public async Task ReservePlate_MarksPlateAsReserved()
    {
        // Arrange
        var plate = new Plate { Id = 1, Plate = "ABC123", PurchasePrice = 100, IsReserved = false };
        _mockContext.Setup(m => m.Plates.FindAsync(1)).ReturnsAsync(plate);

        // Act
        var result = await _controller.ReservePlate(1);

        // Assert
        Assert.True(plate.IsReserved);
        _mockContext.Verify(m => m.SaveChangesAsync(), Times.Once);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task SellPlate_MarksPlateAsSoldAndUpdatesRevenue()
    {
        // Arrange
        var plate = new Plate { Id = 1, Plate = "ABC123", PurchasePrice = 100, IsReserved = false, IsSold = false };
        _mockContext.Setup(m => m.Plates.FindAsync(1)).ReturnsAsync(plate);
        _mockContext.Setup(m => m.TotalRevenue).Returns(0);

        // Act
        var result = await _controller.SellPlate(1);

        // Assert
        Assert.True(plate.IsSold);
        _mockContext.VerifySet(m => m.TotalRevenue = 120, Times.Once); // 20% markup
        _mockContext.Verify(m => m.SaveChangesAsync(), Times.Once);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ApplyDiscount_DISCOUNTCodeReducesPriceBy25()
    {
        // Arrange
        var plates = new List<Plate>
        {
            new Plate { Id = 1, Plate = "ABC123", PurchasePrice = 100 }
        }.AsQueryable();
        _mockContext.Setup(m => m.Plates).Returns(plates);

        // Act
        var result = await _controller.ApplyDiscount("DISCOUNT") as OkObjectResult;
        var discountedPlates = result.Value as IEnumerable<dynamic>;

        // Assert
        Assert.Equal(95, discountedPlates.First().SalePrice); // 20% markup - £25
    }

    [Fact]
    public async Task ApplyDiscount_PERCENTOFFCodeReducesPriceBy15Percent()
    {
        // Arrange
        var plates = new List<Plate>
        {
            new Plate { Id = 1, Plate = "ABC123", PurchasePrice = 100 }
        }.AsQueryable();
        _mockContext.Setup(m => m.Plates).Returns(plates);

        // Act
        var result = await _controller.ApplyDiscount("PERCENTOFF") as OkObjectResult;
        var discountedPlates = result.Value as IEnumerable<dynamic>;

        // Assert
        Assert.Equal(102, discountedPlates.First().SalePrice); // 20% markup - 15%
    }

[Fact]
public async Task ApplyDiscount_InvalidPromoCode_ReturnsBadRequest()
{
    // Act
    var result = await _controller.ApplyDiscount("INVALIDCODE");

    // Assert
    Assert.IsType<BadRequestObjectResult>(result);
}

[Fact]
public async Task ReservePlate_WhenAlreadyReserved_ReturnsBadRequest()
{
    // Arrange
    var plate = new Plate { Id = 1, Plate = "ABC123", PurchasePrice = 100, IsReserved = true };
    _mockContext.Setup(m => m.Plates.FindAsync(1)).ReturnsAsync(plate);

    // Act
    var result = await _controller.ReservePlate(1);

    // Assert
    Assert.IsType<BadRequestObjectResult>(result);
}

[Fact]
public async Task SellPlate_WhenAlreadySold_ReturnsBadRequest()
{
    // Arrange
    var plate = new Plate { Id = 1, Plate = "ABC123", PurchasePrice = 100, IsSold = true };
    _mockContext.Setup(m => m.Plates.FindAsync(1)).ReturnsAsync(plate);

    // Act
    var result = await _controller.SellPlate(1);

    // Assert
    Assert.IsType<BadRequestObjectResult>(result);
}


}
