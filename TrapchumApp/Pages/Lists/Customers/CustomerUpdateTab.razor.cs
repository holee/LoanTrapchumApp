using Applications.Enumerations;
using Applications.Features.AdditionalInformations.Dto;
using Applications.Features.Customers.Commands.EditCustomer;
using Applications.Features.Customers.Dto;
using Applications.Features.Customers.Queries.GetUpdateCustomerById;
using Applications.Features.Customers.Response;
using Applications.Responses.Lists.Entities;
using Applications.Responses.Paginations;
using Applications.Services.Lists;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Lists.Customers;

public partial class CustomerUpdateTab(ISalutationServiceImpl salutationService)
{
    [Parameter] public EventCallback<int> Closed { get; set; }
    [Parameter] public EventCallback<VCustomerList> UpdateChanged { get; set; }
    [Parameter] public string? CustomerId { get; set; }

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

            await GetAdditionalInformation();

            var rsp = await mediator.Send(new GetUpdateCustomerByIdQuery(CustomerId));

            if (rsp.IsSuccess())
            {
                model = rsp.Value;
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

                await CloseTab(3);

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

    private async Task CloseTab(int selectedIndex) => await Closed.InvokeAsync(selectedIndex);

    private async Task GetAdditionalInformation()
    {
        //await GetNationalities();

        //Occupations = await _employeeService!.GetOccupationsAsync("Position");

        //await GetCountries();

        Cities = await _addressService.GetCities();

        await GetSalutations();
    }

    //private async Task GetCountries()
    //{
    //    var rsp = await _addressService.GetCountries(105);

    //    if (rsp.IsSuccess)
    //    {
    //        Countries = rsp.Data ?? [];
    //    }
    //}

    //private async Task GetNationalities()
    //{
    //    var command = new GetNationalitiesQuery(0, new QueryParam());

    //    var resp = await mediator.Send(command);

    //    _nationalities = resp.Items;  //await _nationalityService.GetNationalities();
    //}

    private async Task GetSalutations()
    {
        var salutations = await salutationService.GetSalutions();

        if (salutations.IsSuccess)
        {
            Salutations = salutations.Data!.Select(x => x.Title!).ToList();
        }
        else
        {
            Salutations = new List<string>();
        }
    }

    private async Task AddNewSalutation(KeyboardEventArgs e)
    {
        if (e.Code == "Enter")
        {
            await salutationService.AddSalution(new Salutation { Title = newSalution });

            await GetSalutations();
        }
    }

    private void OnTextChangedSalution(ChangeEventArgs e)
    {
        newSalution = e?.Value?.ToString();
    }

    private async Task<IEnumerable<string>?> SearchSalutaions(string searchText = "")
    {
        var salution = Salutations?.Where(x => x.StartsWith(searchText, StringComparison.InvariantCultureIgnoreCase))
                                  .ToList();
        return await Task.FromResult(salution);
    }
}

