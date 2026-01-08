namespace EventBookings.Frontend.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string MobileNumber { get; set; }
    public string FullName { get; set; }
    public string ResidenceCountryCode { get; set; }
    public string TimeZone { get; set; }
    public List<Event> FavoriteEvents { get; set; }
    public List<Event> BookedEvents { get; set; }
}