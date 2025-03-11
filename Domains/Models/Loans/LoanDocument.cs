using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models.Loans
{
    public class LoanDocument
    {
        public long Id { get; set; }
        public int? BranchId { get; set; }
        public long? LoanRequestId { get; set; }
        public string? OriginalName { get; set; }
        public string? NewFileName { get; set; }
        public string? AltText { get; set; }
        public string? FileUrl { get; set; }
        public string? Description { get; set; }
    }
}
