using GuestlineCodeChallenge.Core.Commands.Common;

namespace GuestlineCodeChallenge.Core.Commands.Search;

public record SearchCommand(string HotelId, int DaysToLookAhead, string RoomType) : ICommand;
