namespace Catalog.Domain.ValueObjects
{
    public class SalePrice
    {
        private readonly decimal _purchasePrice;
        private const decimal MarkupPercentage = 0.20m;

        public SalePrice(decimal purchasePrice)
        {
            _purchasePrice = purchasePrice;
        }

        public decimal Value => Math.Round(_purchasePrice * (1 + MarkupPercentage), 2);

        // Optionally, you could override ToString for formatting
        public override string ToString()
        {
            return Value.ToString("C"); // Formats as currency, e.g., "$120.00"
        }
    }
}