using GuestlineCodeChallenge.Core.Commands.Common;

namespace GuestlineCodeChallenge.Core;

public class HotelReservationManager
{
    private readonly IEnumerable<ICommandHandler> _commandHandlers;

    public HotelReservationManager(IEnumerable<ICommandHandler> commandHandlers)
    {
        _commandHandlers = commandHandlers;
    }

    public async Task<ICommandResult> ExecuteCommandAsync(ICommand command)
    {
        foreach (var handler in _commandHandlers)
        {
            if (handler.CanHandle(command))
            {
                return await handler.HandleAsync(command);
            }
        }

        throw new Exception("Unsupported command type.");
    }
}
