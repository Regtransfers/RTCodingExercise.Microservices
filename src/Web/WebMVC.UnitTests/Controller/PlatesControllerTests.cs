using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using WebMVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

public class PlatesControllerTests
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly PlatesController _controller;

    public PlatesControllerTests()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
        _controller = new PlatesController(_mockHttpClientFactory.Object);
    }

    [Fact]
    public async Task Index_ReturnsViewResultWithListOfPlates()
    {
        // Arrange
        var plates = new List<PlateViewModel>
        {
            new PlateViewModel { Plate = "ABC123", PurchasePrice = 100, SalePrice = 120 },
            new PlateViewModel { Plate = "XYZ456", PurchasePrice = 200, SalePrice = 240 }
        };

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(plates), Encoding.UTF8, "application/json")
        };

        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        var httpClient = new HttpClient(mockHandler.Object) { BaseAddress = new Uri("http://localhost") };
        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _controller.Index() as ViewResult;
        var model = result.Model as IEnumerable<PlateViewModel>;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task OrderedByPrice_ReturnsPlatesOrderedBySalePrice()
    {
        // Arrange
        var plates = new List<PlateViewModel>
        {
            new PlateViewModel { Plate = "XYZ456", PurchasePrice = 200, SalePrice = 240 },
            new PlateViewModel { Plate = "ABC123", PurchasePrice = 100, SalePrice = 120 }
        };

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(plates), Encoding.UTF8, "application/json")
        };

        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        var httpClient = new HttpClient(mockHandler.Object) { BaseAddress = new Uri("http://localhost") };
        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _controller.OrderedByPrice() as ViewResult;
        var model = result.Model as IEnumerable<PlateViewModel>;

        // Assert
        Assert.NotNull(model);
        Assert.True(model.SequenceEqual(model.OrderBy(p => p.SalePrice)), "List is not sorted by SalePrice.");
    }

    [Fact]
    public async Task Filter_ReturnsFilteredPlates()
    {
        // Arrange
        var plates = new List<PlateViewModel>
        {
            new PlateViewModel { Plate = "ABC123", PurchasePrice = 100, SalePrice = 120 }
        };

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(plates), Encoding.UTF8, "application/json")
        };

        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        var httpClient = new HttpClient(mockHandler.Object) { BaseAddress = new Uri("http://localhost") };
        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _controller.Filter("ABC") as ViewResult;
        var model = result.Model as IEnumerable<PlateViewModel>;

        // Assert
        Assert.Single(model);
        Assert.Equal("ABC123", model.First().Plate);
    }

    [Fact]
    public async Task Index_WhenApiFails_ReturnsErrorView()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        var httpClient = new HttpClient(mockHandler.Object) { BaseAddress = new Uri("http://localhost") };
        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Error", result.ViewName);
    }
}
