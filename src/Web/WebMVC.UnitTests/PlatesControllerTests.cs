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
        private readonly IPlatesService _mockPlatesService;
        private readonly PlatesController _controller;

        public PlatesControllerTests()
        {
            _mockPlatesService = Substitute.For<IPlatesService>();
            _controller = new PlatesController(_mockPlatesService);
        }

        [Fact]
        public async Task IndexShouldReturnPlateDataWithView()
        {
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

            _mockPlatesService.GetPlatesAsync(Arg.Any<SortOptions>(), Arg.Any<int>()).Returns(platesListResponse);

            var result = await _controller.Index();

            result.Should().BeOfType<ViewResult>().Which.Model.Should().Be(platesListResponse);
        }

        [Fact]
        public async Task CreateShouldRedirectToIndexWhenSuccessful()
        {
            _mockPlatesService.CreatePlateAsync(Arg.Any<PlateCreateRequest>()).Returns(new CreateUpdateResult { IsSuccess = true });

            var createRequest = new PlateCreateRequest
            {
                Registration = "BBB123",
                SalePrice = 1500,
                PurchasePrice = 100,
                Letters = "BBB",
                Numbers = 123,
            };

            var result = await _controller.Create(createRequest);

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

            _mockPlatesService.CreatePlateAsync(Arg.Any<PlateCreateRequest>()).Returns(errorResult);

            // tempData not initialised in tests by default.
            var httpContext = new DefaultHttpContext();
            var tempDataProvider = Substitute.For<ITempDataProvider>();
            var tempData = new TempDataDictionary(httpContext, tempDataProvider);

            _controller.TempData = tempData;

            var createRequest = new PlateCreateRequest
            {
                Registration = "BBB123",
                SalePrice = 1500,
                PurchasePrice = 100,
                Letters = "BBB",
                Numbers = 123,
            };

            var result = await _controller.Create(createRequest);

            result.Should().BeOfType<ViewResult>().Which.Model.Should().Be(createRequest);
            _controller.TempData["ErrorDetails"].Should().Be("The api encountered an error.");
        }

        [Fact]
        public async Task OrderingByRegAscendingByWillToggleRegSortViewBagValueAndSetCurrentSort()
        {
            // registrationAscending is the default sort
            await _controller.Index();

            _controller.ViewData["RegSort"].Should().Be("registrationDescending");
            _controller.ViewData["CurrentSortOrder"].Should().Be(string.Empty);
        }


        [Fact]
        public async Task OrderingByRegDescendingByWillToggleRegSortViewBagValueAndSetCurrentSort()
        {
            await _controller.Index(orderBy: "registrationDescending");

            _controller.ViewData["RegSort"].Should().Be(string.Empty);
            _controller.ViewData["CurrentSortOrder"].Should().Be("registrationDescending");
        }

        [Fact]
        public async Task OrderingBySalePriceAscendingByWillToggleSalePriceSortViewBagValueAndSetCurrentSort()
        {
            await _controller.Index(orderBy: "salePriceAscending");

            _controller.ViewData["SalePriceSort"].Should().Be("salePriceDescending");
            _controller.ViewData["CurrentSortOrder"].Should().Be("salePriceAscending");
        }

        [Fact]
        public async Task OrderingBySalePriceDescendingByWillToggleSalePriceSortViewBagValueAndSetCurrentSort()
        {
            await _controller.Index(orderBy: "salePriceDescending");

            _controller.ViewData["SalePriceSort"].Should().Be("salePriceAscending");
            _controller.ViewData["CurrentSortOrder"].Should().Be("salePriceDescending");

            await _mockPlatesService.Received().GetPlatesAsync(Arg.Is(SortOptions.SalePriceDescending), Arg.Is(1));
        }

        [Fact]
        public async Task CanParseEmptyOrderByStringToEnum()
        {
            // registrationAscending is the default sort
            await _controller.Index();

            await _mockPlatesService.Received().GetPlatesAsync(Arg.Is(SortOptions.RegistrationAscending), Arg.Is(1));
        }

        [Fact]
        public async Task CanParseRegistrationDescendingOrderByStringToEnum()
        {
            await _controller.Index(orderBy: "registrationDescending");

            await _mockPlatesService.Received().GetPlatesAsync(Arg.Is(SortOptions.RegistrationDescending), Arg.Is(1));
        }

        [Fact]
        public async Task CanParseSalePriceAscendingOrderByStringToEnum()
        {
            await _controller.Index(orderBy: "salePriceAscending");

            await _mockPlatesService.Received().GetPlatesAsync(Arg.Is(SortOptions.SalePriceAscending), Arg.Is(1));
        }

        [Fact]
        public async Task CanParseSalePriceDescendingOrderByStringToEnum()
        {
            await _controller.Index(orderBy: "salePriceDescending");

            await _mockPlatesService.Received().GetPlatesAsync(Arg.Is(SortOptions.SalePriceDescending), Arg.Is(1));
        }
    }
}