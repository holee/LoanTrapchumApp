using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models.Loans;

public sealed class LoanGuarantor 
{
    public long? Id { get; set; } = 0;
    public int? BranchId { get; set; }
    public Guid? LoanRequestRefId { get; set; }
    public Guid? EntityNameRefId { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; } = string.Empty;
}
