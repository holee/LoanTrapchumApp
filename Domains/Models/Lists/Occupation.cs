namespace Domains.Models.Lists
{
    public sealed class Occupation
    {
        public int OccupationId { get; set; }
        public string OccupationTitle { get; set; } = string.Empty;
        public string? Khmer { get; set; } = string.Empty;
        public string? TypeOf { get; set; } = string.Empty;
    }
}
