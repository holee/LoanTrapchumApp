namespace Web.UI.Pages.Lists.Collaterals.Tabs
{
    public sealed class DynamicTab
    {
        public string? Title { get; set; } = string.Empty;
        public string TabId { get; set; } = Guid.NewGuid().ToString("N");
        public long Id { get; set; } = 0;
        public string? CollateralId { get; set; }
        public RenderFragment? ChildContent { get; set; }
    }
}
