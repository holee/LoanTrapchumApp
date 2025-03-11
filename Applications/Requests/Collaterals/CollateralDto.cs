namespace Applications.Requests.Collaterals;

public class CollateralDto
{
    public long? CollateralId { get; set; }
    public DateTime? RegisterDate { get; set; }=DateTime.Now;
    public string? CustomerName { get; set; }
    public string? DocumentKeeping { get; set; }
    public string? Remark { get; set; }
    public List<CollateralLineDto> CollateralLines { get; set; } = [];

}
