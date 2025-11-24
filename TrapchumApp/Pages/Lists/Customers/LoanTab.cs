using TrapchumApp.Helpers.Constants;

namespace TrapchumApp.Pages.Lists.Customers;

internal enum CustomerAction
{
    Create,
    Update,
    Delete,
    Details
}


internal sealed class CustomerTab
{
    public string TabId { get; set; } = string.Empty;
    public string Title { get; set; } = "Customer Tab";
    public string Icon
    {
        get
        {
            return CustomerAction switch
            {
                CustomerAction.Create => IconNames.AddNew,
                CustomerAction.Update => IconNames.PersonEdit,
                CustomerAction.Details => IconNames.Info,
                _ => IconNames.Delete,
            };
        }
    }
    public CustomerAction CustomerAction { get; set; } = CustomerAction.Create;
    public RenderFragment? ChildContent { get; set; }
}
