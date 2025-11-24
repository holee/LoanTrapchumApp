using Domains.Models.Collaterals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.LoanRequests.Dtos;


public sealed class LoanCollateralRequestDtoCreate 
{
    public int? BranchId { get; set; }

    public string? LoanRequestId { get; set; }

    public string? CustomerId { get; set; }

    public List<CollateralLink> CollateralLinesLink { get; set; } = [];
}
