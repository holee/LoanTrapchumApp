using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses.Lists.Entities;
using Applications.Services.Files;
using Applications.Services.Lists;
using Domains.Models.Addresses;
using Domains.Models.Files;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Radzen;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerCreate
    {

        private readonly IFileUploadServiceImp _fileUploadService;
        private readonly INationalityServiceImpl _nationalityService;
        private readonly IOccupationServiceImpl _occupationService;
        private readonly ISalutationServiceImpl _salutationService;
        public CustomerCreate(IFileUploadServiceImp fileUploadService, INationalityServiceImpl ationalityService, IOccupationServiceImpl occupationService, ISalutationServiceImpl salutationService)
        {
            _fileUploadService = fileUploadService;
            _nationalityService = ationalityService;
            _occupationService = occupationService;
            _salutationService = salutationService;
        }

        [Parameter] public EventCallback Closed { get; set; } = default!;
        [Parameter] public EventCallback<int?> SelectedValueChanged { get; set; }
        IEnumerable<string> Salutations = [];
        IEnumerable<ProvinceDto> Provinces = [];
        IEnumerable<string> Cities = [];
        IEnumerable<OcupationResponse> Occupations = [];
        IEnumerable<Country> Countries = [];
        protected EntityNamesCreate model = new();
        protected IEnumerable<DistrictDto> Districts = [];
        protected IEnumerable<CommuneDto> Communes = [];
        protected IEnumerable<VillageDto> Villages = [];
        protected EditForm? editForm;
        private bool IsLoading = true;
        protected string? newSalution = "";
        protected override async Task OnInitializedAsync()
        {

            await GetAdditionalInformation();
            model.City = "Siem Reap";
            model.CountryName = "ប្រទេសកម្ពុជា";
            model.Salutation = "Mr";
            model.MarritalStatus = "Single";
            model.Nationality = "Khmer";
            
            var rsp1 = await _salutationService.GetSalutions();

            if (rsp1.IsSuccess)
            {
                Salutations = rsp1.Data.Select(x => x.Title).ToList();
            }
            await base.OnInitializedAsync();
        }

        IEnumerable<Nationality> Nationalities { get; set; } = [];
        IEnumerable<string> Statuses = ["Single", "Married", "Divorced", "Widow"];
        RadzenFileInput<string> fileUpload = default!;
        string selectedTab = "info";
        string city = "";
        const long maxFileSize = 30_000_000;
        void OnChange(byte[] args, string name)
        {
            // using var content = new MultipartFormDataContent();
            // var ms = new MemoryStream(args);
            // var fileContent = new StreamContent(ms);
            // fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            // content.Add(content: fileContent, name: "\"files\"", fileName: fileUpload.Name);
            // _fileUploadService.UploadFile(content);

            // Console.WriteLine(args);
        }

        void OnError(UploadErrorEventArgs args, string name)
        {

        }
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
        async Task GetAdditionalInformation()
        {
            Nationalities = await _nationalityService.GetNationalities();

            Provinces = await _addressService.GetsProvinces();

            Occupations = await _employeeService!.GetOccupationsAsync("Position");

            var rsp = await _addressService.GetCountries(105);

            if(rsp.IsSuccess)
            {
                Countries = rsp.Data ?? [];
            }   
            Cities = await _addressService.GetCities();
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
        private async Task GetCities()
        {
            Cities = await _addressService.GetCities();
        }
        private async Task GetOccupation()
        {
            var pager = await _occupationService.GetOccupations<OcupationResponse>(0, string.Empty);
            Occupations = pager.Items;
        }
        private async Task GetProvinces()
        {
            Provinces = await _addressService.GetsProvinces();
        }
        private async Task GetCountries()
        {
            var rsp = await _addressService.GetCountries(105);

            if (rsp.IsSuccess)
            {
                Countries = rsp.Data ?? [];
            }
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



        private void AssignImageUrl(string imgUrl) => model.Photo = imgUrl ?? string.Empty;
        protected async void FormSubmit()
        {
            try
            {
                model.EntType = "Customer";

                var response = await _customerService.CreateAsync(model);

                if (response.IsSuccess)
                {

                    NotificationService.Notify(NotificationSeverity.Success, "Customer was created successfully!");
                }
                else
                {

                    NotificationService.Notify(NotificationSeverity.Error, response.ErrMessage);
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
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

    }


}


