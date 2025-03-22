namespace Applications.Responses.Collaterals;

public class CollateralResponse
{
    public long? CollateralId { get; set; }
    public DateTime? RegisterDate { get; set; } = DateTime.Now;
    public string? CustomerName { get; set; }
    public string? DocumentKeeping { get; set; }
    public string? Remark { get; set; }
    public List<CollateralLineResponse> CollateralLines { get; set; } = [];

}
