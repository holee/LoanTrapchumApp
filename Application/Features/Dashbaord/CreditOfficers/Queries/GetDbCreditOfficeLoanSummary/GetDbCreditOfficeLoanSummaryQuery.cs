using Application.Features.Dashbaord.CreditOfficers.Dtos;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Dashbaord.CreditOfficers.Queries.GetDbCreditOfficeLoanSummary;

public record GetDbCreditOfficeLoanSummaryQuery(int? BranchId, string? UserId, DateTime? DueDate) : IRequest<ErrorOr<CoLoanSummary>>;
