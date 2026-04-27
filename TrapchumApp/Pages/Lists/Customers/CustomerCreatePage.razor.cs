using Applications.Features.Customers.Commands.CreateCustomer;
using Applications.Features.Customers.Dto;
using Applications.Features.Customers.Response;

namespace TrapchumApp.Pages.Lists.Customers;

public partial class CustomerCreatePage
{
    [Parameter] public EventCallback Closed { get; set; } = default!;
    [Parameter] public EventCallback<VCustomerList> UpdateState { get; set; }
    [Parameter] public int BranchId { get; set; } = 0;
    [Parameter] public EntityCustomerCreate CustomerModel { get; set; } = new();
    [CascadingParameter] public Task<AuthenticationState> AuthState { get; set; } = default!;
    protected EditForm? editForm;
    private bool IsLoading = true;
    private string? _userId = string.Empty;
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthState;

        _userId = authState.GetUserId();

        CustomerModel.CountryName = "ប្រទេសកម្ពុជា";

        CustomerModel.Salutation = "Mr";

        CustomerModel.MarritalStatus = "Single";

        CustomerModel.Nationality = "Khmer";

        CustomerModel.CreatedBy = _userId;
        
        CustomerModel.Photo = "/assets/profiles/profile.png";

        CustomerModel.BranchId = BranchId;

        await base.OnInitializedAsync();
    }

    protected async Task CreateAsync(bool saveClosed = false)
    {

        CustomerModel.Id = $"ent-{Guid.CreateVersion7()}";

        CustomerModel.EntType = EntityTypes.Customer.ToString();

        CustomerModel.Photo ??= "/assets/profiles/profile.png";

        var rsp = await mediator.Send(new CreateCustomerCommand(CustomerModel));

        if (rsp.IsSuccess())
        {
            NotificationService.Notify(NotificationSeverity.Success, "Customer was created successfully!");

            if (saveClosed)
            {

                _toastService.ShowSuccess("Customer was created successfully!");

                await Close();

                await UpdateState.InvokeAsync(rsp.Value);
            }
            else
            {
                CustomerModel = new();

                await UpdateState.InvokeAsync(rsp.Value);
            }
        }
        else
        {
            _toastService.ShowError(rsp.GetMessage());
        }

    }

    protected async Task Close()
    {
        await Closed.InvokeAsync();
    }

}
