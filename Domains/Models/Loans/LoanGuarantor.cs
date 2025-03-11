using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models.Loans;

public class LoanGuarantor 
{
    public long? Id { get; set; } = 0;
    public int? BranchId { get; set; }
    public long? LoanRequestRefId { get; set; }
    public long? EntityNameRefId { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; } = string.Empty;
}
