using Catalog.API.Data;
using Catalog.API.Requests;
using Catalog.Domain;
using Catalog.DTO;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.UnitTests
{
    public class PlateRepositoryTests
    {
        const decimal SalesPriceMultiplier = 1.2m;
        const int PageSize = 20;
        private readonly ApplicationDbContext _context;
        private readonly PlateRepository _repository;

        public PlateRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new PlateRepository(_context);
        }

        [Fact]
        public async Task CanRetrievePlatesUpToPageLimitWithDefaultSortIncludingSalesPriceTransform()
        {
            var plates = CreatePlates(25);
            await _context.AddRangeAsync(plates);
            await _context.SaveChangesAsync();

            var retrieved = await _repository.GetAllAsync(new QueryOptions { PageNumber = 1 });

            foreach (var plate in plates)
            {
                plate.SalePrice = plate.SalePrice * SalesPriceMultiplier;
            }

            var expected = plates.OrderBy(p => p.Registration).Take(PageSize);

            retrieved.CurrentPage.Should().Be(1);
            retrieved.TotalPages.Should().Be(2);
            retrieved.Plates.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public async Task CanPageResults()
        {
            var plates = CreatePlates(25);
            await _context.AddRangeAsync(plates);
            await _context.SaveChangesAsync();

            var retrieved = await _repository.GetAllAsync(new QueryOptions { PageNumber = 2 });

            foreach (var plate in plates)
            {
                plate.SalePrice = plate.SalePrice * SalesPriceMultiplier;
            }

            var expected = plates.OrderBy(p => p.Registration).Skip(PageSize).Take(PageSize);

            retrieved.CurrentPage.Should().Be(2);
            retrieved.TotalPages.Should().Be(2);
            retrieved.Plates.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public async Task WillAddPlate()
        {
            var newPlate = new Plate
            {
                Id = Guid.NewGuid(),
                Registration = "AAA111",
                SalePrice = 9950,
                PurchasePrice = 550,
                Letters = "AAA",
                Numbers = 111,
            };

            await _repository.AddAsync(newPlate);

            var addedPlate = await _context.Plates.FindAsync(newPlate.Id);

            addedPlate.Should().BeEquivalentTo(newPlate);
        }

        [Fact]
        public async Task CanSortPlatesBySalePriceAscending()
        {
            var plates = CreatePlates(25);
            await _context.AddRangeAsync(plates);
            await _context.SaveChangesAsync();

            var retrieved = await _repository.GetAllAsync(
                new QueryOptions { PageNumber = 1, OrderBy = SortOptions.SalePriceAscending });

            foreach (var plate in plates)
            {
                plate.SalePrice = plate.SalePrice * SalesPriceMultiplier;
            }

            var expected = plates.OrderBy(p => p.SalePrice).Take(PageSize);

            retrieved.CurrentPage.Should().Be(1);
            retrieved.TotalPages.Should().Be(2);
            retrieved.Plates.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public async Task CanSortPlatesBySalePriceDescending()
        {
            var plates = CreatePlates(25);
            await _context.AddRangeAsync(plates);
            await _context.SaveChangesAsync();

            var retrieved = await _repository.GetAllAsync(
                new QueryOptions { PageNumber = 1, OrderBy = SortOptions.SalePriceDescending });

            foreach (var plate in plates)
            {
                plate.SalePrice = plate.SalePrice * SalesPriceMultiplier;
            }

            var expected = plates.OrderByDescending(p => p.SalePrice).Take(PageSize);

            retrieved.CurrentPage.Should().Be(1);
            retrieved.TotalPages.Should().Be(2);
            retrieved.Plates.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public async Task CanSortPlatesByRegistrationDescending()
        {
            var plates = CreatePlates(25);
            await _context.AddRangeAsync(plates);
            await _context.SaveChangesAsync();

            var retrieved = await _repository.GetAllAsync(
                new QueryOptions { PageNumber = 1, OrderBy = SortOptions.RegistrationDescending });

            foreach (var plate in plates)
            {
                plate.SalePrice = plate.SalePrice * SalesPriceMultiplier;
            }

            var expected = plates.OrderByDescending(p => p.Registration).Take(PageSize);

            retrieved.CurrentPage.Should().Be(1);
            retrieved.TotalPages.Should().Be(2);
            retrieved.Plates.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }


        private List<Plate> CreatePlates(int number)
        {
            var plates = new List<Plate>();

            for (int i = 0; i < number; i++)
            {
                var plate = new Plate
                {
                    Id = Guid.NewGuid(),
                    Registration = i.ToString(),
                    Numbers = i,
                    SalePrice = 1000M - (i * 10)
                };

                plates.Add(plate);
            }

            return plates;
        }
    }
}
