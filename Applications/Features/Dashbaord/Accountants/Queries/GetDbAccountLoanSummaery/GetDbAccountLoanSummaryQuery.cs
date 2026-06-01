using ErrorOr;

namespace Applications.Features.Dashbaord.Accountants.Queries.GetDbAccountLoanSummaery;

public record GetDbAccountLoanSummaryQuery(int? BranchId, DateTime? DueDate) : IRequest<ErrorOr<AccountSummary>>;  