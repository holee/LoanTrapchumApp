using Applications.Responses.Lists.Collaterals;

namespace Applications.Responses.ViewModels.Customers
{
    public sealed class CustomerCollateralVm
    {
        public string? CollateralId { get; set; }
        public string? CustomerName { get; set; }
        public string? Description { get; set; }
        public string? DocumentKeeping { get; set; }
        public IEnumerable<VCollateralLines> CollateralLines { get; set; } = [];
    }
}
