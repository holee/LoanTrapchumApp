using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using TCLMS.Shared.Entities.Dtos.Lists;
using TCLMS.Shared.Entities.Dtos.Lists.Addreses;
using TCLMS.Shared.Entities.Dtos.Lists.Customers;
using TCLMS.Shared.Entities.Dtos.Lists.Entities;
using TCLMS.Shared.Entities.Models.Addresses;
using TCLMS.Shared.Entities.Models.Lists;

namespace TCLMS.Front.Pages.Lists.Customers
{
    public partial class CustomerCreateModal
    {
        [Parameter] public IEnumerable<string> Salutations { get; set; } = [];
        [Parameter] public IEnumerable<ProvinceDto> Provinces { get; set; } = [];
        [Parameter] public IEnumerable<string> Cities { get; set; } = [];
        [Parameter] public IEnumerable<StOcupationDto> Occupations { get; set; } = [];
        [Parameter] public IEnumerable<Country> Countries { get; set; } = [];
        [Parameter] public EventCallback Closed { get; set; } = default!;
        protected CustomerDtoCreate model = new();
        protected IEnumerable<DistrictDto?>? DistrictList = [];
        protected IEnumerable<CommuneDto?>? CommuneList = [];
        protected IEnumerable<VillageDto?>? VillageList = [];
        protected EditForm? editForm;
        private bool IsLoading = true;
        protected string? newSalution = "";
        [Parameter] public EventCallback<int?> SelectedValueChanged { get; set; }
        protected override async Task OnInitializedAsync()
        {
            model.City = "Siem Reap";
            model.CountryName = "ប្រទេសកម្ពុជា";
            model.Salutation = "Mr";
            model.MarritalStatus = "Single";
            model.Nationality = "Khmer";
            await base.OnInitializedAsync();
        }
        private void AssignImageUrl(string imgUrl) => model.Photo = imgUrl ?? string.Empty;
        protected async void FormSubmit()
        {
            editForm?.EditContext?.Validate();
            try
            {
                model.EnType = "Customer";

                var response = await _customerService.CreateCustomerAsync(model);

                if (response.IsSuccess)
                {

                    NotificationService.Notify(Radzen.NotificationSeverity.Success, "Customer was created successfully!");
                }
                else
                {
                    var message = response.Errors!.First();

                    NotificationService.Notify(Radzen.NotificationSeverity.Error, message);
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.Message);
            }
        }


        protected async void GetDistrictsByprovince()
        {
            DistrictList = await _addressService.GetDistrictByProvinceAsync(model.ProvinceRefId);
            StateHasChanged();
        }
        protected async void GetCommunesByDistrict()
        {
            CommuneList = await _addressService.GetCommuneByDistricteAsync(model.DistrictRefId);
            StateHasChanged();
        }
        protected async void GetVillagesByCommune()
        {
            VillageList = await _addressService.GetVillageByCommuneAsync(model.CommuneRefId);
            StateHasChanged();
        }

        protected async void SelectedVillageValueChanged(VillageDto village)
        {
            await Task.Delay(100);
            if (village is not null)
            {
                //SelectedVillage= village;
            }
            else
            {
                // SelectedVillage = null;
            }
            StateHasChanged();
        }

        protected async Task AddNewSalutation(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                await _salutationService.AddSalution(new Salutation
                {
                    Title = newSalution
                });
                await GetSalutations();
            }
        }
        protected void OnTextChangedSalution(ChangeEventArgs e)
        {
            newSalution = e?.Value?.ToString();
        }
        protected async Task<IEnumerable<string>?> SearchSalutaions(string searchText = "")
        {
            var salution = Salutations?.Where(x => x.StartsWith(searchText, StringComparison.InvariantCultureIgnoreCase))
                                     .ToList();
            return await Task.FromResult(salution);
        }
        protected async void Close()
        {
            await Closed.InvokeAsync();
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


        //protected CustomerDtoCreate model =new CustomerDtoCreate();
        //protected IEnumerable<ProvinceDto>? ProvinceList = []; 
        //protected IEnumerable<DistrictDto>? DistrictList = [];
        //protected IEnumerable<CommuneDto>? CommuneList = [];
        //protected IEnumerable<VillageDto>? VillageList = [];
        //protected IEnumerable<StOcupationDto>? Occupations = [];
        //protected ProvinceDto? SelectedProvince;
        //protected DistrictDto? SelectedDistrict;
        //protected CommuneDto? SelectedCommune;
        //protected VillageDto? SelectedVillage;
        //protected EditForm? editForm;
        //protected IList<string>? Countries=new List<string>();
        //protected IEnumerable<string>? Cities=new List<string>();
        //protected IEnumerable<string>? Salutions = new List<string>(); 
        //private bool IsLoading = true;
        //protected string? newSalution = "";
        //[Parameter]public EventCallback<int?> SelectedValueChanged { get; set; } 
        //protected override async Task OnInitializedAsync()
        //{
        //    try
        //    {
        //        model.City = "Siem Reap";
        //        model.CountryName = "ប្រទេសកម្ពុជា";
        //        model.Salutation = "Mr";
        //        await GetCountries();
        //        await GetCities();  
        //        ProvinceList = await _addressService.GetAllProvinceAsync();
        //        Occupations =  await _occupationService.GetOccupationsAsync();
        //        await GetSalutations();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    await base.OnInitializedAsync();
        //}



        //protected async void FormSubmit()
        //{

        //    model.ProInKhmer = SelectedProvince?.ProInKhmer??string.Empty;
        //    model.DisInKhmer=SelectedDistrict?.DisInKhmer??string.Empty;
        //    model.ComInKhmer = SelectedCommune?.ComInKhmer ?? string.Empty;
        //    model.VilInKhmer = SelectedVillage?.VilInKhmer;
        //    editForm?.EditContext?.Validate();
        //    try
        //    {
        //        model.EnType = "Customer";
        //        var response = await _customerService.CreateCustomerAsync(model);
        //        if(response.IsSuccess){

        //            NavigationManager.NavigateTo("/Customers/Center");
        //        }
        //        else
        //        {
        //            NotificationService.Notify(Radzen.NotificationSeverity.Success, response.Data!.message);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        NotificationService.Notify(Radzen.NotificationSeverity.Error,$"{ex.Message}");
        //    }
        //}

        //private async Task GetCountries()
        //{
        //    var countries = await _addressService.GetCountries(105);
        //}
        //private async Task GetCities()
        //{
        //    Cities = await _addressService.GetCities();
        //}



        //protected async Task AddNewSalutation(KeyboardEventArgs e)
        //{
        //    if (e.Code == "Enter")
        //    {
        //        await _nameService.AddSalution(new Salutation
        //        {
        //            Title=newSalution
        //        });
        //        await GetSalutations();
        //    }
        //}
        //protected void OnTextChangedSalution(ChangeEventArgs e)
        //{
        //    newSalution = e?.Value?.ToString();
        //}

        //private async Task GetSalutations()
        //{
        //    var salutations = await _nameService.GetSalutions();
        //    Salutions=salutations.IsSuccess 
        //                         ?salutations!.Data!.Select(x => x.Title!).ToList()
        //                         :Salutions = [];

        //}
    }
}
