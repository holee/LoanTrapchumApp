using Domains.Models.Lists;

namespace Domains.Models.Bases
{
    public abstract class BaseEntityName : BaseEntity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? AltName { get; set; } = string.Empty;
        public EntType? EntType { get; set; } = Bases.EntType.Customer;
        public string? FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Sex { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? AltPhone { get; set; } = string.Empty;
        public string? EmailAddress { get; set; } = string.Empty;
        public string? IdCard { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string? Description { get; set; } = string.Empty;

    }
}
