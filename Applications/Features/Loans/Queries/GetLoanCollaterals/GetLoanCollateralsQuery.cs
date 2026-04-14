using Applications.Features.Loans.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Loans.Queries.GetLoanCollaterals;

public sealed record GetLoanCollateralsQuery(string? LoanId, int? BranchId) : IRequest<Pager<VLoanCollateral>>;
