using Catalog.API.Data;
using Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Xunit;
using System.Data.Common;
using System.Linq;
using Polly;
using Microsoft.EntityFrameworkCore.Design;
using System.Collections;

namespace Catalog.UnitTests
{
    public class LicensePlateRepositoryTests
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;

        private IList<Plate> _seedDataPlates;

        public LicensePlateRepositoryTests()
        {
            _seedDataPlates = new List<Plate>()
            {
                new() { Id = Guid.NewGuid(), Registration = "LK93 XTY", Letters = "LK", Numbers = 93, PurchasePrice = 100.57M, SalePrice = 125.00M },
                new() { Id = Guid.NewGuid(), Registration = "MX93 XTY", Letters = "MX", Numbers = 93, PurchasePrice = 570.93M, SalePrice = 624.00M },
                new() { Id = Guid.NewGuid(), Registration = "LK32 XTY", Letters = "LK", Numbers = 32, PurchasePrice = 989.57M, SalePrice = 1245.00M }
            };

            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;
            
            using var context = new ApplicationDbContext(_contextOptions);

            context.Database.EnsureCreated();

            context.AddRange(_seedDataPlates);

            context.SaveChanges();
           
        }

        ApplicationDbContext CreateContext() => new(_contextOptions);

        public void Dispose() => _connection.Dispose();

        [Fact]
        public void GetAll_ReturnsAllPlates()
        {
            // Arrange 
            using var context = CreateContext();
            var licensePlateRepository = new LicensePlateRepository(context);

            // Act
            var result = licensePlateRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result.Result);
            Assert.Equal(_seedDataPlates.Count, result.Result.ToList().Count);
        }

        [Fact]
        public void AddPlate_PlateAddedSuccessfully()
        {
            // Arrange 
            using var context = CreateContext();
            var licensePlateRepository = new LicensePlateRepository(context);

            var newPlate = new Plate() { Id = Guid.NewGuid(), Registration = "LK93 XTY", Letters = "LK", Numbers = 93, PurchasePrice = 100.57M, SalePrice = 125.00M };
            
            // Act
            var result = licensePlateRepository.AddLicensePlateAsync(newPlate);

            // Assert
            var plates = context.Plates.ToList();
            Assert.Equal(4, plates.Count);
            Assert.Contains(newPlate, plates);
        }

    }
}
