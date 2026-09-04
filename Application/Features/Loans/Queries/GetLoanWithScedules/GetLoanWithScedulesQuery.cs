using Application.Features.Loans.Dtos.Responses;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Loans.Queries.GetLoanWithScedules;

public record GetLoanWithScedulesQuery(int? BranchId, string? LoanId) : IRequest<ErrorOr<IEnumerable<LoanResponse>>>;
