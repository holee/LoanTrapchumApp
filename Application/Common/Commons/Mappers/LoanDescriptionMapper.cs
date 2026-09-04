using Application.Features.LoanRequests.Requests;
using Application.Responses.ViewModels.Loans.LoanRequests;
using Domains.Models.Loans;

namespace Application.Commons.Mappers
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
