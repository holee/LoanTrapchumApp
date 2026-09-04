namespace Application.Utils;

/// <summary>
/// Handler interface for queries. Extends <see cref="IRequestHandler{TRequest,TResponse}"/>
/// so query handlers are automatically discovered by the assembly scanner and dispatched
/// by the mediator.
/// </summary>
/// <typeparam name="TQuery">The query type that implements <see cref="IQuery{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">The return type of the query.</typeparam>
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>;