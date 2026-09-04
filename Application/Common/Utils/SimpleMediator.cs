using Application.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Utils;

/// <summary>
/// Lightweight mediator that dispatches <see cref="IRequest{TResponse}"/> objects
/// to their matching <see cref="IRequestHandler{TRequest,TResponse}"/> and integrates
/// FluentValidation into the pipeline.
/// </summary>
public sealed class SimpleMediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public SimpleMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        await ValidateRequestAsync(request, cancellationToken);

        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

        var handler = _serviceProvider.GetService(handlerType)
            ?? throw new MediatorException($"Handler not found for {request.GetType().Name}");

        var methodInfo = handlerType.GetMethod("Handle")
            ?? throw new MediatorException($"Handle method not found on handler for {request.GetType().Name}");

        // Pass the caller's cancellationToken through to the handler
        return await (Task<TResponse>)methodInfo.Invoke(handler, [request, cancellationToken])!;
    }

    /// <summary>
    /// Resolves and executes a FluentValidation validator (if registered) for the request.
    /// Throws <see cref="MediatorException"/> when validation fails.
    /// </summary>
    private async Task ValidateRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(request.GetType());

        var validator = _serviceProvider.GetService(validatorType);

        if (validator is null)
            return; // no validator => skip validation

        var validateMethod = validatorType.GetMethod("ValidateAsync", [request.GetType(), typeof(CancellationToken)]);

        if (validateMethod is null)
            return;

        var validationTask = (Task)validateMethod.Invoke(validator, [request, cancellationToken])!;

        await validationTask;

        var resultProperty = validationTask.GetType().GetProperty("Result");

        var validationResult = (ValidationResult)resultProperty!.GetValue(validationTask)!;

        if (!validationResult.IsValid)
        {
            throw new MediatorException(validationResult);
        }
    }
}
