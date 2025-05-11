using Domains.Models.Bases;
using Domains.Models.Loans;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models.Lists
{
    public class LoanGuarantorLink:BaseEntity 
    {
        public long Id { get; set; }
        [ForeignKey("EntityNames")] 
        public string? EntityNameRefId { get; set; }
        public int? BranchId { get; set; }

        [ForeignKey("LoanRequest")]
        public string? LoanRequestRefId { get; set; } 
        public bool? IsActive { get; set; } = true;
        public EntityNames? Guarantor { get; set; }
        public LoanRequest? LoanRequest { get; set; }

    }
}
