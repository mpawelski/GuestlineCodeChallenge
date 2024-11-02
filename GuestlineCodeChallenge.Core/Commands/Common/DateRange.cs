namespace GuestlineCodeChallenge.Core.Commands.Common;

public record DateRange(DateOnly From, DateOnly? To = null)
{
    public override string ToString() =>
        To == null
            ? From.ToString("yyyyMMdd")
            : $"{From.ToString("yyyyMMdd")}-{To.Value.ToString("yyyyMMdd")}";
}
