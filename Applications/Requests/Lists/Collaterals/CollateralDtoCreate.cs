namespace Applications.Requests.Lists.Collaterals
{
    public sealed class CollateralDtoCreate
    {
        public Guid? CollateralId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string? CustomerName { get; set; }
        public string? DocumentKeeping { get; set; } = string.Empty;
        public string? Remark { get; set; } = string.Empty;
        public List<CollateralLineDtoCreate>? CollateralLines { get; set; } = [];
    }
}
