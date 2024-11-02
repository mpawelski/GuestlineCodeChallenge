namespace GuestlineCodeChallenge.Core.Data.Models;

public record RoomType
{
    public required string Code { get; init; }
    public required string Description { get; init; }
    public required IReadOnlyList<string> Amenities { get; init; }
    public required IReadOnlyList<string> Features { get; init; }
}
