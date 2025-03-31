using Applications.Extensions.mappers;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses.Lists.Entities;
using Applications.Services.Lists;
using Domains.Models.Addresses;
using Domains.Models.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace TrapchumApp.Components.Pages.Lists.Employees
{
    public partial class EmployeeEdit
    {
        private readonly IDepartmentServiceImpl _departmentService;
        [Parameter] public IEnumerable<ProvinceDto> Provinces { get; set; } = [];
        [Parameter] public IEnumerable<BranchDto> Branches { get; set; } = [];
        [Parameter] public IEnumerable<DepartmentDto> Departments { get; set; } = [];
        [Parameter] public IEnumerable<Country> Countries { get; set; } = [];
        [Parameter] public IEnumerable<Nationality> Nationalities { get; set; } = [];
        [Parameter] public IEnumerable<OcupationResponse> Occuptions { get; set; } = [];
        [Parameter] public IEnumerable<string> Cities { get; set; } = [];
        [Parameter] public EventCallback Close { get; set; }
        [Parameter] public EntityNamesCreate model { get; set; } = new();
        protected IEnumerable<CommuneDto>? Communes;
        protected IEnumerable<DistrictDto>? Districts;
        protected IEnumerable<VillageDto>? Villages;
        protected EditForm? editForm;
        protected string? strText = "";
        protected bool IsActive = false;

        protected async void SelectedProvinceChanged(object arg)
        {
            if (arg is not null)
            {
                model.ProInKhmer = ((ProvinceDto)arg).ProInKhmer;

                Districts = await _addressService!.GetDistrictsByProvinceId(((ProvinceDto)arg).ProvinceId);
                StateHasChanged();
            }
        }

        protected async void SelectedDistrictChanged(object arg)
        {
            if (arg is not null)
            {
                model.DisInKhmer = ((DistrictDto)arg).DisInKhmer;
                Communes = await _addressService!.GetCommunesByDistrictId(((DistrictDto)arg).DistrictId);
                StateHasChanged();
            }
        }

        protected async void SelectedCommuneChanged(object arg)
        {
            if (arg is not null)
            {
                model.ComInKhmer = ((CommuneDto)arg).ComInKhmer;
                Villages = await _addressService!.GetVillagesByCommuneId(((CommuneDto)arg).CommuneId);
                StateHasChanged();
            }
        }

        protected void SelectedVillageChanged(object arg)
        {
            if (arg is not null)
            {
                model.VilInKhmer = ((VillageDto)arg).VilInKhmer;
                StateHasChanged();
            }
        }

        protected async Task FormValidSubmit()
        {
            try
            {
                editForm?.EditContext?.Validate();

                model.Name = model.FirstName;

                var updateResponse = await _employeeService.UpdateAsync(model);

                if (updateResponse.IsSuccess)
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Success, updateResponse.Data);
                    model = new();
                    await CloseTab();

                    Reload();
                }
                else
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, updateResponse.ErrMessage);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Success, ex.Message);
            }
        }

        bool clickedEmploymen = true;

        async Task LoadEmployment()
        {
            if (clickedEmploymen)
            {
                var results = await _branchService!.GetBranchesAsync(0, 0, false);

                if (results.IsSuccess)
                {
                    Branches = results!.Data!.Maps<BranchDto>();
                }

                var deptResponse = await _departmentService!.GetDepartments(0, 0);

                if (deptResponse.IsSuccess)
                {
                    Departments = deptResponse.Data!.Maps<DepartmentDto>();
                }

                Occuptions = await _employeeService!.GetOccupationsAsync("Position");

                clickedEmploymen = false;

                StateHasChanged();
            }
        }

        bool clicked = true;

        async Task LoadAddress()
        {
            if (clicked)
            {
                OnInitialized();
            }

            clicked = false;
            await InvokeAsync(StateHasChanged);
        }

        protected override async void OnInitialized()
        {
            Provinces = await _addressService.GetsProvinces();
            Countries = await _addressService.GetCountries(105);
            Cities = await _addressService.GetCities();
            if (model is not null)
            {
                if (model.DistrictRefId.HasValue)
                {
                    Districts = await _addressService.GetDistrictsByProvinceId(model?.ProvinceRefId);
                }

                if (model!.DistrictRefId.HasValue)
                {
                    Communes = await _addressService.GetCommunesByDistrictId(model?.DistrictRefId);
                }

                if (model!.DistrictRefId.HasValue)
                {
                    Villages = await _addressService.GetVillagesByCommuneId(model?.CommuneRefId);
                }

                await InvokeAsync(StateHasChanged);
            }

            base.OnInitialized();
        }

        protected void OnToList()
        {
            NavigationManager?.NavigateTo("/Employees/List");
        }

        protected async Task AddCity(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                var city = new CityDtoCreate { CityName = strText };

                var message = await _addressService.AddCity(city);

                if (message!.IsSuccess)
                {
                    Cities = await _addressService.GetCities();

                    StateHasChanged();
                }
            }
        }

        protected void SearchCities(ChangeEventArgs e)
        {
            if (e.Value is not null)
            {
                strText = e?.Value?.ToString();
            }
        }

        private ElementReference _input;

        [Inject]
        //public IFileReaderService? FileReaderService { get; set; }
        //[Inject]
        public HttpClient? http { get; set; }

        string? ImgUrl;

        public EmployeeEdit(IDepartmentServiceImpl departmentService)
        {
            _departmentService = departmentService;
        }

        //private async Task HandleSelected()
        //{

        //    foreach (var file in await FileReaderService!.CreateReference(_input).EnumerateFilesAsync())
        //    {
        //        if (file != null)
        //        {
        //            var fileInfo = await file.ReadFileInfoAsync();
        //            using (var ms = await file.CreateMemoryStreamAsync(4 * 1024))
        //            {
        //                var content = new MultipartFormDataContent();
        //                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
        //                content.Add(new StreamContent(ms, Convert.ToInt32(ms.Length)), "image", fileInfo.Name);
        //                var postResult = await http!.PostAsync("https://localhost:7237/api/Files", content);
        //                var postContent = await postResult.Content.ReadAsStringAsync();
        //                if (!postResult.IsSuccessStatusCode)
        //                {
        //                    throw new ApplicationException(postContent);
        //                }
        //                else
        //                {
        //                    ImgUrl = Path.Combine("https://localhost:7237/", postContent);
        //                    model.Photo = ImgUrl;

        //                }
        //                await InvokeAsync(StateHasChanged);
        //            }
        //        }
        //    }
        //}
    }
}