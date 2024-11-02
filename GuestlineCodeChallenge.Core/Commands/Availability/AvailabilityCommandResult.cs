using GuestlineCodeChallenge.Core.Commands.Common;

namespace GuestlineCodeChallenge.Core.Commands.Availability;

public record AvailabilityCommandResult(int Availability) : ICommandResult
{
    public override string ToString() => Availability.ToString();
}
