
using Applications.Exceptions;
using FluentValidation;
using FluentValidation.Results;
namespace Applications.Utils;

public sealed class SimpleMediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public SimpleMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        
        await ValidateRequestAsync(request,cancellationToken);
        
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

        var handler = _serviceProvider.GetService(handlerType);

        if (handler is null)
        {
            throw new MediatorException($"Handler not found for {request.GetType().Name}");
        }

        var methodInfo = handlerType.GetMethod("Handle");


        return await (Task<TResponse>)methodInfo?.Invoke(handler, new object[] { request, CancellationToken.None })!;

    }

    
    // --- Helper: FluentValidation Integration ---
    private async Task ValidateRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken=default)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(request.GetType());
        
        var validator = _serviceProvider.GetService(validatorType);
        
        if (validator is null)
            return; // no validator => skip validation

        var validateMethod = validatorType.GetMethod("ValidateAsync", new[] { request.GetType(), typeof(CancellationToken) });
       
        if (validateMethod is null)
            return;

        var validationTask = (Task)validateMethod.Invoke(validator, new object[] { request, cancellationToken })!;
      
        await validationTask;

        var result = validationTask.GetType().GetProperty("Result");
        
        var validationResult = (ValidationResult)result!.GetValue(validationTask)!;
        
        if (!validationResult.IsValid)
        {
            throw new MediatorException(validationResult);
        }
        
    }

}
