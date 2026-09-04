using ErrorOr;

namespace Application.Features.Loans.Command.ApproveEditLoanRequest;

public sealed class ApproveEditLoanRequestCommandHandler(ILoanService service) : IRequestHandler<ApproveEditLoanRequestCommand, ErrorOr<AppResult>>
{
    public async Task<ErrorOr<AppResult>> Handle(ApproveEditLoanRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
           return await service.ApproveEditLoanRequestAsync(request.Reqeust,cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Unexpected(description: e.Message);
        }
        
    }
}