using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses.Lists.Entities;
using Applications.Services.Files;
using Applications.Services.Lists;
using Domains.Models.Files;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TrapchumApp.Components.Pages.Lists.Customers
{
    public partial class CustomerEdit
    {
        private readonly IOccupationServiceImpl _occupationService;
        private readonly IFileUploadServiceImp _fileUploadService;

        public CustomerEdit(IOccupationServiceImpl occupationService, IFileUploadServiceImp fileUploadService)
        {
            _occupationService = occupationService;
            _fileUploadService = fileUploadService;
        }

        [Parameter] public Guid? CustomerId { get; set; }
        [Parameter] public IEnumerable<ProvinceDto> Provinces { get; set; } = [];
        [Parameter] public IEnumerable<string> Salutations { get; set; } = [];
        [Parameter] public IEnumerable<string> Cities { get; set; } = [];
        [Parameter] public IEnumerable<string> Countries { get; set; } = [];
        [Parameter] public EventCallback<int?> SelectedValueChanged { get; set; }
        protected EntityNamesCreate model = new();
        protected IEnumerable<ProvinceDto>? ProvinceList = [];
        protected IEnumerable<DistrictDto> DistrictList = [];
        protected IEnumerable<CommuneDto> CommuneList = [];
        protected IEnumerable<VillageDto> VillageList = [];
        protected IEnumerable<OcupationResponse>? Occupations = [];
        protected EditForm editForm = default!;
        private IEnumerable<string> Statuses = ["Single", "Married", "Divorced", "Widow"];
        private bool IsLoading = true;
        protected string? newSalution = "";
        string city = "";

        void OnChanged(ChangeEventArgs e)
        {
            city = e!.Value!.ToString()!;
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

        void OnProgress(UploadProgressArgs args, string name)
        {
            if (args.Progress == 100)
            {
                foreach (var file in args.Files)
                {
                }
            }
        }

        async Task HandleFileUpload(UploadChangeEventArgs args, string name)
        {
            if (args.Files.Count() == 0)
            {
                NotificationService.Notify(NotificationSeverity.Info, "No file is selected.");
                return;
            }

            var file = args.Files.FirstOrDefault();

            if (file is null)
            {
                NotificationService.Notify(NotificationSeverity.Info, "File is null");
                return;
            }

            var ms = new MemoryStream();

            await file.OpenReadStream().CopyToAsync(ms);

            string fileName = file.Name;

            string fileExtension = Path.GetExtension(fileName);

            long fileSize = file.Size;

            var uploadFile = new FileUploadModel(fileName, fileExtension, fileSize, ms.ToArray());

            var resp = await _fileUploadService.UploadFile(uploadFile);

            if (!resp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Info, resp.ErrMessage);

                return;
            }

            model.Photo = resp.Data?.FileUrl;

            StateHasChanged();
        }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                ProvinceList = await _addressService.GetsProvinces();

                var occ = await _occupationService.GetOccupations<OcupationResponse>(0, string.Empty);

                Occupations = occ.Items ?? [];

                var rsp1 = await _salutationService.GetSalutions();

                if (rsp1.IsSuccess)
                {
                    Salutations = rsp1.Data.Select(x => x.Title).ToList();
                }

                var rsp = await _customerService.GetCustomerUpdateAsync(CustomerId);


                if (rsp.IsSuccess)
                {
                    model = rsp.Data!;

                    if (model is not null)
                    {
                        if (model!.ProvinceRefId!.HasValue)
                        {
                            this.DistrictList = await _addressService.GetDistrictsByProvinceId(model?.ProvinceRefId);
                        }

                        if (model!.DistrictRefId.HasValue)
                        {
                            this.CommuneList = await _addressService.GetCommunesByDistrictId(model?.DistrictRefId);
                        }

                        if (model!.CommuneRefId.HasValue)
                        {
                            this.VillageList = await _addressService.GetVillagesByCommuneId(model?.CommuneRefId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            await base.OnInitializedAsync();
        }

        void Close() => DialogService.Close(false);

        protected async Task FormSubmit()
        {
            editForm.EditContext?.Validate();
            try
            {
                model!.EntType = "Customer";

                var response = await _customerService.UpdateAsync(model);

                if (response.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, response.Data);

                    NavigationManager.NavigateTo($"/App/Customers/{CustomerId}/Details");
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, response.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.InnerException?.Message);
            }
        }

        protected async void SelectedDistrictsByprovince()
        {
            DistrictList = await _addressService.GetDistrictsByProvinceId(model!.ProvinceRefId);
        }

        protected async void SelectedCommunesByDistrict()
        {
            CommuneList = await _addressService.GetCommunesByDistrictId(model!.DistrictRefId);
        }

        protected async void SelectedVillagesByCommune()
        {
            VillageList = await _addressService.GetVillagesByCommuneId(model!.CommuneRefId);
        }

        private async Task GetCountries()
        {
            var countries = await _addressService.GetCountries(105);
            Countries = countries.Where(x => x.CountryId == 105)
                .Select(x => x.DesInKhmer!)
                .ToList();
        }

        private async Task GetCities()
        {
            Cities = await _addressService.GetCities();
        }

        protected async void GetDistrictsByprovince()
        {
            DistrictList = await _addressService.GetDistrictsByProvinceId(model!.ProvinceRefId);
            StateHasChanged();
        }

        protected async void GetCommunesByDistrict()
        {
            CommuneList = await _addressService.GetCommunesByDistrictId(model!.DistrictRefId);
            StateHasChanged();
        }

        protected async void GetVillagesByCommune()
        {
            VillageList = await _addressService.GetVillagesByCommuneId(model!.CommuneRefId);
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


        private async Task GetSalutations()
        {
            var salutations = await _salutationService.GetSalutions();
            Salutations = salutations.IsSuccess
                ? salutations!.Data!.Select(x => x.Title!).ToList()
                : Salutations = [];
        }
    }
}