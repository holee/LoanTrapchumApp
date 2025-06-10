using Applications.Enumerations;
using Applications.Interfaces.Lists;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses.Lists.Entities;
using Applications.Services.Files;
using Applications.Services.Lists;
using Domains.Models.Addresses;
using Domains.Models.Files;
using Domains.Models.Lists;
namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerUpdateTab
    {
        private readonly INationalityService _nationalityService;

        private readonly IFileUploadServiceImp _fileUploadService;

        private readonly ISalutationServiceImpl _salutationService;

        public CustomerUpdateTab(INationalityService nationalityService, IFileUploadServiceImp fileUploadService, ISalutationServiceImpl salutationService)
        {
            _nationalityService = nationalityService;
            _fileUploadService = fileUploadService;
            _salutationService = salutationService;
        }

        [Parameter] public EventCallback<int> Closed { get; set; }
        [Parameter] public bool ShowEditTab { get; set; } = false;

        string selectedTab = "info";

        string city = "";
        async Task ClickCloseTab()
        {
            await Closed.InvokeAsync(2);
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
        async Task GetAdditionalInformation()
        {
            Nationalities = await _nationalityService!.GetNationalities();

            Occupations = await _employeeService!.GetOccupationsAsync("Position");

            var rsp = await _addressService.GetCountries(105);

            if (rsp.IsSuccess)
            {
                Countries = rsp.Data ?? [];
            }

            Cities = await _addressService.GetCities();

            var rsp1 = await _salutationService.GetSalutions();

            if (rsp1.IsSuccess)
            {
                Salutations = rsp1.Data!.Select(x => x.Title!).ToList();
            }
        }
        [Parameter] public EventCallback<int?> SelectedValueChanged { get; set; }
        [Parameter] public string? CustomerId { get; set; }
        IEnumerable<string> Salutations = [];
        IEnumerable<string> Cities = [];
        IEnumerable<OcupationResponse> Occupations = [];
        IEnumerable<Country> Countries = [];
        EntityCustomerCreate model = new();
        EditForm? editForm;
        bool IsLoading = true;
        string? newSalution = "";
        IEnumerable<Nationality> Nationalities = [];
        IEnumerable<string> Statuses = ["Single", "Married", "Divorced", "Widow", "Others"];

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            await GetAdditionalInformation();

            var rsp = await _customerService.GetCustomerUpdateAsync(CustomerId);

            if (rsp.IsSuccess)
            {
                model = rsp.Data!;
            }
            else
            {
                model.City = "Siem Reap";
                model.CountryName = "ប្រទេសកម្ពុជា";
                model.Salutation = "Mr";
                model.MarritalStatus = "Single";
                model.Nationality = "Khmer";
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }

            IsLoading = false;

            await base.OnInitializedAsync();
        }
        protected async void FormSubmit()
        {
            editForm?.EditContext?.Validate();
            try
            {
                model.EntType = EntityTypes.Customer.ToString();

                var rsp = await _customerService.UpdateAsync(model);

                if (rsp.IsSuccess)
                {
                    await CloseTab(2);

                    NotificationService.Notify(NotificationSeverity.Success, "Customer was edited successfully!");
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
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
        protected async Task CloseTab(int selectedIndex)
        {
            await Closed.InvokeAsync(selectedIndex);
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

