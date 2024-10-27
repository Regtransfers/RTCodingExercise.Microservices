using Catalog.Domain;

namespace Catalog.DTO
{
    public class PlateListResponse
    {
        public List<Plate> Plates { get; set; } = new List<Plate>();

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public bool HasNext { get; set; }

        public bool HasPrevious { get; set; }
    }
}
