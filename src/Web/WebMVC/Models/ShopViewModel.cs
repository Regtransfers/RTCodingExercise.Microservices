using Catalog.Domain;

namespace WebMVC.Models
{
    public class ShopViewModel
    {
        public IEnumerable<Plate> Plates { get; set; } = Enumerable.Empty<Plate>();
        public decimal SalesMultiplier { get; set; }
    }
}
