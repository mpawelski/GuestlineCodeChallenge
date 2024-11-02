namespace GuestlineCodeChallenge.Tests.TestData;

public class MockTimeProvider(DateTimeOffset utcNow) : TimeProvider
{
    public override DateTimeOffset GetUtcNow() => utcNow;
}
