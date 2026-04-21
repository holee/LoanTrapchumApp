using Applications.Extensions;
using Applications.Features.Guarantor.Dtos;
using Applications.Responses.Paginations;
using Applications.Services.Lists;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.OtherNames;
public partial class List
{
    private readonly IGuarantorServiceImpl _gService;

    public List(IGuarantorServiceImpl gService)
    {
        _gService = gService;
    }


    protected override async Task OnInitializedAsync()
    {

        await GetGuarantors();

        await base.OnInitializedAsync();
    }

    bool includeInactive = false;
    string? filterText = "";
    int pageIndex = 0;
    int pageSize = 10;
    string sortColumn = "Name";
    bool sortDescending = false;
    int counts = 0;
    bool isLoading = false;
    bool ShowAddTab = false;
    bool ShowDetailTab = false;
    bool ShowEditTab = false;
    RadzenTabs tabs = default!;
    int selectedTabIndex = 0;
    string? SelectedGuarantorId = string.Empty;
    IEnumerable<VGuarantor> guarantors = [];
    async Task ChangedPageSize(int page)
    {
        pageSize = page;
        await GetGuarantors();
    }

    async void SearchTextChanged()
    {
        await GetGuarantors();
    }

    async Task AddGuarantorTab()
    {
        if (!ShowAddTab)
        {
            ShowAddTab = true;
            //provinceList = await _addressService!.GetsProvinces();
            //Model.Gender = "mr";
        }
        selectedTabIndex = 1;
        tabs.Reload();
    }

    void GetGuarantorDetails(string? id)
    {
        ContextMenuService.Close();
        if (!ShowDetailTab)
        {
            ShowDetailTab = true;
        }
        SelectedGuarantorId = id;

        selectedTabIndex = 2;

        tabs.Reload();

        Refresh();
    }
    protected async Task DeleteGuarantorAsync(string? id)
    {

        if (id.IsNotNullOrEmpty())
        {
            NotificationService.Notify(NotificationSeverity.Warning, "there is no any data selected.");
            return;
        }

        var confirm = await DialogService.Confirm($"do you want to delete guarantor with #{id}",
                                            "Delete", new ConfirmOptions
                                            {
                                                OkButtonText = "Ok",
                                                CancelButtonText = "Cancel"
                                            });
        if (confirm == false) return;

        var rsp = await _gService.DeleteAsync(id);

        if (rsp.IsSuccess)
        {
            NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
            ShowAddTab = false;
            selectedTabIndex = 0;
            await GetGuarantors();
            tabs.Reload();
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
        }
    }

    void Refresh() { }
    async void GetEditGuarantor(string? id)
    {
        ContextMenuService.Close();

        var param = new Dictionary<string, object?> { { "NameId", id } };

        var options = new DialogOptions
        {
            Width = "60rem",
            Resizable = true,
            Draggable = true
        };

        var result = await DialogService.OpenAsync<GuarantorEditPage>($"Edit Guarantor #{id}", param, options);

        if (((bool)result) == true)
        {
            await GetGuarantors();
        }
    }


    void AddDetailGuarantorTab()
    {
        if (!ShowDetailTab)
        {
            ShowDetailTab = true;
        }
        selectedTabIndex = 2;
        tabs.Reload();
    }

    async Task GetGuarantors()
    {
        isLoading = true;
        
        var param = new QueryParam(pageIndex, pageSize, sortColumn, sortDescending, includeInactive, filterText);
        
        var guarantorResp = await _gService.GetGuarantorsAsync(param);

        guarantors = guarantorResp.Items;
       
        counts = guarantorResp.Count;

        isLoading = false;

        StateHasChanged();
    }

    async Task LoadGuarantors(LoadDataArgs e)
    {
        pageIndex = e.Skip!.Value;
        if (!string.IsNullOrEmpty(e.OrderBy))
        {
            sortColumn = e.OrderBy.Split(" ")[0];
            string dir = e.OrderBy.Split(" ")[1];
            sortDescending = dir.Contains("asc") ? false : true;
        }
        await GetGuarantors();

    }
    async Task ShowGuarantorModal()
    {
        var options = new DialogOptions
        {
            Width = "60rem",
            Height = "30rem",
            Draggable = true,
            AutoFocusFirstElement = true
        };
        var modalResult = await DialogService.OpenAsync<GuarantorAddModal>("Guarantor", options: options);

        if (bool.TryParse(modalResult, out bool result) && result == true)
        {
            await GetGuarantors();
        }

    }
    async Task IncludeInactiveCheckedChanged()
    {
        await GetGuarantors();
    }


}
