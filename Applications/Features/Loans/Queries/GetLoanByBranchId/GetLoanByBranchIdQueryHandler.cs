namespace Applications.Features.Loans.Queries.GetLoanByBranchId;

public sealed class GetLoanByBranchIdQueryHandler(ILoanService loanService) : IRequestHandler<GetLoanByBranchIdQuery, Pager<VLoanDisbursments>>
{
    public async Task<Pager<VLoanDisbursments>> Handle(GetLoanByBranchIdQuery request, CancellationToken cancellationToken = default)
    {
        return await loanService.GetLoansByBranchId(request.BranchId, request.UserId, request.State, request.Start, request.To, request.Param, cancellationToken);
    }
}
