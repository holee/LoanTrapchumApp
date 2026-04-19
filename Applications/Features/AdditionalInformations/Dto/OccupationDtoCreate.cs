namespace Applications.Features.AdditionalInformations.Dto;

public class OccupationDtoCreate 
{
    public string OccupationTitle { get; set; } = string.Empty;
    public string? Khmer { get; set; } = string.Empty;
    public string? TypeOf { get; set; } = string.Empty;
}