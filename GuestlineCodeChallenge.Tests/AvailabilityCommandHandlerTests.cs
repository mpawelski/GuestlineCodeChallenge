using FluentAssertions;
using GuestlineCodeChallenge.Core.Commands.Availability;
using GuestlineCodeChallenge.Tests.TestData;

namespace GuestlineCodeChallenge.Tests;

public class AvailabilityCommandHandlerTests
{
    [Test]
    public async Task Should_Return_All_Rooms_Availability_When_There_Are_No_Bookings_For_That_Date()
    {
        // Arrange
        var sut = new AvailabilityCommandHandler(
            new MockBookingsDataProvider(TestDataSimple.Hotels, TestDataSimple.Bookings)
        );

        // Act
        var result = await sut.HandleAsync(
            new AvailabilityCommand("H1", new(new(2024, 9, 1)), "SGL")
        );

        // Assert
        // Both rooms are available for that date
        result.Should().BeEquivalentTo(new AvailabilityCommandResult(2));
    }

    [Test]
    public async Task Should_Return_Correct_Rooms_Availability_When_There_Are_Some_Bookings_For_That_Date()
    {
        // Arrange
        var sut = new AvailabilityCommandHandler(
            new MockBookingsDataProvider(TestDataSimple.Hotels, TestDataSimple.Bookings)
        );

        // Act
        var result = await sut.HandleAsync(
            new AvailabilityCommand("H1", new(new(2024, 9, 2)), "SGL")
        );

        // Assert
        // One room is booked on 2024.09.02 - 2024.09.05, the second one is available
        result.Should().BeEquivalentTo(new AvailabilityCommandResult(1));
    }

    [Test]
    public async Task Should_Return_Correct_Rooms_Availability_When_Provided_Date_Range_And_There_Are_Some_Bookings_In_That_Range()
    {
        // Arrange
        var sut = new AvailabilityCommandHandler(
            new MockBookingsDataProvider(TestDataSimple.Hotels, TestDataSimple.Bookings)
        );

        // Act
        var result = await sut.HandleAsync(
            new AvailabilityCommand("H1", new(new(2024, 9, 1), new(2024, 9, 3)), "SGL")
        );

        // Assert
        // One room is booked on 2024.09.02 - 2024.09.05 date range which overlaps with requested date range, the second one is available.
        result.Should().BeEquivalentTo(new AvailabilityCommandResult(1));
    }

    [Test]
    public async Task Should_Return_Negative_Rooms_Availability_When_Provided_Date_Range_Is_Range_Of_Overbooked_Rooms()
    {
        // Arrange
        var sut = new AvailabilityCommandHandler(
            new MockBookingsDataProvider(TestDataOverBookings.Hotels, TestDataOverBookings.Bookings)
        );

        // Act
        var result = await sut.HandleAsync(
            new AvailabilityCommand("H1", new(new(2024, 9, 2), new(2024, 9, 3)), "SGL")
        );

        // Assert
        // There are three booking in range 2024.09.02 - 2024.09.03 but are only 2 rooms of that type.
        result.Should().BeEquivalentTo(new AvailabilityCommandResult(-1));
    }

    [Test]
    public async Task Should_Return_Negative_Rooms_Availability_When_Provided_Date_Range_Overlap_With_Overbooked_Rooms()
    {
        // Arrange
        var sut = new AvailabilityCommandHandler(
            new MockBookingsDataProvider(TestDataOverBookings.Hotels, TestDataOverBookings.Bookings)
        );

        // Act
        var result = await sut.HandleAsync(
            new AvailabilityCommand("H1", new(new(2024, 9, 3), new(2024, 9, 6)), "SGL")
        );

        // Assert
        // There are three booking that overlaps with range 2024.09.03 - 2024.09.06 (day 2024.09.03 is in all of them) but are only 2 rooms of that type.
        result.Should().BeEquivalentTo(new AvailabilityCommandResult(-1));
    }
}
