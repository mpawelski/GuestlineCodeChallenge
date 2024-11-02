namespace GuestlineCodeChallenge.Core.Data.Models;

public record Booking
{
    public required string HotelId { get; init; }
    public required DateOnly Arrival { get; init; }
    public required DateOnly Departure { get; init; }
    public required string RoomType { get; init; }
    public required string RoomRate { get; init; }
}
