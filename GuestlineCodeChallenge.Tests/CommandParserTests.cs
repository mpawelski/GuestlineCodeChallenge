using FluentAssertions;
using GuestlineCodeChallenge.Core;
using GuestlineCodeChallenge.Core.Commands.Availability;
using GuestlineCodeChallenge.Core.Commands.Common;
using GuestlineCodeChallenge.Core.Commands.Search;

namespace GuestlineCodeChallenge.Tests;

public class CommandParserTests
{
    [Test]
    public void Should_Parse_AvailabilityCommand_When_Given_Proper_Input()
    {
        var sut = new CommandParser();
        var command = sut.Parse("Availability(H1, 20240901, SGL)");
        command
            .Should()
            .BeEquivalentTo(
                new AvailabilityCommand("H1", new DateRange(new DateOnly(2024, 9, 1)), "SGL")
            );
    }

    [Test]
    public void Should_Parse_AvailabilityCommand_With_DateRange_When_Given_Proper_Input()
    {
        var sut = new CommandParser();
        var command = sut.Parse("Availability(H1, 20240901-20240903, DBL)");
        command
            .Should()
            .BeEquivalentTo(
                new AvailabilityCommand(
                    "H1",
                    new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3)),
                    "DBL"
                )
            );
    }

    [Test]
    public void Should_Parse_SearchCommand_When_Given_Proper_Input()
    {
        var sut = new CommandParser();
        var command = sut.Parse("Search(H1, 365, SGL)");
        command.Should().BeEquivalentTo(new SearchCommand("H1", 365, "SGL"));
    }
}
