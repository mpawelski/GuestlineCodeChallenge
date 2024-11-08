using GuestlineCodeChallenge.Core.Commands.Common;

namespace GuestlineCodeChallenge.Core.Commands.Search;

public record SearchCommandResult(List<DateRangeAvailability> DateRangeAvailabilities)
    : ICommandResult
{
    public override string ToString() => string.Join(", ", DateRangeAvailabilities);
}

public record DateRangeAvailability(DateRange DateRange, int Availability)
{
    public override string ToString() => $"({DateRange}, {Availability})";
}
