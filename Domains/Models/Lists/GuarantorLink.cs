using Domains.Models.Bases;
using Domains.Models.Loans;

namespace Domains.Models.Lists
{
    public class GuarantorLink:BaseEntity 
    {
        public long Id { get; set; } 
        public Guid? EntityNameRefId { get; set; }
        public int? BranchId { get; set; }
        public Guid? LoanRequestRefId { get; set; } 
        public bool? IsActive { get; set; } = true;
        public EntityName? Guarantor { get; set; }
        public LoanRequest? LoanRequest { get; set; }

    }
}
