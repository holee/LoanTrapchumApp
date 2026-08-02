namespace Applications.Utils;

/// <summary>
/// Marker interface for queries (read-only operations).
/// Extends <see cref="IRequest{TResponse}"/> so queries flow through the
/// <see cref="IMediator"/> / <see cref="SimpleMediator"/> pipeline.
/// </summary>
public interface IQuery<TResponse> : IRequest<TResponse>;
