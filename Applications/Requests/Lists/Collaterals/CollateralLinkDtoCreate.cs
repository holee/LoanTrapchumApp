using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Requests.Lists.Collaterals
{
    public sealed class CollateralLinkDtoCreate
    {
        public long? Id { get; set; }
        public int? BranchId { get; set; }
        public long? CollateralLineRefId { get; set; }
        public Guid? LoanRequestRefId { get; set; }
        public Guid? CollateralRefId { get; set; }
        public bool? IsActive { get; set; } = true;
        
        public string? Description { get; set; } = string.Empty;
    }
}
