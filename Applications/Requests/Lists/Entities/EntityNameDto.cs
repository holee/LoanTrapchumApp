using Applications.Requests.Lists.Addreses;
using Domains.Models;

namespace Applications.Requests.Lists.Entities;

public class EntityNameDto:BaseEntity
{
    public long EntId { get; set; }
    public string? Name { get; set; }
    public string? AltName { get; set; }
    public string? EntType { get; set; } = default!;
    public string? FirstName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public string? LastName { get; set; } = default!;
    public string? JobTitle { get; set; } 
    public string? Sex { get; set; }
    public string? EmailAddress { get; set; }
    public string? Phone { get; set; }
    public bool? IsActive { get; set; }
    public AddressDto? Address { get; set; }
}


