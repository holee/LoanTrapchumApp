using Applications.Features.Reports.Dtos;
using Applications.Interfaces;
using ErrorOr;

namespace Applications.Features.Reports.Queries.PrintLoanScheduleDetails;

public class PrintLoanScheduleDetailsQueryHandler : IRequestHandler<PrintLoanScheduleDetailsQuery, ErrorOr<RptScheduleDetails?>>
{
    private readonly IPrintingLoanService _reportService;
    public PrintLoanScheduleDetailsQueryHandler(IPrintingLoanService reportService)
    {
        _reportService = reportService;
    }
    public async Task<ErrorOr<RptScheduleDetails?>> Handle(PrintLoanScheduleDetailsQuery request, CancellationToken cancellationToken)
    {
        var result = await _reportService.PrintScheduleDetailsAsync(request.LoanId, request.BranchId, cancellationToken);

        if (result is null)
        {
            return Error.NotFound("Loan schedule details not found.");
        }

        return result;
    }
}
