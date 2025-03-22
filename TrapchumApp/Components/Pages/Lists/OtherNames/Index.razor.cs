using Applications.Interfaces.Addresses;
using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Customers;
using Applications.Requests.Lists.Employees;
using Applications.Requests.Lists.OtherNames;
using Applications.Requests.Lists.Vendors;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;


namespace TrapchumApp.Components.Pages.Lists.OtherNames;

public partial class Index
{

    private readonly IAddressService _addressService;

    private readonly ISalutationService _salutationService;
    private readonly IOtherNameServiceImpl _otherNameService;

    IEnumerable<VOtherNames> Names = [];
    int pageIndex = 0;
    int pageSize = 10;
    int counts = 0;
    string sortField = "EntId";
    bool sortDescending = false;
    bool includeInactive = false;
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
    protected async Task OnSearch(ChangeEventArgs e)
    {
        q = e.Value!.ToString();
        await GetOtherNames(q);
    }

    async Task PageSizeChanged(int page)
    {
        pageSize = page;
        await GetOtherNames(q);
    }

    async Task GetOtherNames(string? filterText = "")
    {

        var param = new QueryParam(pageIndex, pageSize, sortField, sortDescending, includeInactive, filterText);
        var resp = await _otherNameService.GetNamesAsync(param, string.Empty);
        counts = resp.Count;
        Names = resp.Items;

    }

    async Task LoadData(LoadDataArgs e)
    {
        q = e.Filter ?? string.Empty;
        pageIndex = e.Skip!.Value;
        if (!string.IsNullOrEmpty(e.OrderBy))
        {
            sortField = e.OrderBy.Split(" ")[0];
            string dir = e.OrderBy.Split(" ")[1];
            sortDescending = dir.Contains("asc") ? false : true;
        }
        await GetOtherNames(q);

    }

    protected async Task IncludeInActiveChanged(bool e)
    {
        includeInactive = e;
        await OnInitializedAsync();
    }
    protected async Task Reload()
    {
        await OnInitializedAsync();
    }

    RadzenTabs? tabs;
    bool IsEnableEdit = false;
    bool IsEnableNew = false;
    bool IsEnableDetail = false;
    [Parameter] public int selectedIndex { get; set; } = 0;
    protected IEnumerable<string> Salutations = [];
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
    async void CloseTabs(int e)
    {
        if (selectedIndex == 1)
        {
            IsEnableNew = false;
            selectedIndex = 0;

            await OnInitializedAsync();
        }
        else if (selectedIndex == 2)
        {
            IsEnableEdit = false;
            selectedIndex = 0;

            await OnInitializedAsync();
        }
        else if (selectedIndex == 3)
        {
            IsEnableDetail = false;
            selectedIndex = 0;

        }
        StateHasChanged();
    }
    protected async Task DeactiveOrActive(EntNameDto e)
    {

        var response = await _otherNameService!.UpdateStatusAsync(e.EntId);

        if (response.IsSuccess)
        {
            names!.ToList().Remove(e);
            NotificationService.Notify(NotificationSeverity.Success, "", response.ErrMessage);
            // await IncludeInActiveChanged(false);
            await InvokeAsync(StateHasChanged);

        }
        else
        {
            // await IncludeInActiveChanged(false);
            NotificationService.Notify(NotificationSeverity.Warning, "", "Changing status failded!");
        }
    }
    string? q;
    protected IEnumerable<ProvinceDto>? ProvinceList = [];
    protected EntNameDtoUpdate? modelEdit = new();
    protected IEnumerable<CommuneDto>? CommuneList = [];
    protected IEnumerable<DistrictDto>? DistrictList = [];
    protected IEnumerable<VillageDto>? VillageList = [];
    protected ProvinceDto? SelectedProvince;
    protected DistrictDto? SelectedDistrict;
    protected CommuneDto? SelectedCommune;
    protected VillageDto? SelectedVillage;
    //protected bool includeInactive = false;
    List<int> SelectedRow = [10, 20, 50, 100];
    IQueryable<EntNameDto> names;
    protected async Task ShowEdit(long id)
    {
        if (id != 0)
        {
            try
            {

                //modelEdit = await _otherNameService!.GetToEditAsync(id);
                IsEnableEdit = true;
                selectedIndex = 2;
                //ProvinceList = await AddressService!.GetAllProvinceAsync();
                StateHasChanged();
                await GetSalutations();

                if (modelEdit is not null)
                {
                    if (modelEdit!.ProvinceRefId.HasValue)
                    {
                        SelectedProvince = ProvinceList!.FirstOrDefault(d => d.ProvinceId == modelEdit?.ProvinceRefId);
                    }
                    if (modelEdit.DistrictRefId.HasValue)
                    {
                        DistrictList = await _addressService!.GetDistrictsByProvinceId(modelEdit?.ProvinceRefId);
                        SelectedDistrict = DistrictList.FirstOrDefault(d => d.DistrictId == modelEdit?.DistrictRefId);

                    }
                    if (modelEdit!.DistrictRefId.HasValue)
                    {
                        CommuneList = await _addressService!.GetCommunesByDistrictId(modelEdit?.DistrictRefId);
                        SelectedCommune = CommuneList.FirstOrDefault(c => c.CommuneId == modelEdit?.CommuneRefId);
                    }
                    if (modelEdit!.DistrictRefId.HasValue)
                    {
                        VillageList = await _addressService!.GetVillagesByCommuneId(modelEdit?.CommuneRefId);
                        SelectedVillage = VillageList.FirstOrDefault(x => x.VillageId == modelEdit?.VillageRefId);
                        Console.WriteLine(SelectedVillage!.VilInKhmer + " Selected");

                    }
                }
                ContextMenuService.Close();

                StateHasChanged();

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }

        }
    }

