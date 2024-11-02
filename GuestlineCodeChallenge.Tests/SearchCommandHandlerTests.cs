using FluentAssertions;
using GuestlineCodeChallenge.Core.Commands.Search;
using GuestlineCodeChallenge.Tests.TestData;

namespace GuestlineCodeChallenge.Tests;

public class SearchCommandHandlerTests
{
    [Test]
    public async Task Should_Return_Proper_Availability_Ranges_For_Given_SearchCommand()
    {
        // Arrange
        var sut = new SearchCommandHandler(
            new MockBookingsDataProvider(TestDataSimple.Hotels, TestDataSimple.Bookings),
            new MockTimeProvider(new(new(2024, 6, 11), default, default))
        );

        // Act
        var result = await sut.HandleAsync(new("H1", 365, "SGL"));

        // Assert
        result
            .Should()
            .BeEquivalentTo(
                new SearchCommandResult(
                    // There are two "SGL" rooms and there is only one booking in that range.
                    [
                        new(new(new(2024, 6, 11), new(2024, 9, 1)), 2),
                        new(new(new(2024, 9, 2), new(2024, 9, 5)), 1),
                        new(new(new(2024, 9, 6), new(2025, 6, 11)), 2),
                    ]
                )
            );
    }
}
