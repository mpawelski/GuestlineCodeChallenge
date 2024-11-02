using GuestlineCodeChallenge.Core.Commands.Common;
using GuestlineCodeChallenge.Core.Data;
using GuestlineCodeChallenge.Core.Utils;

namespace GuestlineCodeChallenge.Core.Commands.Availability;

public class AvailabilityCommandHandler
    : BaseCommandHandler<AvailabilityCommand, AvailabilityCommandResult>
{
    private readonly IBookingsDataProvider _bookingsDataProvider;

    public AvailabilityCommandHandler(IBookingsDataProvider bookingsDataProvider)
    {
        _bookingsDataProvider = bookingsDataProvider;
    }

    public override async Task<AvailabilityCommandResult> HandleAsync(AvailabilityCommand command)
    {
        var hotels = await _bookingsDataProvider.GetHotelsAsync();
        var bookings = await _bookingsDataProvider.GetBookingsAsync();

        var hotel = hotels.FirstOrDefault(h => h.Id == command.HotelId);
        if (hotel == null)
        {
            throw new Exception("Invalid hotel ID.");
        }

        var dateFrom = command.DateRange.From;
        var dateTo = command.DateRange.To ?? dateFrom;

        var bookingsInDateRange = bookings
            .FilterBookingsInDateRange(command.HotelId, command.RoomType, dateFrom, dateTo)
            .ToList();

        var roomsCount = hotel.Rooms.Count(r => r.RoomType == command.RoomType);
        var availabilityForDateRange = DateUtils
            .EnumerateDays(dateFrom, dateTo)
            .Select(date =>
            {
                var bookedRoomsForThatDay = bookingsInDateRange.Count(b =>
                    date >= b.Arrival && date <= b.Departure
                );
                return roomsCount - bookedRoomsForThatDay;
            })
            .Min();

        return new(availabilityForDateRange);
    }
}
