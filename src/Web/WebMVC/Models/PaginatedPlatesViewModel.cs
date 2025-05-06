using Catalog.Domain;

namespace WebMVC.Models
{
    public class PaginatedPlatesViewModel
    {
        public IEnumerable<ViewPlate> Plates { get; set; } = Enumerable.Empty<ViewPlate>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public decimal SalesMultiplier { get; set; }

        public Plate NewPlate { get; set; } = new();
        public string SortBy { get; set; } = "Registration";
        public string SortOrder { get; set; } = "asc";
        public string Filter { get; set; }
        public string Status { get; set; }
    }
}
