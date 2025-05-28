using Applications.Responses.Lists.Collaterals;
using Domains.Models.Collaterals;
using Domains.Models.Loans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Commons.Mappers
{
    public static class LoanCollateralRequestMapper
    {
        public static List<CollateralLink> MapToCollateralLinkCreate(this LoanCollateralRequest req)
        {
            if (req is not null && req.CollateralLinesLink.Any()) 
            {
                return req.CollateralLinesLink.Select(x => new CollateralLink
                {
                    Id = x.Id,
                    BranchId = req.BranchId,
                    LoanRequestRefId = req.LoanRequestId,
                    CollateralRefId = x.CollateralRefId,
                    CollateralLineRefId = x.CollateralLineRefId,
                    IsActive = x.IsActive,
                    Description = x.Description,
                    CreatedDt = DateTime.Now,
                }).ToList();
            }
            return [];

        }
    }
}
