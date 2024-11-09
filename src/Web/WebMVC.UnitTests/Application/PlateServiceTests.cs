using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Domain.Entities;
using Moq;
using WebMVC.Application.Services;
using WebMVC.Infrastructure.Data.Repositories;
using Xunit;

namespace WebMVC.UnitTests.Application
{
    public class PlateServiceTests
    {
        private readonly Mock<IPlateRepository> _mockPlateRepository;
        private readonly PlateService _service;

        public PlateServiceTests()
        {
            _mockPlateRepository = new Mock<IPlateRepository>();
            _service = new PlateService(_mockPlateRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_WhenCalled_ReturnsAllPlatesAsDto()
        {
            var plates = new List<Plate>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Registration = "ABC123",
                    PurchasePrice = 100.0m,
                    Letters = "ABC",
                    Numbers = 123
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Registration = "DEF456",
                    PurchasePrice = 200.0m,
                    Letters = "DEF",
                    Numbers = 456
                }
            };

            _mockPlateRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(plates);

            var result = await _service.GetAllAsync();

            var plateDtos = result.ToList();
            Assert.Equal(plates.Count, plateDtos.Count);
            var resultList = plateDtos.ToList();
            Assert.Equal(plates[0].Id, resultList[0].Id);
            Assert.Equal(plates[0].Registration, resultList[0].Registration);
            Assert.Equal(plates[0].PurchasePrice, resultList[0].PurchasePrice);
            Assert.Equal(plates[0].SalePrice.Value, resultList[0].SalePrice);
            Assert.Equal(plates[0].Letters, resultList[0].Letters);
            Assert.Equal(plates[0].Numbers, resultList[0].Numbers);

            Assert.Equal(plates[1].Id, resultList[1].Id);
            Assert.Equal(plates[1].Registration, resultList[1].Registration);
            Assert.Equal(plates[1].PurchasePrice, resultList[1].PurchasePrice);
            Assert.Equal(plates[1].SalePrice.Value, resultList[1].SalePrice);
            Assert.Equal(plates[1].Letters, resultList[1].Letters);
            Assert.Equal(plates[1].Numbers, resultList[1].Numbers);
        }

        [Fact]
        public async Task GetAllAsync_WhenRepositoryReturnsEmpty_ReturnsEmptyDtoList()
        {
            _mockPlateRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Plate>());

            var result = await _service.GetAllAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetPagedPlatesAsync_WithValidPage_ReturnsPaginatedPlatesAsDto()
        {
            var pageNumber = 1;
            var pageSize = 2;
            var plates = new List<Plate>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Registration = "ABC123",
                    PurchasePrice = 100.0m,
                    Letters = "ABC",
                    Numbers = 123
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Registration = "DEF456",
                    PurchasePrice = 200.0m,
                    Letters = "DEF",
                    Numbers = 456
                }
            };

            _mockPlateRepository.Setup(repo => repo.GetPaginatedAsync(pageNumber, pageSize))
                .ReturnsAsync(plates);

            var result = await _service.GetPagedPlatesAsync(pageNumber, pageSize);

            Assert.Equal(pageNumber, result.PageNumber);
            Assert.Equal(pageSize, result.PageSize);
            Assert.Equal(plates.Count, result.Items.Count);
            Assert.Equal(plates[0].Id, result.Items[0].Id);
            Assert.Equal(plates[0].Registration, result.Items[0].Registration);
            Assert.Equal(plates[0].PurchasePrice, result.Items[0].PurchasePrice);
            Assert.Equal(plates[0].SalePrice.Value, result.Items[0].SalePrice);
            Assert.Equal(plates[0].Letters, result.Items[0].Letters);
            Assert.Equal(plates[0].Numbers, result.Items[0].Numbers);
        }

        [Fact]
        public async Task GetPagedPlatesAsync_WhenRepositoryReturnsEmpty_ReturnsEmptyPaginatedDto()
        {
            var pageNumber = 1;
            var pageSize = 2;

            _mockPlateRepository.Setup(repo => repo.GetPaginatedAsync(pageNumber, pageSize))
                .ReturnsAsync(new List<Plate>());

            var result = await _service.GetPagedPlatesAsync(pageNumber, pageSize);

            Assert.Empty(result.Items);
            Assert.Equal(pageNumber, result.PageNumber);
            Assert.Equal(pageSize, result.PageSize);
            Assert.False(result.HasPreviousPage);
            Assert.False(result.HasNextPage);
        }

        [Fact]
        public async Task GetPagedPlatesAsync_WithLargePageNumber_ReturnsEmptyPaginatedDto()
        {
            var pageNumber = 100;
            var pageSize = 2;

            _mockPlateRepository.Setup(repo => repo.GetPaginatedAsync(pageNumber, pageSize))
                .ReturnsAsync(new List<Plate>());

            var result = await _service.GetPagedPlatesAsync(pageNumber, pageSize);

            Assert.Empty(result.Items);
            Assert.Equal(pageNumber, result.PageNumber);
            Assert.Equal(pageSize, result.PageSize);
            Assert.True(result.HasPreviousPage);
            Assert.False(result.HasNextPage);
        }
    }
}
