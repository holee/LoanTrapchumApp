using Applications.Utils;
using Domains.Models.Loans;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.LoanRequests.Command.AddLoanRequestCollateral;

public record AddLoanRequestCollateralCommand(LoanCollateralRequest Request) : IRequest<ErrorOr<string>>;
