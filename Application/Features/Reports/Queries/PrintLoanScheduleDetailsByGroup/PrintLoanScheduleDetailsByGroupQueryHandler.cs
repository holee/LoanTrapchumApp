using Application.Features.Reports.Dtos;
using Application.Interfaces;
using ErrorOr;

namespace Application.Features.Reports.Queries.PrintLoanScheduleDetailsByGroup;

public class PrintLoanScheduleDetailsByGroupQueryHandler : IRequestHandler<PrintLoanScheduleDetailsByGroupQuery, ErrorOr<RptScheduleDetails?>>
{
    private readonly IPrintingLoanService _reportService;
    public PrintLoanScheduleDetailsByGroupQueryHandler(IPrintingLoanService reportService)
    {
        _reportService = reportService;
    }
    public async Task<ErrorOr<RptScheduleDetails?>> Handle(PrintLoanScheduleDetailsByGroupQuery request, CancellationToken cancellationToken)
    {
        var result = await _reportService.PrintScheduleDetailsAsync(request.LoanId, request.BranchId, request.ScheduleGroup, cancellationToken);

        if (result is null)
        {
            return Error.NotFound("Loan schedule details not found.");
        }

        return result;
    }
}
