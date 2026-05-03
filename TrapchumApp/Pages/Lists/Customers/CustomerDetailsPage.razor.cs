using Applications.Bases;
using Applications.Features.Customers.Commands.DeleteCustomer;
using Applications.Features.Customers.Queries.GetCustomerDetailById;
using Applications.Features.Customers.Response;
using Domains.Models.Identity;

namespace TrapchumApp.Pages.Lists.Customers;

public partial class CustomerDetailsPage(ILogger<CustomerDetailsPage> logger)
{
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [Parameter] public int? BranchId { get; set; } = 0;
    [Parameter, EditorRequired] public string? CustomerId { get; set; }
    [Parameter] public EventCallback Closed { get; set; }
    [Parameter] public EventCallback<string?> EditClick { get; set; }

    private VCustomerDetails Customer = new();

    private string? _userId;

    private bool _isLoading = false;
    protected override async Task OnInitializedAsync()
    {
        try
        {
            _isLoading = true;

            var auth = await AuthenticationState;

            if (auth.User.Identity!.IsAuthenticated)
            {
                _userId ??= auth.User.GetId();
            }

            var response = await mediator.Send(new GetCustomerDetailByIdQuery(CustomerId, false));

            if (response.IsSuccess())
            {
                Customer = response.Value;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, response.GetMessage());
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            _isLoading = false;
        }
        finally
        {
            _isLoading = false;
        }

        await base.OnInitializedAsync();
    }

    async Task ShowEditCustomer(string? customerId)
    {
        await EditClick.InvokeAsync(customerId);
    }

    async Task DeleteCustomer()
    {
        var rsp = await mediator.Send(new DeleteCustomerCommand(CustomerId));

        if (rsp.Status == DeleteStatus.Success)
        {
            NavigationManager.NavigateTo("/App/Customers");
            return;
        }

        if (rsp.Status == DeleteStatus.InUse)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Customer cannot be deleted because it has active loan or collateral");
        }
    }

    async Task CloseTab()
    {
        await Closed.InvokeAsync(2);
    }
}
