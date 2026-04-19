namespace Domains.Models.Lists
{
    public sealed class Nationality
    {
        public int NationalityId { get; set; }
        public string? NationalityName { get; set; } = string.Empty;
        public string? NationalityCode { get; set; } = string.Empty;
    }
}
