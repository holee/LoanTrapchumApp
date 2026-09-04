using Application.Features.Addresses.Dto;

namespace Application.Features.Entities;
public class EntityNameDtoCreate : AddressDtoCreate
{
    public string Name { get; set; } = default!;
    public string AltName { get; set; } = string.Empty;
    public string EntType { get; set; } = string.Empty;
    public string FirstName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = default!;
    public string? Sex { get; set; }
    public string JobTitle { get; set; }=string.Empty;
    public string? Phone { get; set; }
    public string? AltPhone { get; set; } 
    public string? EmailAddress { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; } 
}
