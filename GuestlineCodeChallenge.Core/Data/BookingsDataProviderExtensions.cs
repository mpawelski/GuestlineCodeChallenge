using GuestlineCodeChallenge.Core.Data.Models;

namespace GuestlineCodeChallenge.Core.Data;

public static class BookingsDataProviderExtensions
{
    /// <summary>
    /// Filters list of <see cref="Booking"/> to list of booking which date range intersect with provided range.
    /// </summary>
    public static IEnumerable<Booking> FilterBookingsInDateRange(
        this IEnumerable<Booking> bookings,
        string hotelId,
        string roomType,
        DateOnly dateFrom,
        DateOnly dateTo
    )
    {
        return bookings.Where(b =>
            b.HotelId == hotelId
            && b.RoomType == roomType
            && (
                (b.Arrival >= dateFrom && b.Arrival <= dateTo)
                || (b.Departure >= dateFrom && b.Departure <= dateTo)
            )
        );
    }
}
