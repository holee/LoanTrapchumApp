using Application.Features.Dashbaord.Dto;

namespace Application.Features.Dashbaord.CreditOfficers.Dtos;

public sealed record CoLoanSummary(IEnumerable<DbCoLoanSummary> LoanSummaries, IEnumerable<LoanRequestSummary> RequestSummaries);
