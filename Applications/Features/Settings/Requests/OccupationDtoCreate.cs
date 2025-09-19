namespace Applications.Features.Settings.Requests
{
    public sealed class OccupationDtoCreate
    {
        public int OccupationId { get; set; }
        public string? OccupationTitle { get; set; } = string.Empty;
        public string? Khmer { get; set; } = string.Empty;
        public string? TypeOf { get; set; } = string.Empty;
    }

}
