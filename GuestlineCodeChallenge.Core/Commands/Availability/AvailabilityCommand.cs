using GuestlineCodeChallenge.Core.Commands.Common;

namespace GuestlineCodeChallenge.Core.Commands.Availability;

public record AvailabilityCommand(string HotelId, DateRange DateRange, string RoomType) : ICommand;
