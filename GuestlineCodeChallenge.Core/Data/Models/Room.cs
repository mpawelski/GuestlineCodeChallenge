namespace GuestlineCodeChallenge.Core.Data.Models;

public record Room
{
    public required string RoomType { get; init; }
    public required string RoomId { get; init; }
}
