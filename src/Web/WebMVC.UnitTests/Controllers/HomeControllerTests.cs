using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RTCodingExercise.Microservices.Models;
using WebMVC.Application;
using WebMVC.Application.DTO;
using WebMVC.Application.Services;
using WebMVC.Controllers;
using WebMVC.Models;
using Xunit;

namespace WebMVC.UnitTests.Controllers;

public class HomeControllerTests
{
    private readonly Mock<IPlateService> _mockPlateService;
    private readonly Mock<ILogger<HomeController>> _mockLogger;
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        _mockPlateService = new Mock<IPlateService>();
        _mockLogger = new Mock<ILogger<HomeController>>();
        _controller = new HomeController(_mockPlateService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Index_WithValidPageNumber_ReturnsViewResultWithHomeModel()
    {
        var pageNumber = 1;
        var pageSize = 20;
        var plates = new List<PlateDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Registration = "ABC123",
                PurchasePrice = 100.0m,
                SalePrice = 120.0m,
                Letters = "ABC",
                Numbers = 123
            }
        };
        var paginatedPlates = new PaginatedList<PlateDto>(plates, pageNumber, pageSize);

        _mockPlateService.Setup(service => service.GetPagedPlatesAsync(pageNumber, pageSize))
            .ReturnsAsync(paginatedPlates);

        var result = await _controller.Index(pageNumber);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Home>(viewResult.Model);
        Assert.Collection(model.Plates, plate =>
        {
            Assert.Equal(plates[0].Id, plate.Id);
            Assert.Equal(plates[0].Registration, plate.Registration);
            Assert.Equal(plates[0].PurchasePrice, plate.PurchasePrice);
            Assert.Equal(plates[0].SalePrice, plate.SalePrice);
            Assert.Equal(plates[0].Letters, plate.Letters);
            Assert.Equal(plates[0].Numbers, plate.Numbers);
        });
        Assert.Equal(pageNumber, model.PageNumber);
        Assert.False(model.HasPreviousPage);
        Assert.Equal(plates.Count == pageSize, model.HasNextPage);
    }

    [Fact]
    public async Task Index_WithNoPlates_ReturnsViewResultWithEmptyHomeModel()
    {
        var pageNumber = 1;
        var pageSize = 20;
        var paginatedPlates = new PaginatedList<PlateDto>(new List<PlateDto>(), pageNumber, pageSize);

        _mockPlateService.Setup(service => service.GetPagedPlatesAsync(pageNumber, pageSize))
            .ReturnsAsync(paginatedPlates);

        var result = await _controller.Index(pageNumber);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Home>(viewResult.Model);
        Assert.Empty(model.Plates);
        Assert.Equal(pageNumber, model.PageNumber);
        Assert.False(model.HasPreviousPage);
        Assert.False(model.HasNextPage);
    }

    [Fact]
    public void Privacy_WhenCalled_ReturnsViewResult()
    {
        var result = _controller.Privacy();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error_WhenCalled_ReturnsViewResultWithErrorViewModel()
    {
        var expectedRequestId = "test-request-id";
        
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext.HttpContext = httpContext;
        httpContext.TraceIdentifier = expectedRequestId;

        var result = _controller.Error();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
        Assert.Equal(expectedRequestId, model.RequestId);
    }
}