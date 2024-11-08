using GuestlineCodeChallenge.Core.Data.Models;

namespace GuestlineCodeChallenge.Tests.TestData;

public static class TestDataUnavailable
{
    public static IReadOnlyList<Hotel> Hotels { get; } =
        [
            new()
            {
                Id = "H1",
                Name = "Hotel California",
                RoomTypes =
                [
                    new()
                    {
                        Code = "SGL",
                        Description = "Single Room",
                        Amenities = ["WiFi", "TV"],
                        Features = ["Non-smoking"],
                    },
                    new()
                    {
                        Code = "DBL",
                        Description = "Double Room",
                        Amenities = ["WiFi", "TV", "Minibar"],
                        Features = ["Non-smoking", "Sea View"],
                    },
                ],
                Rooms =
                [
                    new() { RoomType = "SGL", RoomId = "101" },
                    new() { RoomType = "SGL", RoomId = "102" },
                    new() { RoomType = "DBL", RoomId = "201" },
                    new() { RoomType = "DBL", RoomId = "202" },
                ],
            },
        ];

    public static IReadOnlyList<Booking> Bookings { get; } =
        [
            new()
            {
                HotelId = "H1",
                Arrival = new(2024, 9, 1),
                Departure = new(2024, 9, 3),
                RoomType = "DBL",
                RoomRate = "Prepaid",
            },
            new()
            {
                HotelId = "H1",
                Arrival = new(2025, 9, 2),
                Departure = new(2025, 9, 5),
                RoomType = "SGL",
                RoomRate = "Standard",
            },
            new()
            {
                HotelId = "H1",
                Arrival = new(2025, 5, 1),
                Departure = new(2025, 5, 17),
                RoomType = "SGL",
                RoomRate = "Standard",
            },
            new()
            {
                HotelId = "H1",
                Arrival = new(2025, 5, 11),
                Departure = new(2025, 6, 05),
                RoomType = "SGL",
                RoomRate = "Standard",
            },
        ];
}
