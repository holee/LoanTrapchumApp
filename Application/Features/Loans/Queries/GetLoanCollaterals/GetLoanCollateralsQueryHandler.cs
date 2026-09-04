using Application.Features.Loans.Dtos;

namespace Application.Features.Loans.Queries.GetLoanCollaterals;

public sealed class GetLoanCollateralsQueryHandler : IRequestHandler<GetLoanCollateralsQuery, Pager<VLoanCollateral>>
{
    private readonly ILoanService _unitOfWork;
    public GetLoanCollateralsQueryHandler(ILoanService unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Pager<VLoanCollateral>> Handle(GetLoanCollateralsQuery request, CancellationToken cancellationToken)
    {

        return await _unitOfWork.GetLoanCollaterals(request.LoanId, request.BranchId, cancellationToken);
    }
}
