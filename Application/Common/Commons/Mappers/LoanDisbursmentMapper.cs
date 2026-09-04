using Application.Features.LoanPayments.Requests;
using Application.Features.Loans.Dtos.Responses;

namespace Application.Commons.Mappers
{
    public static class LoanDisbursmentMapper
    {
        public static LoanPaymentDtoCreate? Create(this VLoanDisbursments? source, string? UserId,
            string? OfficialReceiptNo)
        {
            if (source == null) throw new ArgumentNullException(nameof(VLoanDisbursments));

            return new LoanPaymentDtoCreate
            {
                LoanRefId = source?.LoanId,

                CreditAgentRefId = source?.CreditAgentRefId,

                BranchRefId = source?.BranchId,

                CurrencyRefId = source?.CurrencyId,

                UserRefId = UserId,

                PayDate = DateTime.Now,

                CustomerName = source?.AltName,

                OfficialReceiptNo = OfficialReceiptNo
            };
        }
    }
}