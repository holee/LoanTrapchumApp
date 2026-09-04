using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LoanRequests.Requests;

namespace Application.Features.LoanRequests.Command.ApprovedLoanRequest
{
    public class AprovedLoanDtoCreateValidator : AbstractValidator<AproveLoanDtoCreate>
    {
        public AprovedLoanDtoCreateValidator()
        {
            RuleFor(x => x.AprovedAmount).Cascade(CascadeMode.Stop)
                                       .NotEmpty()
                                            .WithMessage("Approved Amount is needed.")
                                       .GreaterThan(0)
                                            .WithMessage("Approved Amount is bigger than 0.");
            RuleFor(x => x.LoanRequestId).Cascade(CascadeMode.Stop)
                                      .NotEmpty()
                                        .WithMessage("Loan Request ID is needed.")
                                       .NotNull()
                                        .WithMessage("Loan Request ID cannot be null.");
            RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
                                      .NotEmpty().WithMessage("Branch ID is needed.")
                                      .GreaterThan(0).WithMessage("Branch ID must be greater than 0.");
        }

    }
}
