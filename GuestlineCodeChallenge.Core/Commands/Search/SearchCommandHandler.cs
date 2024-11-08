using GuestlineCodeChallenge.Core.Commands.Common;
using GuestlineCodeChallenge.Core.Data;
using GuestlineCodeChallenge.Core.Utils;

namespace GuestlineCodeChallenge.Core.Commands.Search;

public class SearchCommandHandler : BaseCommandHandler<SearchCommand, SearchCommandResult>
{
    private readonly IBookingsDataProvider _bookingsDataProvider;
    private readonly TimeProvider _timeProvider;

    public SearchCommandHandler(
        IBookingsDataProvider bookingsDataProvider,
        TimeProvider timeProvider
    )
    {
        _bookingsDataProvider = bookingsDataProvider;
        _timeProvider = timeProvider;
    }

    public override async Task<SearchCommandResult> HandleAsync(SearchCommand command)
    {
        var hotels = await _bookingsDataProvider.GetHotelsAsync();
        var bookings = await _bookingsDataProvider.GetBookingsAsync();

        var hotel = hotels.FirstOrDefault(h => h.Id == command.HotelId);
        if (hotel == null)
        {
            throw new Exception("Invalid hotel ID.");
        }

        var (todayDate, _, _) = _timeProvider.GetUtcNow();
        var dateTo = todayDate.AddDays(command.DaysToLookAhead);

        var bookingsInDateRange = bookings
            .FilterBookingsInDateRange(command.HotelId, command.RoomType, todayDate, dateTo)
            .ToList();

        var roomsCount = hotel.Rooms.Count(r => r.RoomType == command.RoomType);
        var availabilitiesForFutureDays = DateUtils
            .EnumerateDays(todayDate, dateTo)
            .Select(date =>
            {
                var bookedRoomsForThatDay = bookingsInDateRange.Count(b =>
                    date >= b.Arrival && date <= b.Departure
                );
                var availability = roomsCount - bookedRoomsForThatDay;
                return (date, availability);
            })
            .ToList();

        // We have list of availability numbers for each day.
        // Now we need to recognize when this number is the same for consecutive days.

        List<(int availability, DateOnly from, DateOnly? to)> tmpResult = [];
        var previous = availabilitiesForFutureDays.First();
        (int availability, DateOnly from, DateOnly? to) availabilityForDateRange = (
            previous.availability,
            previous.date,
            null
        );
        foreach (var current in availabilitiesForFutureDays.Skip(1))
        {
            if (current.availability == previous.availability)
            {
                // Same availability as in previous day. Extend the date range.
                availabilityForDateRange.to = current.date;
            }
            else
            {
                // Different availability than previous day. Save the availability for the date range.
                tmpResult.Add(availabilityForDateRange);
                availabilityForDateRange = (current.availability, current.date, null);
            }
            previous = current;
        }
        // Save last availability for date range
        tmpResult.Add(availabilityForDateRange);

        return new SearchCommandResult(
            tmpResult
                .Where(x => x.availability > 0)
                .Select(t => new DateRangeAvailability(new DateRange(t.from, t.to), t.availability))
                .ToList()
        );
    }
}
