using ErrorOr;

namespace Applications.Features.Settings.Queries.GetApprovedUsers;

public sealed class GetApprovedUserQueryHandler : IRequestHandler<GetApprovedUserQuery, ErrorOr<IEnumerable<VUserLoanSizes>>>
{
    private readonly ILoanSettingService _loanSettingService;
    public GetApprovedUserQueryHandler(ILoanSettingService loanSettingService)
    {
        _loanSettingService = loanSettingService;
    }
    public async Task<ErrorOr<IEnumerable<VUserLoanSizes>>> Handle(GetApprovedUserQuery request, CancellationToken cancellationToken)
    {

        var param = request.Param ?? new QueryParam();

        var result = await _loanSettingService.GetAprovedUserAsync(request.BranchId, request.CurrencyId, request.Amount, param);

        return result.ToList();
    }
}
