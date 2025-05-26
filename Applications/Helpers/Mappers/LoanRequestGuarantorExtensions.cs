using Applications.Requests.Loans.LoanRequests;
using Domains.Models.Lists;
using Domains.Models.Loans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Helpers.Mappers
{
    public static class LoanRequestGuarantorExtensions
    {
        public static LoanRequestGuarantors MapTo(this LoanRequestGuarantorsDtoCreate source)
        {
            return new LoanRequestGuarantors
            {
                 BranchRefId = source.BranchRefId,
                 LoanRequestId=source.LoanRequestId,
                 CustomerRefId=source.CustomerRefId,
                 GuarantorsLink=source.GuarantorsLink
            };
        }

        public static List<LoanGuarantorLink> MapToGuarantorLinkCreate(this LoanRequestGuarantorsDtoCreate? source)
        {
            if (source is not null && source.GuarantorsLink.Any()) {

                return source.GuarantorsLink.Select(x => new LoanGuarantorLink
                {
                    BranchId = source.BranchRefId,
                    LoanRequestRefId=source.LoanRequestId,
                    EntityNameRefId=x.EntityNameRefId, 
                    IsActive=true,
                }).ToList();
            }

            return [];
           
        }
    }
}
