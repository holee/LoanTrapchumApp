namespace TrapchumApp.Components.Comps.Controls
{
    public partial class InputGroup
    {
        private string size = "sm";
        [Parameter]
        public string CssClass { get; set; } = string.Empty;
        [Parameter]
        public RenderFragment? Start { get; set; }
        [Parameter]
        public RenderFragment? End { get; set; }
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
        [Parameter]
        public InputSize Size { get; set; } = InputSize.Default;

        protected override void OnParametersSet()
        {
            size = Size switch
            {
                InputSize.Small => "sm",
                InputSize.Medium => "md",
                InputSize.Large => "lg",
                _ => "md"
            };
            base.OnParametersSet();
        }
    }
}