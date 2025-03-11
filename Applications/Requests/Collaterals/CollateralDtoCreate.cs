namespace Applications.Requests.Collaterals
{
    public class CollateralDtoCreate
    {
        public CollateralDtoCreate()
        {
            CollateralLines = new List<CollateralLineDtoCreate>();
        }
        public long CollateralId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public long? CustomerRefId { get; set; }
        public string? CustomerName { get; set; }
        public string? DocumentKeeping { get; set; } = string.Empty;
        public string? Remark { get; set; } = string.Empty;
        public List<CollateralLineDtoCreate>? CollateralLines { get; set; } = [];
    }
}
