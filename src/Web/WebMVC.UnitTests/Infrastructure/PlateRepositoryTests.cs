using System;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using WebMVC.Infrastructure;
using WebMVC.Infrastructure.Data.Repositories;
using Xunit;

namespace WebMVC.UnitTests.Infrastructure
{
    public class PlateRepositoryTests : IAsyncLifetime
    {
        private readonly MsSqlContainer _sqlServerContainer;
        private ApplicationDbContext _context;

        public PlateRepositoryTests()
        {
            _sqlServerContainer = new MsSqlBuilder()
                .WithPassword("AStrongP4ssword!")
                .WithPortBinding(1434, 1433)
                .WithEnvironment("ACCEPT_EULA", "Y") 
                .Build();
            
            Environment.SetEnvironmentVariable("TESTCONTAINERS_RYUK_DISABLED", "true");
        }

        public async Task InitializeAsync()
        {
            await _sqlServerContainer.StartAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(_sqlServerContainer.GetConnectionString())
                .Options;

            _context = new ApplicationDbContext(options);
            await _context.Database.EnsureCreatedAsync(); 
            await SeedDatabaseAsync();
        }

        public async Task DisposeAsync()
        {
            await _context.DisposeAsync();
            await _sqlServerContainer.DisposeAsync();
        }

        private async Task SeedDatabaseAsync()
        {
            _context.Plates.AddRange(
                new Plate
                {
                    Id = Guid.NewGuid(), Registration = "ABC123", PurchasePrice = 100, Letters = "ABC", Numbers = 123
                },
                new Plate
                {
                    Id = Guid.NewGuid(), Registration = "DEF456", PurchasePrice = 200, Letters = "DEF", Numbers = 456
                },
                new Plate
                {
                    Id = Guid.NewGuid(), Registration = "GHI789", PurchasePrice = 300, Letters = "GHI", Numbers = 789
                }
            );
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllAsync_WhenCalled_ReturnsAllPlates()
        {
            var repository = new PlateRepository(_context);

            var result = await repository.GetAllAsync();

            var resultAsList = result.ToList();
            Assert.Equal(3, resultAsList.Count);
            Assert.Contains(resultAsList, plate => plate.Registration == "ABC123");
            Assert.Contains(resultAsList, plate => plate.Registration == "DEF456");
            Assert.Contains(resultAsList, plate => plate.Registration == "GHI789");
        }

        [Fact]
        public async Task GetPaginatedAsync_WithValidPageNumber_ReturnsCorrectPageOfPlates()
        {
            var repository = new PlateRepository(_context);
            var pageNumber = 1;
            var pageSize = 2;

            var result = await repository.GetPaginatedAsync(pageNumber, pageSize);

            Assert.Equal(pageSize, result.Count());
        }

        [Fact]
        public async Task GetPaginatedAsync_WithSecondPage_ReturnsRemainingItems()
        {
            var repository = new PlateRepository(_context);
            var pageNumber = 2;
            var pageSize = 2;

            var result = await repository.GetPaginatedAsync(pageNumber, pageSize);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetPaginatedAsync_WithLargePageNumber_ReturnsEmptyResult()
        {
            var repository = new PlateRepository(_context);
            var pageNumber = 10;
            var pageSize = 2;

            var result = await repository.GetPaginatedAsync(pageNumber, pageSize);

            Assert.Empty(result);
        }
    }
}