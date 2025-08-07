using Applications.Requests.Lists.Collaterals;

namespace Applications.Features.Collaterals.Requests
{
    public sealed class CollateralDtoCreate
    {
        public string? CollateralId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string? CustomerName { get; set; }
        public string? DocumentKeeping { get; set; } = string.Empty;
        public string? Remark { get; set; } = string.Empty;
        public List<CollateralLineDtoCreate> CollateralLines { get; set; } = [];
    }
}
