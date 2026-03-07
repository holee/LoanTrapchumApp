using Applications.Features.Dashbaord.CreditOfficers.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Dashbaord.CreditOfficers.Queries.GetDbCreditOfficeLoanSummary;

public record GetDbCreditOfficeLoanSummaryQuery(int? BranchId, string? UserId, DateTime? DueDate) : IRequest<Pager<DbCoLoanSummary>>;
