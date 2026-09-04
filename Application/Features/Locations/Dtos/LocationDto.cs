namespace Application.Features.Locations.Dtos;

public class LocationDto
{
    public long Id { get; set; }
    public string? LocationName { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public DateTimeOffset? CreatedDT { get; set; }
    public DateTimeOffset? ModifiedDT { get; set; }
}
