using Domains.Models.Lists;

namespace Applications.Features.Locations.Dtos;

public static class LocationDtoExtensions
{
    public static LtLocation ToLocation(this LocationCreateDto source)
    {
        return new LtLocation
        {
            Id = source.Id,
            LocationName = source.LocationName,
            Latitude = source.Latitude,
            Longitude = source.Longitude
        };
    }
}
