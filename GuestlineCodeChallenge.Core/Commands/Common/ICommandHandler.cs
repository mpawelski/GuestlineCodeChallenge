namespace GuestlineCodeChallenge.Core.Commands.Common;

public interface ICommandHandler
{
    bool CanHandle(ICommand command);
    Task<ICommandResult> HandleAsync(ICommand command);
}
