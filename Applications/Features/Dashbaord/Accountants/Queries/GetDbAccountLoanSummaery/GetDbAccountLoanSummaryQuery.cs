using Applications.Features.Dashbaord.Accountants.Dtos;
using ErrorOr;

namespace Applications.Features.Dashbaord.Accountants.Queries.GetDbAccountLoanSummaery;

public record GetDbAccountLoanSummaryQuery(int? BranchId, DateTime? DueDate) : IRequest<Pager<AccountantLoanSummary>>; 