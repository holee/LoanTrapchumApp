using System.ComponentModel.DataAnnotations;
using Domains.Models.Bases;

namespace Domains.Models.Lists
{
    public sealed class EntityName : BaseAddress
    {
        
        [Key]
        public Guid EntId { get; set; }
        public string? Name { get; set; } = default!;
        public string? AltName { get; set; } = string.Empty;
        public string? EntType { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? JobTitle { get; set; } = string.Empty;
        public string? Sex { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? AltPhone { get; set; } = string.Empty;
        public string? EmailAddress { get; set; } = string.Empty;
        public string? IdCard { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
       // public List<LoanGuarantorLink> GuarantorsLink { get; set; } = [];

    }
}
