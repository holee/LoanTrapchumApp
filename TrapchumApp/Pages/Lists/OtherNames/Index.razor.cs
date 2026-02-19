using Applications.Extensions;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Lists;
using Domains.Models.Bases;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Threading.Tasks;
using Applications.Features.OtherNames;
using TrapchumApp.Helpers.Extensions;
using Applications.Features.Addresses.Dto;

namespace TrapchumApp.Pages.Lists.OtherNames;

public partial class Index
{
    private readonly IOtherNameServiceImpl _otherNameService;
    private readonly ISalutationServiceImpl _salutationService;
    public Index(IOtherNameServiceImpl otherNameService, ISalutationServiceImpl salutationService)
    {
        _otherNameService = otherNameService;
        _salutationService = salutationService;
    }

    List<VOtherNames> Names = [];
    int pageIndex = 0;
    int pageSize = 15;
    int counts = 0;
    string sortField = "EntId";
    string? _nameId = string.Empty;
    bool sortDescending = false;
    bool includeInactive = false;
    RadzenTabs? tabs;
    bool IsEnableEdit = false;
    bool IsEnableNew = false;
    bool IsEnableDetail = false;
    [Parameter] public int selectedIndex { get; set; } = 0;
    protected IEnumerable<string> Salutations = [];
    string? q;
    protected IEnumerable<ProvinceDto>? ProvinceList = [];
    protected EntityNamesCreate? modelEdit = new();
    protected IEnumerable<CommuneDto>? CommuneList = [];
    protected IEnumerable<DistrictDto>? DistrictList = [];
    protected IEnumerable<VillageDto>? VillageList = [];
    protected ProvinceDto? SelectedProvince;
    protected DistrictDto? SelectedDistrict;
    protected CommuneDto? SelectedCommune;
    protected VillageDto? SelectedVillage;
    bool isLoading = false;
    protected override async Task OnInitializedAsync()
    {
        try
        {
            await GetOtherNames();
            await GetSalutations();
            ProvinceList = await _addressService!.GetsProvinces();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await DialogService.Alert(ex.Message, "Exception");
        }
        await base.OnInitializedAsync();

    }
    protected async Task SearchChanged(string filter)
    {
        q = filter;
        await GetOtherNames(filter);
    }

    async Task PageSizeChanged(int page)
    {
        pageSize = page;
        await GetOtherNames(q);
    }

    async Task GetOtherNames(string? filterText = "")
    {
        isLoading = true;

        var param = new QueryParam(pageIndex, pageSize, sortField, includeInactive, sortDescending, filterText);
       
        var resp = await _otherNameService.GetNamesAsync(param);
        
        counts = resp.Count;
       
        Names = resp.Items;

        isLoading = false;

        StateHasChanged();

    }

    async Task LoadData(LoadDataArgs e)
    {
        pageIndex = e.Skip!.Value;

        (sortField, sortDescending) = e.PageInfo();

        var filterColunm = e.Filters?.FirstOrDefault(p =>
                                p.Property == nameof(VOtherNames.AltName) ||
                                p.Property == nameof(VOtherNames.Phone) ||
                                p.Property == nameof(VOtherNames.EmailAddress) ||
                                p.Property == nameof(VOtherNames.FirstName) ||
                                p.Property == nameof(VOtherNames.Gender) ||
                                p.Property == nameof(VOtherNames.LastName)
                            )?.FilterValue.ToString() ?? string.Empty;

        q = filterColunm ?? string.Empty;

        await GetOtherNames(q);

    }

    protected async Task IncludeInActiveChanged(bool e)
    {
        includeInactive = e;
        await GetOtherNames(q);
    }

    private async Task GetSalutations()
    {
        var salutations = await _salutationService.GetSalutions();

        if (salutations.IsSuccess)
        {
            Salutations = salutations!.Data!.Select(x => x.Title!).ToList();
        }
        else
        {
            Salutations = [];
        }

    }




    void AddTabOtherName()
    {
        IsEnableNew = true;
        selectedIndex = 1;
        StateHasChanged();
    }

