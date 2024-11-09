using WebMVC.Application.DTO;

namespace WebMVC.Models;

public class Home
{
    public IEnumerable<PlateDto> Plates { get; set; }
    public string Initials { get; set; }
    public string Age { get; set; }
    public int PageNumber { get; set; } = 1;
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
    public string OrderBy { get; set; }
    public string DiscountCode { get; set; }
}