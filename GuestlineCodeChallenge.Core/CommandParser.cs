using System.Text.RegularExpressions;
using GuestlineCodeChallenge.Core.Commands.Availability;
using GuestlineCodeChallenge.Core.Commands.Common;
using GuestlineCodeChallenge.Core.Commands.Search;

namespace GuestlineCodeChallenge.Core;

public partial class CommandParser
{
    [GeneratedRegex(@"Availability\((\w+),(\d+),(\w+)\)")]
    private static partial Regex AvailabilityCommandSingleDateRegex();

    [GeneratedRegex(@"Availability\((\w+),(\d+)-(\d+),(\w+)\)")]
    private static partial Regex AvailabilityCommandDateRangeRegex();

    [GeneratedRegex(@"Search\((\w+),(\d+),(\w+)\)")]
    private static partial Regex SearchCommandRegex();

    [GeneratedRegex(@"\s")]
    private static partial Regex WhitespaceRegex();

    /// <summary>
    /// Parses command.
    /// </summary>
    /// <returns>Returns command object or null if input string is invalid.</returns>
    public ICommand? Parse(string commandString)
    {
        commandString = WhitespaceRegex().Replace(commandString, "");

        return ParseAvailabilityCommandWithSingleDate(commandString) as ICommand
            ?? ParseAvailabilityCommandWithDateRange(commandString) as ICommand
            ?? ParseSearchCommand(commandString);
    }

    private static AvailabilityCommand? ParseAvailabilityCommandWithSingleDate(string inputString)
    {
        var match = AvailabilityCommandSingleDateRegex().Match(inputString);
        if (
            match
                is {
                    Success: true,
                    Groups: [_, var hotelIdGroup, var dateGroup, var roomTypeGroup]
                }
            && TryParseDateOnly(dateGroup.Value, out var date)
        )
        {
            return new AvailabilityCommand(
                hotelIdGroup.Value,
                new DateRange(date),
                roomTypeGroup.Value
            );
        }

        return null;
    }

    private static AvailabilityCommand? ParseAvailabilityCommandWithDateRange(string inputString)
    {
        var match = AvailabilityCommandDateRangeRegex().Match(inputString);
        if (
            match
                is {
                    Success: true,
                    Groups: [
                        _,
                        var hotelIdGroup,
                        var dateFromGroup,
                        var dateToGroup,
                        var roomTypeGroup,
                    ]
                }
            && TryParseDateOnly(dateFromGroup.Value, out var dateFrom)
            && TryParseDateOnly(dateToGroup.Value, out var dateTo)
        )
        {
            return new AvailabilityCommand(
                hotelIdGroup.Value,
                new DateRange(dateFrom, dateTo),
                roomTypeGroup.Value
            );
        }

        return null;
    }

    private static SearchCommand? ParseSearchCommand(string inputString)
    {
        var match = SearchCommandRegex().Match(inputString);
        if (
            match
                is {
                    Success: true,
                    Groups: [_, var hotelIdGroup, var daysToLookAheadGroup, var roomTypeGroup]
                }
            && int.TryParse(daysToLookAheadGroup.Value, out var daysToLookAhead)
        )
        {
            return new SearchCommand(hotelIdGroup.Value, daysToLookAhead, roomTypeGroup.Value);
        }

        return null;
    }

    private static bool TryParseDateOnly(string dateOnlyString, out DateOnly parsedDate) =>
        DateOnly.TryParseExact(dateOnlyString, "yyyyMMdd", out parsedDate);
}
