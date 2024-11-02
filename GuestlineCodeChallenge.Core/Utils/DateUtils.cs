namespace GuestlineCodeChallenge.Core.Utils;

public static class DateUtils
{
    public static IEnumerable<DateOnly> EnumerateDays(DateOnly from, DateOnly to)
    {
        for (var date = from; date <= to; date = date.AddDays(1))
        {
            yield return date;
        }
    }
}
