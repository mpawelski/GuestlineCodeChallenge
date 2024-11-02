using GuestlineCodeChallenge.Core.Data;
using GuestlineCodeChallenge.Core.Data.Models;

namespace GuestlineCodeChallenge.Tests.TestData;

public class MockBookingsDataProvider(IReadOnlyList<Hotel> hotels, IReadOnlyList<Booking> bookings)
    : IBookingsDataProvider
{
    public Task<IReadOnlyList<Hotel>> GetHotelsAsync() => Task.FromResult(hotels);

    public Task<IReadOnlyList<Booking>> GetBookingsAsync() => Task.FromResult(bookings);
}
