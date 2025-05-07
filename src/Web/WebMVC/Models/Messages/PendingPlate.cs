namespace WebMVC.Models.Messages
{
    public interface IPendingPlate
    {
        Guid PlateId { get; }
        DateTime Timestamp { get; }
    }

    public class PendingPlate : IPendingPlate
    {
        public Guid PlateId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
