using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;
using FluentValidation;

namespace Application.Features.Activities.Command.DeleteActivity;

public record DeleteActivityCommand(int? Id, string CustomerId) : IRequest<ErrorOr<bool>>;



public sealed class DeleteActivityCommandHandler : IRequestHandler<DeleteActivityCommand, ErrorOr<bool>>
{
    private readonly ICustomerActivityService _activityService;
    public DeleteActivityCommandHandler(ICustomerActivityService activityService)
    {
        _activityService = activityService;
    }
    public async Task<ErrorOr<bool>> Handle(DeleteActivityCommand request, CancellationToken cancellationToken = default)
    {
        return await _activityService.DeleteActivityAsync(request.Id, request.CustomerId, cancellationToken);
    }
}



public sealed class DeleteActivityCommandValidator : AbstractValidator<DeleteActivityCommand>
{
    public DeleteActivityCommandValidator()
    {
        RuleFor(x => x.Id).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Activity Id is required.")
            .GreaterThan(0).WithMessage("Activity Id must be greater than zero.");
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer Id is required.");
    }
}

