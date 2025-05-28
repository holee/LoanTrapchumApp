using Applications.Requests.Loans.LoanRequests;
using Applications.Responses.ViewModels.Loans.LoanRequests;
using Domains.Models.Loans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Commons.Mappers
{
    public static class LoanDescriptionMapper
    {
        public static LoanDescriptions MapToDescriptionCreate(this LoanDescriptionsDtoCreate source) 
        {
            return new LoanDescriptions
            {
                Memo = source.Memo,
                LoanRequestRefId = source.LoanRequestRefId,
                UserRefId = source.UserRefId,
            };
        }

        public static LoanDescriptions MapToDescriptionEdit(this LoanDescriptionsDtoUpdate source)
        {
            return new LoanDescriptions
            {
                Id = source.Id,
                Memo = source.Memo,
                LoanRequestRefId = source.LoanRequestRefId,
                UserRefId = source.UserRefId,
            };
        }

        public static LoanDescriptionsDtoUpdate MapToDescriptionEdit(this VLoanRequestDescription source)
        {
            return new LoanDescriptionsDtoUpdate
            {
                Id = source.Id,
                Memo = source.Memo,
                LoanRequestRefId = source.LoanRequestRefId
            };
        }


    }
}
