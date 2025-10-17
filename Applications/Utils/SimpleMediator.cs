using Applications.Exceptions;

namespace Applications.Utils;

public sealed class SimpleMediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public SimpleMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

        var handler = _serviceProvider.GetService(handlerType);

        if (handler is null)
        {
            throw new MediatorException($"Handler not found for {request.GetType().Name}");
        }

        var methodInfo = handlerType.GetMethod("Handle");


        return (Task<TResponse>)methodInfo?.Invoke(handler, new object[] { request, CancellationToken.None })!;

    }


}
