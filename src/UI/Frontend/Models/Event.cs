namespace Frontend.Models;

public class Event
{
    public Guid Id { get; set; }
    public string EventName { get; set; } = null!;
    public string EventType { get; set; }
    public string EventLocation { get; set; }
    public string EventCountry { get; set; }
    public DateTime EventDate { get; set; }
    public int EventStartHour { get; set; }
    public int EventEndHour { get; set; }
    public int TotalSeatingCapacity { get; set; }
    public int SeatsBooked { get; set; }
    public decimal TicketPriceInUsd { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid ArtistId { get; set; }
}