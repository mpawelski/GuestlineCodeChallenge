using GuestlineCodeChallenge.Core.Data;
using GuestlineCodeChallenge.Core.Data.Models;

namespace GuestlineCodeChallenge;

// An implementation of IBookingsDataProvider that cache the loaded data.
//
// The requirements were not clear if we are allowed to read the json file
// every time a command is executed. And this is what will happen when we'll
// just use JsonFileBookingsDataProvider.
//
// Anyway I decided it's a good way to show how you can add such caching
// functionality with a simple use of Decorator pattern ;)
public class InMemoryCacheBookingsDataProvider(IBookingsDataProvider bookingsDataProvider)
    : IBookingsDataProvider
{
    private readonly Lazy<Task<IReadOnlyList<Hotel>>> _hotels =
        new(async () => await bookingsDataProvider.GetHotelsAsync());

    private readonly Lazy<Task<IReadOnlyList<Booking>>> _bookings =
        new(async () => await bookingsDataProvider.GetBookingsAsync());

    public Task<IReadOnlyList<Hotel>> GetHotelsAsync() => _hotels.Value;

    public Task<IReadOnlyList<Booking>> GetBookingsAsync() => _bookings.Value;
}
