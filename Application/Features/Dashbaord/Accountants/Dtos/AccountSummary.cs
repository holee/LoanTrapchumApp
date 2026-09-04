using Application.Features.Dashbaord.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Dashbaord.Accountants.Dtos;

public record AccountSummary(IEnumerable<AccountantLoanSummary> LoanSummaries, IEnumerable<LoanRequestSummary> RequestSummaries);
