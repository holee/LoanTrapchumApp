namespace Applications.Features.Customers.Dto;

public sealed class EmergencyContactDtoCreate
{
    public string? ContactId { get; set; }
    public string? ContactName { get; set; }
    public string? ContactNumber { get; set; }
    public string? Relationship { get; set; }
    public string? Description { get; set; }
}