    protected async Task ConvertToCustomer(long? id)
    {
        var confirm = (bool)await DialogService.Confirm("Are you sure!, you want to this other name to customer?", "Changing Entity",
            new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
        if (confirm)
        {
            var e = await _otherNameService.GetByIdAsync(id!.Value);

            if (e is null)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Changing Entity", "Getting other name information is empty!");
                return;
            }
            var customer = new CustomerDtoCreate();
            customer.FirstName = e.FirstName;
            customer.LastName = e.LastName == "" ? "N/A" : e.LastName;
            customer.MiddleName = e.MiddleName;
            customer.AltName = e.AltName;
            customer.JobTitle = e.JobTitle;
            customer.Gender = e.Sex;
            customer.Phone = e.Phone;
            customer.AltPhone = e.AltPhone;
            customer.EmailAddress = e.EmailAddress;
            customer.CompanyName = "N/A";
            customer.EnType = "Customer";
            //customer.IDCard = e.IDCard;
            customer.Description = e.Description;
            customer.CountryName = e.CountryName;
            customer.City = e.City;
            // customer.ProvinceRefId = e.ProvinceRefId;
            // customer.ProDescription = e.ProDescription;
            customer.ProInKhmer = e.ProInKhmer;
            // customer.DistrictRefId = e.DistrictRefId;
            //  customer.DisDescription = e.DisDescription;
            customer.DisInKhmer = e.DisInKhmer;
            //  customer.CommuneRefId = e.CommuneRefId;
            // customer.ComDescription = e.ComDescription;
            customer.ComInKhmer = e.ComInKhmer;
            //   customer.VillageRefId = e.VillageRefId;
            //   customer.VillDescription = e.VillDescription;
            customer.VilInKhmer = e.VilInKhmer;
            try
            {

                var resutl = await _otherNameService!.ConvertToCustomerAsync(e.EntId, customer);

                if (resutl.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Changing Entity", "Changing other name to customer was successful!");
                    customer = new();
                    await OnInitializedAsync();
                    await InvokeAsync(StateHasChanged);
                    return;
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Changing Entity", "Changing was failed! ");
                }
                ContextMenuService.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                NotificationService.Notify(NotificationSeverity.Success, "Changing Entity", "Changing was failed! ");

            }

        }
        else
        {
            Console.WriteLine("Canceled");
            ContextMenuService.Close();

            return;
        }
    }
    protected async Task ConvertToVedor(Guid? id)
    {
        var confirm = (bool)await DialogService.Confirm("Are you sure!, you want to this other name to model?", "Changing Entity",
            new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
        if (confirm)
        {
            var vendor = new VendorDtoCreate();

            var e = await _otherNameService.GetByIdAsync(id);

            if (e is null)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Changing Entity", "Getting other name information is empty!");

                return;
            }
            vendor.FirstName = e.FirstName;
            vendor.LastName = e.LastName;
            vendor.MiddleName = e.MiddleName;
            vendor.Name = e.Name;
            vendor.AltName = e.AltName;
            vendor.JobTitle = e.JobTitle;
            vendor.Salution = e.Sex;
            vendor.Phone = e.Phone;
            vendor.AltPhone = e.AltPhone;
            vendor.EmailAddress = e.EmailAddress;
            vendor.CompanyName = "N/A";
            vendor.EnType = "Vendor";
            // vendor.IDCard = e.IDCard;
            vendor.IsActive = e.IsActive;
            vendor.Description = e.Description;
            vendor.CountryName = e.CountryName;
            vendor.City = e.City;
            // vendor.ProvinceRefId = e.ProvinceRefId;
            // vendor.ProDescription = e.ProDescription;
            vendor.ProInKhmer = e.ProInKhmer;
            //  vendor.DistrictRefId = e.DistrictRefId;
            // vendor.DisDescription = e.DisDescription;
            vendor.DisInKhmer = e.DisInKhmer;
            // vendor.CommuneRefId = e.CommuneRefId;
            //  vendor.ComDescription = e.ComDescription;
            vendor.ComInKhmer = e.ComInKhmer;
            //  vendor.VillageRefId = e.VillageRefId;
            //   vendor.VillDescription = e.VillDescription;
            vendor.VilInKhmer = e.VilInKhmer;
            //var resutl = await _otherNameService!.ConvertToVendorAsync(1, vendor);
            //if (resutl.IsSuccess)
            //{
            //    NotificationService.Notify(NotificationSeverity.Success, "Changing Entity", "Changing other name to model was successful!");
            //    vendor = new();
            //    await OnInitializedAsync();
            //    StateHasChanged();
            //    return;
            //}
            //else
            //{
            //    NotificationService.Notify(NotificationSeverity.Success, "Changing Entity", "Changing was failed!");

            //}
            ContextMenuService.Close();

        }
        else
        {
            Console.WriteLine("Canceled");
            ContextMenuService.Close();

        }
    }
    protected async Task ConvertToEmployee(Guid? id)
    {
        var confirm = (bool)await DialogService.Confirm("Are you sure!, you want to this other name to employee?", "Changing Entity",
            new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
        if (confirm)
        {
            var model = new EmployeeDtoCreate();

            var e = await _otherNameService.GetByIdAsync(id);

            if (e is null)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Changing Entity", "Getting other name information is empty!");
                return;
            }
            model.FirstName = e.FirstName;
            model.LastName = e.LastName == "" ? "N/A" : e.LastName;
            model.MiddleName = e.MiddleName;
            model.Name = e.Name;
            model.AltName = e.AltName;
            model.DateOfBirth = DateTime.Now;
            model.JobTitle = e.JobTitle;
            model.Salutation = e.Sex;
            model.Gender = "N/A";
            model.Phone = e.Phone;
            model.AltPhone = e.AltPhone;
            model.EmailAddress = e.EmailAddress;
            model.EntType = "Employee";
            // model.IDCard = e.IDCard;
            model.IsActive = e.IsActive;
            model.Description = e.Description;
            model.CountryName = e.CountryName;
            model.City = e.City;
            //model.ProvinceRefId = e.ProvinceRefId;
            // model.ProDescription = e.ProDescription;
            model.ProInKhmer = e.ProInKhmer;
            //  model.DistrictRefId = e.DistrictRefId;
            //  model.DisDescription = e.DisDescription;
            model.DisInKhmer = e.DisInKhmer;
            //  model.CommuneRefId = e.CommuneRefId;
            //   model.ComDescription = e.ComDescription;
            model.ComInKhmer = e.ComInKhmer;
            //   model.VillageRefId = e.VillageRefId;
            //   model.VillDescription = e.VillDescription;
            model.VilInKhmer = e.VilInKhmer;
            //var resutl = await _otherNameService!.ConvertToEmployeeAsync(9, model);
            //if (resutl.IsSuccess)
            //{
            //    NotificationService.Notify(NotificationSeverity.Success, "Changing Entity", "Changing other name to model was successful!");
            //    model = new();
            //    await OnInitializedAsync();
            //    StateHasChanged();
            //    return;
            //}
            //else
            //{
            //    NotificationService.Notify(NotificationSeverity.Success, "Changing Entity", "Changing was failed!");

            //}
            ContextMenuService.Close();

        }
        else
        {
            Console.WriteLine("Canceled");

            ContextMenuService.Close();

        }
    }
    protected EntNameDto? OtherNames = new();

    public Index(IAddressService addressService, ISalutationService salutationService, IOtherNameServiceImpl otherNameService)
    {
        _addressService = addressService;
        _salutationService = salutationService;
        _otherNameService = otherNameService;
    }

    protected async Task DisplayDetail(long e)
    {
        if (e != 0)
        {
            try
            {
                //OtherNames = await _otherNameService!.GetByIdAsync(e);
                IsEnableDetail = true;
                selectedIndex = 3;
                ContextMenuService.Close();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Exception", ex.Message);
            }

        }
    }
    async Task OnDelete(Guid? id)
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




}





















