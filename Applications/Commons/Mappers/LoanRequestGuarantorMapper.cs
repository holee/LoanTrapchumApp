using System;
using System.Collections.Generic;
using Applications.Features.LoanRequests.Requests;
using Domains.Models.Lists;
using Domains.Models.Loans;

namespace Applications.Commons.Mappers
{
    public static class LoanRequestGuarantorMapper
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

        public static List<LoanGuarantorLink> MapToGuarantorCreate(this LoanRequestGuarantorsDtoCreate? source) 
        {
            if (source is not null && source.GuarantorsLink.Any())
            {

                return source.GuarantorsLink.Select(x => new LoanGuarantorLink
                {
                    BranchId = source.BranchRefId,
                    LoanRequestRefId = source.LoanRequestId,
                    EntityNameRefId = x.EntityNameRefId,
                    IsActive = true,
                }).ToList();
            }

            return [];

        }



        //ToGuarantorDtoCreate
    }
}
