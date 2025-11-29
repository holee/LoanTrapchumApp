using Applications.Features.Branches.Interfaces;
using Applications.Utils;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Loans.Command.DeleteLoan;

public record DeleteLoanCommand(string LoanId,int BranchId) : IRequest<ErrorOr<bool>>;