    async Task OnEditClose(bool isEdited)
    {
        if (selectedIndex == 2)
        {
            IsEnableEdit = isEdited;

            selectedIndex = 0;
        }

        await GetOtherNames();

        StateHasChanged();
    }

    void OnDetailsClose(bool isDetailTab)
    {
        if (selectedIndex == 3)
        {
            IsEnableDetail = isDetailTab;
            selectedIndex = 0;

        }
        StateHasChanged();
    }

    void OnNameEdit(string? id)
    {
        selectedIndex = 2;
        IsEnableEdit = true;
        _nameId = id;
        StateHasChanged();
    }

    async void CloseTabs(int e)
    {
        if (selectedIndex == 1)
        {
            IsEnableNew = false;

            selectedIndex = 0;
        }
        else if (selectedIndex == 2)
        {
            IsEnableEdit = false;

            selectedIndex = 0;
        }
        else if (selectedIndex == 3)
        {
            IsEnableDetail = false;

            selectedIndex = 0;

        }

       await GetOtherNames(q);

        StateHasChanged();
    }
    
    protected async Task ShowEdit(string? id)
    {
        ContextMenuService.Close();

        if (id.IsNotNullOrEmpty())
        {
            try
            {
                _nameId = id;
                IsEnableEdit = true;
                selectedIndex = 2;
                await GetSalutations();
                StateHasChanged();

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }

        }
    }

    protected async Task ConvertToCustomer(string? id)
    {
        ContextMenuService.Close();

        var options = new DialogOptions
        {
            Width = "53rem",
            Draggable = true
        };

        bool? dialogueResult = await DialogService.OpenAsync<ConvertToCustomer>("Customer", new Dictionary<string, object?>() { { "Id", id } }, options);

        if (dialogueResult is not null && dialogueResult == true)
        {
            await GetOtherNames(q);
        }
    }
    protected async Task ConvertToVedor(string? id)
    {
        ContextMenuService.Close();

        var options = new DialogOptions
        {
            Width = "43rem",
            Draggable = true
        };

        bool? dialogueResult = await DialogService.OpenAsync<ConvertToVendor>("Vendor", new Dictionary<string, object?>() { { "Id", id } }, options);

        if (dialogueResult is not null && dialogueResult == true)
        {
            await GetOtherNames(q);
        }
    }
    protected async Task ConvertToEmployee(string? id)
    {
        ContextMenuService.Close();

        var options = new DialogOptions
        {
            Width = "43rem",
            Draggable = true
        };

        bool? rsp = await DialogService.OpenAsync<ConvertToEmployee>("Convert to Employee", new Dictionary<string, object?>() { { "Id", id } }, options);

        if (rsp is bool result && result == true)
        {
            await GetOtherNames(q);
        }
    }
    protected EntNameDto? OtherNames = new();



    void DisplayDetail(string? id)
    {
        ContextMenuService.Close();

        if (id.IsNotNullOrEmpty())
        {
            IsEnableDetail = true;
            selectedIndex = 3;
            _nameId = id;
            StateHasChanged();
        }
    }
    async Task OnDelete(string? id)
    {
        var confirm = (bool)await DialogService.Confirm("Other Name deleted will not be able to recover, would you like to continue?", "Delete Other Name",
            new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
        if (confirm)
        {
            var result = await _otherNameService!.DeleteAsync(id);
            if (result.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Delete Other Name", "Other Name was deleted successful!");
                await OnInitializedAsync();
                ContextMenuService.Close();
                return;
            }
        }
        else
        {
            Console.WriteLine("Canceled");
            ContextMenuService.Close();

        }
    }




    private async Task OnChanged(bool args,string? id)
    {
        var confirm = await DialogService.Confirm("Do you want enable or disable?", "Confirm", new ConfirmOptions
        {
            OkButtonText="Yes",
            CancelButtonText="Cancel"
        });

        if (confirm == false) return;

        var rsp=  await _otherNameService.UpdateStatusAsync(id, args);

        if (rsp.IsSuccess) { 
        
            await GetOtherNames(q);
            return;
        }

        NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);

    }
}





















