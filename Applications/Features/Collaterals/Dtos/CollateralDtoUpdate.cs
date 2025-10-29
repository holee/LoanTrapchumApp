namespace Applications.Features.Collaterals.Dtos 
{
    public class CollateralDtoUpdate
    {
        public long CollateralId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public long? CustomerRefId { get; set; }
        public string? CustomerName { get; set; }
        public string? DocumentKeeping { get; set; } = string.Empty;
        public string? Remark { get; set; } = string.Empty;
        public ICollection<CollateralLineDtoUpdate>? CollateralLines { get; set; }
    }
}
