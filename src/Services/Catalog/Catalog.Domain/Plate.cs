using System;
using System.Collections.Generic;

namespace Catalog.Domain
{
    public class Plate
    {
        private const decimal DefaultMarkupPercentage = 1.2M; // Default markup percentage for sale price

        public Guid Id { get; set; } // Unique identifier for the plate
        public string? Registration { get; set; } // Registration number of the plate
        public decimal PurchasePrice { get; set; } // Initial purchase price

        // Sale price calculated dynamically based on markup percentage
        public decimal SalePrice => PurchasePrice * DefaultMarkupPercentage;

        public string? Letters { get; set; } // Letters part of the plate
        public int Numbers { get; set; } // Numeric part of the plate

        public bool IsReserved { get; set; } // Indicates if the plate is reserved
        public bool IsSold { get; set; } // Indicates if the plate has been sold

        public ICollection<Sale> Sales { get; set; } = new List<Sale>(); // Initialize to avoid null checks

        // Reserve the plate if it is not sold or already reserved
        public bool Reserve()
        {
            if (IsSold) return false;
            if (IsReserved) return false;
            IsReserved = true;
            return true;
        }

        // Sell the plate if it is not reserved or sold
        public bool Sell()
        {
            if (IsReserved || IsSold) return false;
            IsSold = true;
            return true;
        }
    }

    public class Sale
    {
        public int Id { get; set; } // Unique identifier for the sale
        public Guid PlateId { get; set; } // Foreign key reference to Plate
        public Plate Plate { get; set; } // Navigation property to the associated plate

        public decimal PurchasePrice { get; set; } // Purchase price of the plate
        public decimal SalePrice { get; set; } // Actual sale price

        // Profit margin calculated based on sale price and purchase price
        public decimal ProfitMargin => Math.Max(SalePrice - PurchasePrice, 0); // Ensures no negative margin

        public DateTime SaleDate { get; set; } // Date the plate was sold
    }
}
