using Domains.Models.Bases;

namespace Applications.Features.OtherNames
{
    public class EntNameDtoCreate : BaseAddress
    {
        public string? Name { get; set; } = default!;
        public string? AltName { get; set; } = string.Empty;
        public string? EntType { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? JobTitle { get; set; } = string.Empty;
        public string? Sex { get; set; }
        public string? Phone { get; set; } = string.Empty;
        public string? AltPhone { get; set; } = string.Empty;
        public string? EmailAddress { get; set; } = string.Empty;
        public string? IdCard { get; set; } = string.Empty;
        public long? AddressRefId { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
    }

}
