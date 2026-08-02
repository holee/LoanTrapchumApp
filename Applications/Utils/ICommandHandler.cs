namespace Applications.Utils;

/// <summary>
/// Handler interface for commands. Extends <see cref="IRequestHandler{TRequest,TResponse}"/>
/// so command handlers are automatically discovered by the assembly scanner and dispatched
/// by the mediator.
/// </summary>
/// <typeparam name="TCommand">The command type that implements <see cref="ICommand{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">The return type of the command.</typeparam>
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>;
