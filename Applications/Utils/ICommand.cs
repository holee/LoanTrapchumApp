namespace Applications.Utils;

/// <summary>
/// Marker interface for commands (write operations that mutate state).
/// Extends <see cref="IRequest{TResponse}"/> so commands flow through the
/// <see cref="IMediator"/> / <see cref="SimpleMediator"/> pipeline.
/// </summary>
public interface ICommand<TResponse> : IRequest<TResponse>;
