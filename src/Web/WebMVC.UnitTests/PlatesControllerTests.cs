using Catalog.Domain;
using Catalog.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebMVC.Controllers;
using WebMVC.Services;
using Xunit;

namespace WebMVC.UnitTests
{
    public class PlatesControllerTests
    {
        [Fact]
        public async Task IndexShouldReturnPlateDataWithView()
        {
            var mockPlatesService = Substitute.For<IPlatesService>();

            var plates = new List<Plate>()
            {
                new() { Id = Guid.NewGuid(), Registration = "A123", SalePrice = 2000, PurchasePrice = 400, Letters = "A", Numbers = 123 },
                new() { Id = Guid.NewGuid(), Registration = "B123", SalePrice = 2000, PurchasePrice = 450, Letters = "B", Numbers = 123 },
                new() { Id = Guid.NewGuid(), Registration = "C123", SalePrice = 2000, PurchasePrice = 500, Letters = "C", Numbers = 123 },
            };

            var platesListResponse = new PlateListResponse
            {
                Plates = plates,
                CurrentPage = 1,
                TotalPages = 1,
                HasNext = false,
                HasPrevious = false,
            };

            mockPlatesService.GetPlatesAsync(Arg.Any<int>()).Returns(platesListResponse);

            var controller = new PlatesController(mockPlatesService);

            var result = await controller.Index();

            result.Should().BeOfType<ViewResult>().Which.Model.Should().Be(platesListResponse);
        }

        [Fact]
        public async Task CreateShouldRedirectToIndexWhenSuccessful()
        {
            var mockPlatesService = Substitute.For<IPlatesService>();
            mockPlatesService.CreatePlateAsync(Arg.Any<PlateCreateRequest>()).Returns(new CreateUpdateResult { IsSuccess = true });

            var controller = new PlatesController(mockPlatesService);

            var createRequest = new PlateCreateRequest
            {
                Registration = "BBB123",
                SalePrice = 1500,
                PurchasePrice = 100,
                Letters = "BBB",
                Numbers = 123,
            };

            var result = await controller.Create(createRequest);

            result.Should().BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task CreateShouldReturnViewWithErrorWhenCreateRequestFails()
        {
            var errorResult = new CreateUpdateResult
            {
                IsSuccess = false,
                ErrorMessage = "The api encountered an error.",
            };

            var mockPlatesService = Substitute.For<IPlatesService>();
            mockPlatesService.CreatePlateAsync(Arg.Any<PlateCreateRequest>()).Returns(errorResult);

            // tempData not initialised in tests by default.
            var httpContext = new DefaultHttpContext();
            var tempDataProvider = Substitute.For<ITempDataProvider>();
            var tempData = new TempDataDictionary(httpContext, tempDataProvider);

            var controller = new PlatesController(mockPlatesService);
            controller.TempData = tempData;

            var createRequest = new PlateCreateRequest
            {
                Registration = "BBB123",
                SalePrice = 1500,
                PurchasePrice = 100,
                Letters = "BBB",
                Numbers = 123,
            };

            var result = await controller.Create(createRequest);

            result.Should().BeOfType<ViewResult>().Which.Model.Should().Be(createRequest);
            controller.TempData["ErrorDetails"].Should().Be("The api encountered an error.");
        }
    }
}