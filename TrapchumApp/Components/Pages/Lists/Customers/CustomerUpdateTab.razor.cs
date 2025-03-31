using Applications.Interfaces.Addresses;
using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Customers;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses.Lists.Entities;
using Applications.Services.Customers;
using Applications.Services.Files;
using Domains.Models.Addresses;
using Domains.Models.Files;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TrapchumApp.Components.Pages.Lists.Customers
{
    public partial class CustomerUpdateTab
    {
        private readonly INationalityService _nationalityService;

        private readonly IFileUploadServiceImp _fileUploadService;

        public CustomerUpdateTab(ISalutationService salutationService, INationalityService nationalityService, IFileUploadServiceImp fileUploadService)
        {
            _salutationService = salutationService;
            _nationalityService = nationalityService;
            _fileUploadService = fileUploadService;
        }



        string selectedTab = "info";

        string city = "";

        private Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;
            return Task.CompletedTask;
        }
        void OnChanged(ChangeEventArgs e)
        {
            city = e!.Value!.ToString()!;
        }
        void OnProgress(UploadProgressArgs args, string name)
        {
            if (args.Progress == 100)
            {
                foreach (var file in args.Files)
                {

                }
            }
        }
        async Task OnImageChange(UploadChangeEventArgs args, string name)
        {
            if (args.Files.Count() == 0) return;

            var file = args.Files.FirstOrDefault();

            if (file is null) return;

            using var ms = new MemoryStream();

            await file.OpenReadStream().CopyToAsync(ms);
            var fileName = file.Name;
            var fileExtension = Path.GetExtension(file.Name);
            var fileSize = file.Size;
            var fileUpload = new FileUploadModel(fileName, fileExtension, fileSize, ms.ToArray());
            var resp = await _fileUploadService.UploadFile(fileUpload);
            if (!resp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Info, resp.ErrMessage);
                return;
            }

            model.Photo = resp.Data?.FileUrl;
            StateHasChanged();
        }
        async Task OnKeyPress(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                await _addressService.AddCity(new CityDtoCreate
                {
                    CityName = city
                });
                Cities = await _addressService.GetCities();
            }

        }
        async void SelectedProvinceChanged(object arg)
        {
            if (arg is not null)
            {
                model.ProInKhmer = ((ProvinceDto)arg).ProInKhmer;
                Districts = await _addressService!.GetDistrictsByProvinceId(((ProvinceDto)arg).ProvinceId);
                StateHasChanged();
            }

        }
        async void SelectedDistrictChanged(object arg)
        {
            if (arg is not null)
            {
                model.DisInKhmer = ((DistrictDto)arg).DisInKhmer;
                Communes = await _addressService!.GetCommunesByDistrictId(((DistrictDto)arg).DistrictId);
                StateHasChanged();
            }
        }
        async void SelectedCommuneChanged(object arg)
        {
            if (arg is not null)
            {
                model.ComInKhmer = ((CommuneDto)arg).ComInKhmer;
                Villages = await _addressService!.GetVillagesByCommuneId(((CommuneDto)arg).CommuneId);
                StateHasChanged();
            }

        }
        void SelectedVillageChanged(object arg)
        {
            if (arg is not null)
            {
                model.VilInKhmer = ((VillageDto)arg).VilInKhmer;
                StateHasChanged();
            }

        }


        async Task GetAdditionalInformation()
        {
            Nationalities = await _nationalityService!.GetNationalities();

            Provinces = await _addressService.GetsProvinces();

            Occupations = await _employeeService!.GetOccupationsAsync("Position");

            Countries = await _addressService.GetCountries(105);

            Cities = await _addressService.GetCities();
        }
        [Parameter] public EventCallback Closed { get; set; } = default!;
        [Parameter] public EventCallback<int?> SelectedValueChanged { get; set; }
        [Parameter] public Guid? CustomerId { get; set; }
        IEnumerable<string> Salutations = [];
        IEnumerable<ProvinceDto> Provinces = [];
        IEnumerable<string> Cities = [];
        IEnumerable<OcupationResponse> Occupations = [];
        IEnumerable<Country> Countries = [];
        EntityNamesCreate model = new();
        IEnumerable<DistrictDto> Districts = [];
        IEnumerable<CommuneDto> Communes = [];
        IEnumerable<VillageDto> Villages = [];
        EditForm? editForm;
        bool IsLoading = true;
        string? newSalution = "";
        IEnumerable<Nationality> Nationalities = [];
        IEnumerable<string> Statuses = ["Single", "Married", "Divorced", "Widow"];


        protected override async Task OnParametersSetAsync()
        {
            await GetAdditionalInformation();
            await base.OnParametersSetAsync();
        }
        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            var rsp = await _customerService.GetCustomerUpdateAsync(CustomerId);

            if (rsp.IsSuccess)
            {

                model = rsp.Data!;

                if (model!.ProvinceRefId > 0)
                {
                    Districts = await _addressService.GetDistrictsByProvinceId(model?.ProvinceRefId);
                }
                if (model!.DistrictRefId > 0)
                {
                    Communes = await _addressService.GetCommunesByDistrictId(model?.DistrictRefId);
                }
                if (model!.CommuneRefId > 0)
                {
                    Villages = await _addressService.GetVillagesByCommuneId(model?.CommuneRefId);
                }

            }
            else
            {
                model.City = "Siem Reap";
                model.CountryName = "ប្រទេសកម្ពុជា";
                model.Salutation = "Mr";
                model.MarritalStatus = "Single";
                model.Nationality = "Khmer";
                NotificationService.Notify(Radzen.NotificationSeverity.Error, rsp.ErrMessage);
            }

            IsLoading = false;

            await base.OnInitializedAsync();
        }
        protected async void FormSubmit()
        {
            editForm?.EditContext?.Validate();
            try
            {
                model.EntType = "Customer";

                var rsp = await _customerService.UpdateAsync(model);

                if (rsp.IsSuccess)
                {

                    NotificationService.Notify(Radzen.NotificationSeverity.Success, "Customer was created successfully!");
                }
                else
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, rsp.ErrMessage);
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.Message);
            }
        }


        protected async void GetDistrictsByprovince()
        {
            Districts = await _addressService.GetDistrictsByProvinceId(model.ProvinceRefId);
            StateHasChanged();
        }
        protected async void GetCommunesByDistrict()
        {
            Communes = await _addressService.GetCommunesByDistrictId(model.DistrictRefId);
            StateHasChanged();
        }
        protected async void GetVillagesByCommune()
        {
            Villages = await _addressService.GetVillagesByCommuneId(model.CommuneRefId);
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
    }

}

