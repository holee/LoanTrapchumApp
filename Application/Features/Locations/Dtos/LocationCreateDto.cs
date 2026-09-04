namespace Application.Features.Locations.Dtos;

public sealed class LocationCreateDto
{
    public long Id { get; set; }
    public string? LocationName { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
}
