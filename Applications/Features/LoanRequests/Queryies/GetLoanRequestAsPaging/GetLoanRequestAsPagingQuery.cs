using Applications.Enumerations;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestAsPaging;

public record GetLoanRequestAsPagingQuery(string? UserId, int? BranchId, RequestStatus RequestStatus, QueryParam Param): IRequest<Pager<VLoanRequests>>;
