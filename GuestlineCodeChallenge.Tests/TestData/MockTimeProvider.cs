namespace GuestlineCodeChallenge.Tests.TestData;

public class MockTimeProvider(DateOnly date) : TimeProvider
{
    public override DateTimeOffset GetUtcNow() => new(date, default, default);
}
