using GuestlineCodeChallenge;
using GuestlineCodeChallenge.Core;
using GuestlineCodeChallenge.Core.Commands.Availability;
using GuestlineCodeChallenge.Core.Commands.Common;
using GuestlineCodeChallenge.Core.Commands.Search;

if (args is not ["--hotels", var hotelsJsonPath, "--bookings", var bookingJsonPath])
{
    Console.WriteLine(
        """
        Error: Wrong parameters passed to program.
        Usage:
          GuestlineCodeChallenge --hotels hotels.json --bookings bookings.json
        """
    );
    Environment.Exit(1);
    return;
}

if (!Path.Exists(hotelsJsonPath))
{
    Console.WriteLine($"Error: Hotels path ({hotelsJsonPath}) does not exist.\".");
    Environment.Exit(1);
    return;
}

if (!Path.Exists(bookingJsonPath))
{
    Console.WriteLine($"Error: Bookings path ({bookingJsonPath}) does not exist.\".");
    Environment.Exit(1);
    return;
}

// Note: For simplicity, I decided here to create objects manually. In production code
// I would most likely use a DI library to register dependencies and resolve them.

// Note: For simplicity I put data access implementation in "App" layer. In production code
// I would probably create separate "Infrastructure" project where I would implement data access.

var jsonFileBookingsDataProvider = new JsonFileBookingsDataProvider(
    bookingJsonPath,
    hotelsJsonPath
);
var dataProvider = new InMemoryCacheBookingsDataProvider(jsonFileBookingsDataProvider);

var availabilityCommandHandler = new AvailabilityCommandHandler(dataProvider);
var searchCommandHandler = new SearchCommandHandler(dataProvider, TimeProvider.System);

var hotelReservationManager = new HotelReservationManager(
    [availabilityCommandHandler, searchCommandHandler]
);
var commandParser = new CommandParser();

while (true)
{
    var lineInput = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(lineInput))
    {
        Environment.Exit(0);
    }

    var command = commandParser.Parse(lineInput);
    if (command == null)
    {
        Console.WriteLine("Error: Invalid command. Try again.");
        continue;
    }

    // NOTE: instead of exceptions we could consider changing "ExecuteCommandAsync" return type to return some
    // type of "Result" object with information if command succeeded or not (with and error data if not).
    ICommandResult commandResponse;
    try
    {
        commandResponse = await hotelReservationManager.ExecuteCommandAsync(command);
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            $"Error: Error while executing command. Check if command is valid and try again. Error message: \"{ex.Message}\""
        );
        continue;
    }

    Console.WriteLine(commandResponse);
}
