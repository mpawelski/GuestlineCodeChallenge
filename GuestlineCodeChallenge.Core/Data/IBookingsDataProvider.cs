using GuestlineCodeChallenge.Core.Data.Models;

namespace GuestlineCodeChallenge.Core.Data;

public interface IBookingsDataProvider
{
    Task<IReadOnlyList<Hotel>> GetHotelsAsync();
    Task<IReadOnlyList<Booking>> GetBookingsAsync();
}
