using Applications.Features.Dashbaord.Dto;

namespace Applications.Features.Dashbaord.CreditOfficers.Dtos;

public sealed record CoLoanSummary(IEnumerable<DbCoLoanSummary> LoanSummaries, IEnumerable<LoanRequestSummary> RequestSummaries);
