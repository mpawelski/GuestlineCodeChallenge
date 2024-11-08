using FluentAssertions;
using GuestlineCodeChallenge.Core.Commands.Search;
using GuestlineCodeChallenge.Tests.TestData;

namespace GuestlineCodeChallenge.Tests;

public class SearchCommandHandlerTests
{
    [Test]
    public async Task Should_Return_Proper_Availability_Ranges()
    {
        // Arrange
        var sut = new SearchCommandHandler(
            new MockBookingsDataProvider(TestDataSimple.Hotels, TestDataSimple.Bookings),
            new MockTimeProvider(new(2024, 6, 11))
        );

        // Act
        var result = await sut.HandleAsync(new("H1", 365, "SGL"));

        // Assert
        result
            .Should()
            .BeEquivalentTo(
                new SearchCommandResult(
                    // There are two "SGL" rooms and there is one booking in the middle of the provided range.
                    [
                        new(new(new(2024, 6, 11), new(2024, 9, 1)), 2),
                        new(new(new(2024, 9, 2), new(2024, 9, 5)), 1),
                        new(new(new(2024, 9, 6), new(2025, 6, 11)), 2),
                    ]
                )
            );
    }

    [Test]
    public async Task Should_Return_Availability_Ranges_Without_Ranges_Where_There_Is_No_Availability()
    {
        // Arrange
        var sut = new SearchCommandHandler(
            new MockBookingsDataProvider(TestDataUnavailable.Hotels, TestDataUnavailable.Bookings),
            new MockTimeProvider(new(2024, 11, 8))
        );

        // Act
        var result = await sut.HandleAsync(new("H1", 365, "SGL"));

        // Assert
        result
            .Should()
            .BeEquivalentTo(
                new SearchCommandResult(
                    // There are two bookings in May that overlap causing 0 availability for some period of time
                    [
                        new(new(new(2024, 11, 8), new(2025, 4, 30)), 2),
                        new(new(new(2025, 5, 1), new(2025, 5, 10)), 1),
                        // > here there is zero availability for 2025.05.11 - 2025.05.17
                        new(new(new(2025, 5, 18), new(2025, 6, 5)), 1),
                        new(new(new(2025, 6, 6), new(2025, 9, 1)), 2),
                        new(new(new(2025, 9, 2), new(2025, 9, 5)), 1),
                        new(new(new(2025, 9, 6), new(2025, 11, 8)), 2),
                    ]
                )
            );
    }
}
