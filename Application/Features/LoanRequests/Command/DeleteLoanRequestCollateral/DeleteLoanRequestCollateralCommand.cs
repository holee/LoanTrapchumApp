using Application.Utils;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LoanRequests.Command.DeleteLoanRequestCollateral;

public record DeleteLoanRequestCollateralCommand(int? BranchId, string? LoanRequestId, long? CollateralLinkId)
      : IRequest<ErrorOr<string>>;
