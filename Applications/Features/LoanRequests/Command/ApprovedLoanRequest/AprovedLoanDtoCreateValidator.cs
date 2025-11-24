using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.Features.LoanRequests.Requests;

namespace Applications.Features.LoanRequests.Command.ApprovedLoanRequest
{
    public class AprovedLoanDtoCreateValidator : AbstractValidator<AproveLoanDtoCreate>
    {
        public AprovedLoanDtoCreateValidator()
        {
            RuleFor(x => x.AprovedAmount).Cascade(CascadeMode.Stop)
                                       .NotEmpty().WithMessage("Approved Amount is needed.")
                                       .GreaterThan(0).WithMessage("Approved Amount is bigger than 0.");
            RuleFor(x => x.LoanRequestId).Cascade(CascadeMode.Stop)
                                      .NotEmpty().WithMessage("Approved Amount is needed.");
            RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
                                      .NotEmpty().WithMessage("Approved Amount is needed.")
                                      .GreaterThan(0).WithMessage("Approved Amount is bigger than 0.");
        }

    }
}
