using ActualLab.Serialization.Internal;
using Applications.Requests.Loans.Payments;
using Applications.Responses.ViewModels.Loans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Commons.Mappers
{
    public static class LoanDisbursmentMapper
    {
        public static LoanPaymentDtoCreate? Create(this VLoanDisbursments? source, string? UserId, string? OfficialReceiptNo)
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
