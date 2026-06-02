using Applications.Features.Dashbaord.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Dashbaord.Accountants.Dtos;

public record AccountSummary(IEnumerable<AccountantLoanSummary> LoanSummaries, IEnumerable<LoanRequestSummary> RequestSummaries);
