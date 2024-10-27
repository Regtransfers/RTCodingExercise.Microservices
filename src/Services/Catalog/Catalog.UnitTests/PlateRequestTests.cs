using Catalog.DTO;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Catalog.UnitTests
{
    public class PlateRequestTests
    {
        [Fact]
        public void CanCreateValidPlate()
        {
            var request = new PlateCreateRequest
            {
                Registration = "AAA222",
                SalePrice = 7600,
                PurchasePrice = 1000,
                Letters = "AAA",
                Numbers = 222,
            };

            var validatorResults = CheckModelIsValid(request);

            validatorResults.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CannotCreatePlateWithMissingRegistration(string reg)
        {
            var request = new PlateCreateRequest
            {
                Registration = reg,
                SalePrice = 7600,
                PurchasePrice = 1000,
                Letters = "AAA",
                Numbers = 222,
            };

            var validatorResults = CheckModelIsValid(request);

            validatorResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("The Registration field is required.");
        }

        [Theory]
        [InlineData("AAA 222")]
        [InlineData("AAA!222")]
        [InlineData("AAAA2222")]
        public void CannotCreatePlateWithInvalidRegistration(string reg)
        {
            var request = new PlateCreateRequest
            {
                Registration = reg,
                SalePrice = 7600,
                PurchasePrice = 1000,
                Letters = "AAA",
                Numbers = 222,
            };

            var validatorResults = CheckModelIsValid(request);

            validatorResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Registration should be only numbers and letters and must be 7 characters or less.");
        }

        [Fact]
        public void SalePriceCannotBeLessThanZero()
        {
            var request = new PlateCreateRequest
            {
                Registration = "AAA222",
                SalePrice = -5,
                PurchasePrice = 1000,
                Letters = "AAA",
                Numbers = 222,
            };

            var validatorResults = CheckModelIsValid(request);

            validatorResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("SalePrice must be zero or greater.");
        }

        [Fact]
        public void PurchasePriceCannotBeLessThanZero()
        {
            var request = new PlateCreateRequest
            {
                Registration = "AAA222",
                SalePrice = 7600,
                PurchasePrice = -10,
                Letters = "AAA",
                Numbers = 222,
            };

            var validatorResults = CheckModelIsValid(request);

            validatorResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("PurchasePrice must be zero or greater.");
        }


        [Theory]
        [InlineData("111")]
        [InlineData("AABB")]
        [InlineData("AB$")]
        public void CannotCreatePlateWithInvalidLetters(string letters)
        {
            var request = new PlateCreateRequest
            {
                Registration = "AAA222",
                SalePrice = 7600,
                PurchasePrice = 1000,
                Letters = letters,
                Numbers = 222,
            };

            var validatorResults = CheckModelIsValid(request);

            validatorResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Letters should be only letters and must be 3 characters or less.");
        }

        [Theory]
        [InlineData(1111)]
        [InlineData(-5)]
        public void CannotCreatePlateWithInvalidNumbers(int numbers)
        {
            var request = new PlateCreateRequest
            {
                Registration = "AAA222",
                SalePrice = 7600,
                PurchasePrice = 1000,
                Letters = "AAA",
                Numbers = numbers,
            };

            var validatorResults = CheckModelIsValid(request);

            validatorResults.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("The field Numbers must be between 0 and 999.");
        }

        private List<ValidationResult> CheckModelIsValid(PlateCreateRequest request)
        {
            var context = new ValidationContext(request);

            var result = new List<ValidationResult>();

            Validator.TryValidateObject(request, context, result, true);

            return result;
        }
    }
}
