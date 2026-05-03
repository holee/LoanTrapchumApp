using Applications.Features.AdditionalInformations.Dto;
using Applications.Features.Customers.Commands.EditCustomer;
using Applications.Features.Customers.Dto;
using Applications.Features.Customers.Queries.GetUpdateCustomerById;
using Applications.Features.Customers.Response;
using Applications.Responses.Lists.Entities;
using Domains.Models.Addresses;

namespace TrapchumApp.Pages.Lists.Customers;

public partial class CustomerUpdatePage
{
    [Parameter] public EventCallback Closed { get; set; }
    [Parameter] public EventCallback<VCustomerList> UpdateChanged { get; set; }
    [Parameter] public string? CustomerId { get; set; }
    [Parameter] public int? BranchId { get; set; } = 0;

    private EntityCustomerCreate model = new();

    private EditForm? editForm;

    private bool IsLoading = true;

    private bool isSaving;

    private string? newSalution = "";

    private IEnumerable<string> Salutations = [];

    private IEnumerable<string> Cities = [];

    private IEnumerable<OcupationResponse> Occupations = [];

    private IEnumerable<Country> Countries = [];

    private IEnumerable<NationalityDto> _nationalities = [];

    private IEnumerable<string> Statuses = ["Single", "Married", "Divorced", "Widow", "Others"];
    protected override async Task OnInitializedAsync()
    {
        try
        {
            IsLoading = true;

            var rsp = await mediator.Send(new GetUpdateCustomerByIdQuery(CustomerId));

            if (rsp.IsSuccess())
            {
                model = rsp.Value;

                model.BranchId = BranchId;
            }
            else
            {
                model.City = "Siem Reap";
                model.CountryName = "ប្រទេសកម្ពុជា";
                model.Salutation = "Mr";
                model.MarritalStatus = "Single";
                model.Nationality = "Khmer";
                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
            }
        }
        finally
        {
            IsLoading = false;
        }

        await base.OnInitializedAsync();
    }


    private async Task FormSubmit()
    {
        if (editForm?.EditContext?.Validate() == false) return;

        try
        {
            isSaving = true;

            model.EntType = EntityTypes.Customer.ToString();

            var command = new EditCustomerCommand(model);

            var rsp = await mediator.Send(command);

            if (rsp.IsSuccess())
            {
                await UpdateChanged.InvokeAsync(rsp.Value);

                await CloseTab();

                NotificationService.Notify(NotificationSeverity.Success, "Customer was edited successfully!");
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
            }
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
        finally
        {
            isSaving = false;
        }
    }

    private async Task ClickCloseTab() => await Closed.InvokeAsync(3);

    private async Task CloseTab() => await Closed.InvokeAsync();


}

