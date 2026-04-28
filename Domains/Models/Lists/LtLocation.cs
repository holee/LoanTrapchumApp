namespace Domains.Models.Lists;

public sealed class LtLocation
{
    public long Id { get; set; }
    public string? LocationName { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public DateTimeOffset? CreatedDT { get; set; }
    public DateTimeOffset? ModifiedDT { get; set; }
}
