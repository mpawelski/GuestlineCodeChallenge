namespace GuestlineCodeChallenge.Core.Commands.Common;

/// <summary>
/// Helper base class for implementing ICommandHandler interface. With it, you have
/// strongly typed "Handle" method, and you don't need to implement "CanHandle" method.
/// </summary>
public abstract class BaseCommandHandler<TCommand, TCommandResult> : ICommandHandler
    where TCommand : ICommand
    where TCommandResult : ICommandResult
{
    public bool CanHandle(ICommand command) => typeof(TCommand) == command.GetType();

    async Task<ICommandResult> ICommandHandler.HandleAsync(ICommand command) =>
        await HandleAsync((TCommand)command);

    public abstract Task<TCommandResult> HandleAsync(TCommand command);
}
