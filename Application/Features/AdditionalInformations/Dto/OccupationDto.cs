namespace Application.Features.AdditionalInformations.Dto
{
    public sealed class OccupationDto 
    {
        public int OccupationId { get; set; }
        public string OccupationTitle { get; set; } = string.Empty;
        public string? Khmer { get; set; } = string.Empty;
        public string? TypeOf { get; set; } = string.Empty;

        public int RowIndex { get; set; } = 0;
    }
}
