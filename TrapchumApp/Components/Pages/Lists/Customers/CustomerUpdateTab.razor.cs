using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using TCLMS.Shared.Entities.Dtos.Lists;
using TCLMS.Shared.Entities.Dtos.Lists.Addreses;
using TCLMS.Shared.Entities.Dtos.Lists.Customers;
using TCLMS.Shared.Entities.Dtos.Lists.Entities;
using TCLMS.Shared.Entities.Models.Addresses;
using TCLMS.Shared.Entities.Models.Lists;
using TCLMS.Shared.Entities.Models.Loans;

namespace TCLMS.Front.Pages.Lists.Customers
{
    public partial class CustomerUpdateTab
    {

        [Parameter] public EventCallback Closed { get; set; } = default!;
        [Parameter] public EventCallback<int?> SelectedValueChanged { get; set; }
        [Parameter] public long CustomerId { get; set; }
        IEnumerable<string> Salutations = [];
        IEnumerable<ProvinceDto> Provinces = [];
        IEnumerable<string> Cities = [];
        IEnumerable<StOcupationDto> Occupations = [];
        IEnumerable<Country> Countries = [];
        CustomerDtoUpdate model = new();
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
            var customerResponse = await _customerService.GetCustomerUpdateAsync(CustomerId);

            if (customerResponse.IsSuccess)
            {

                model = customerResponse.Data!;

                if (model!.ProvinceRefId > 0)
                {
                    this.Districts = await _addressService.GetDistrictByProvinceAsync(model?.ProvinceRefId);
                }
                if (model!.DistrictRefId > 0)
                {
                    this.Communes = await _addressService.GetCommuneByDistricteAsync(model?.DistrictRefId);
                }
                if (model!.CommuneRefId > 0)
                {
                    this.Villages = await _addressService.GetVillageByCommuneAsync(model?.CommuneRefId);
                }

            }
            else
            {
                model.City = "Siem Reap";
                model.CountryName = "ប្រទេសកម្ពុជា";
                model.Salutation = "Mr";
                model.MarritalStatus = "Single";
                model.Nationality = "Khmer";
                var error = customerResponse.Errors!.FirstOrDefault();
                NotificationService.Notify(Radzen.NotificationSeverity.Error, error);
            }

            IsLoading = false;

            await base.OnInitializedAsync();
        }
        protected async void FormSubmit()
        {
            editForm?.EditContext?.Validate();
            try
            {
                model.EnType = "Customer";

                var response = await _customerService.UpdateCustomerAsync(CustomerId, model);

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
            Districts = await _addressService.GetDistrictByProvinceAsync(model.ProvinceRefId);
            StateHasChanged();
        }
        protected async void GetCommunesByDistrict()
        {
            Communes = await _addressService.GetCommuneByDistricteAsync(model.DistrictRefId);
            StateHasChanged();
        }
        protected async void GetVillagesByCommune()
        {
            Villages = await _addressService.GetVillageByCommuneAsync(model.CommuneRefId);
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


//[Parameter] public long id { get; set; }
//[Parameter] public EventCallback<int?> SelectedValueChanged { get; set; }
//protected CustomerDtoUpdate? model = new();
//protected IEnumerable<ProvinceDto>? ProvinceList = Enumerable.Empty<ProvinceDto>();
//protected IEnumerable<DistrictDto>? DistrictList = Enumerable.Empty<DistrictDto>();
//protected IEnumerable<CommuneDto>? CommuneList = Enumerable.Empty<CommuneDto>();
//protected IEnumerable<VillageDto>? VillageList = Enumerable.Empty<VillageDto>();
//protected IEnumerable<StOcupationDto>? Occupations = Enumerable.Empty<StOcupationDto>();
//protected EditForm editForm = default!;
//private bool IsLoading = true;
//protected IEnumerable<string>? Salutions = new List<string>();
//protected IList<string?> Countries = [];
//protected IEnumerable<string>? Cities = Enumerable.Empty<string>();
//protected string? newSalution = "";
//protected ProvinceDto? SelectedProvince;
//protected DistrictDto? SelectedDistrict;
//protected CommuneDto? SelectedCommune;
//protected VillageDto? SelectedVillage;
//string city = "";
//void OnChanged(ChangeEventArgs e)
//{
//    city = e!.Value!.ToString()!;
//}
//async Task OnKeyPress(KeyboardEventArgs e)
//{
//    if (e.Code == "Enter")
//    {
//        await _addressService.AddCity(new CityDtoCreate
//        {
//            CityName = city
//        });
//        Cities = await _addressService.GetCities();
//    }

//}
//protected override async Task OnInitializedAsync()
//{
//    try
//    {
//        await GetCountries();
//        await GetCities();
//        await GetSalutations();
//        ProvinceList = await _addressService.GetAllProvinceAsync();
//        Occupations = await _occupationService.GetOccupationsAsync();
//        ProvinceList = await _addressService.GetAllProvinceAsync();
//        Occupations = await _occupationService.GetOccupationsAsync();
//        model = await _customerService.GetCustomerForUpdateAsync(id);
//        ImgUrl = model?.Photo;
//        if (model is not null)
//        {
//            var occ = Occupations!.Where(x => x.OccupationTitle == model.Occupation).FirstOrDefault();
//            SelectCustomerOccupation(occ!.OccupationID);

//            if (model!.ProvinceRefId!.HasValue)
//            {
//                SelectedProvince = ProvinceList?.FirstOrDefault(x => x!.ProvinceID == model.ProvinceRefId);
//                this.DistrictList = await _addressService.GetDistrictByProvinceAsync(model?.ProvinceRefId);
//                SelectedDistrict = DistrictList.FirstOrDefault(x => x!.DistrictId == model!.DistrictRefId);
//            }
//            if (model!.DistrictRefId.HasValue)
//            {
//                this.CommuneList = await _addressService.GetCommuneByDistricteAsync(model?.DistrictRefId);
//                SelectedCommune = CommuneList?.FirstOrDefault(x => x!.CommuneId == model!.CommuneRefId);
//            }
//            if (model!.CommuneRefId.HasValue)
//            {
//                this.VillageList = await _addressService.GetVillageByCommuneAsync(model?.CommuneRefId);
//                SelectedVillage = VillageList?.FirstOrDefault(x => x!.VillageID == model!.VillageRefId);
//            }
//        }
//    }
//    catch (Exception ex)
//    {

//    }
//    await base.OnInitializedAsync();
//}
//void Close()
//{
//    NavigationManager.NavigateTo("/Customers");
//}
//protected async Task FormSubmit()
//{
//    model!.ProvinceRefId = SelectedProvince?.ProvinceID ?? null;
//    model.ProDescription = SelectedProvince?.ProDescription ?? null;
//    model.ProInKhmer = SelectedProvince?.ProInKhmer ?? string.Empty;
//    model.DistrictRefId = SelectedDistrict?.DistrictId ?? null;
//    model.DisDescription = SelectedDistrict?.DisDescription ?? string.Empty;
//    model.DisInKhmer = SelectedDistrict?.DisInKhmer ?? string.Empty;
//    model.CommuneRefId = SelectedCommune?.CommuneId ?? null;
//    model.ComDescription = SelectedCommune?.ComDescription ?? string.Empty;
//    model.ComInKhmer = SelectedCommune?.ComInKhmer ?? string.Empty;
//    model.VillageRefId = SelectedVillage?.VillageID;
//    model.VillDescription = SelectedVillage?.VillDescription ?? string.Empty;
//    model.VilInKhmer = SelectedVillage?.VilInKhmer ?? string.Empty;
//    editForm.EditContext?.Validate();
//    try
//    {
//        model!.EnType = "Customer";
//        var response = await _customerService.UpdateCustomerAsync(id, model);
//        if (response.IsSuccess)
//        {

//            NotificationService.Notify(Radzen.NotificationSeverity.Success, response.Data!.message);
//            NavigationManager.NavigateTo($"/Customers/Details/{model!.CustomerId}");
//        }
//        else
//        {

//            var message = response.Errors!.FirstOrDefault();

//            NotificationService.Notify(Radzen.NotificationSeverity.Error, message);
//        }

//    }
//    catch (Exception ex)
//    {

//    }
//}

//protected async void SelectedDistrictsByprovince(int? id)
//{
//    model!.ProvinceRefId = id;
//    if (model.ProvinceRefId.HasValue)
//    {
//        DistrictList = await _addressService.GetDistrictByProvinceAsync(model.ProvinceRefId);
//    }
//}
//protected async void SelectedCommunesByDistrict(int? id)
//{
//    model!.DistrictRefId = id;
//    if (model.DistrictRefId.HasValue)
//    {
//        CommuneList = await _addressService.GetCommuneByDistricteAsync(model.DistrictRefId);
//    }

//}

//protected async void SelectedVillagesByCommune(int? id)
//{
//    model!.CommuneRefId = id;
//    if (model.CommuneRefId.HasValue)
//    {
//        VillageList = await _addressService.GetVillageByCommuneAsync(model.CommuneRefId);
//    }
//}


///// <summary>
///// Coutries
///// </summary>
///// <param name="searchText"></param>
///// <returns></returns>


//private async Task GetCountries()
//{
//    var countries = await _addressService.GetCountries(105);
//    Countries = countries!.Where(x => x.CountryId == 105)!.Select(x => x.DesInKhmer).ToList();
//}
//private async Task GetCities()
//{
//    Cities = await _addressService.GetCities();
//}

///// <summary>
///// Provinces
///// </summary>








//protected async Task AddNewSalutation(KeyboardEventArgs e)
//{
//    if (e.Code == "Enter")
//    {
//        await _nameService.AddSalution(new Salutation
//        {
//            Title = newSalution
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
//    Salutions = salutations.IsSuccess
//                         ? salutations!.Data!.Select(x => x.Title!).ToList()
//                         : Salutions = [];
//}

//protected int? SelectedOccupationId;
//protected int? SelectedCustOccupationId;
//protected int? ConvertOccupation(StOcupationDto vil) => vil?.OccupationID;
//protected StOcupationDto? LookupOccupation(int? id) => Occupations?.FirstOrDefault(x => x.OccupationID == id);
//protected async Task<IEnumerable<StOcupationDto>?> SearchOccupations(string? searchText)
//{
//    var occupations = Occupations?.Where(x => x.OccupationTitle != null && x.OccupationTitle.StartsWith(searchText!, StringComparison.InvariantCultureIgnoreCase));

//    return await Task.FromResult(occupations);
//}
//protected void SelectedOccupationValueChanged(int? id)
//{
//    SelectedOccupationId = id;
//    if (id.HasValue)
//    {
//        var selectedOccupation = Occupations?.FirstOrDefault(x => x.OccupationID == id);
//        model!.SpouseOccupation = selectedOccupation?.OccupationTitle;
//    }
//    else
//    {
//        model!.SpouseOccupation = null;
//        SelectedOccupationId = null;
//    }
//    StateHasChanged();
//}

//protected void SelectCustomerOccupation(int? id)
//{
//    SelectedCustOccupationId = id;
//    if (id.HasValue)
//    {
//        var selectedOccupation = Occupations?.FirstOrDefault(x => x.OccupationID == id);
//        model!.Occupation = selectedOccupation?.OccupationTitle;
//    }
//    else
//    {
//        model!.Occupation = null;
//        SelectedCustOccupationId = null;
//    }
//    StateHasChanged();
//}
//private ElementReference _input;
//[Inject]
//public IFileReaderService? FileReaderService { get; set; }
//[Inject]
//public HttpClient? http { get; set; }
//string? ImgUrl;
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
//                    model.Photo = ImgUrl ?? null;

//                }
//                //ImgUrl = await http.UploadProductImage(content);
//                await InvokeAsync(StateHasChanged);
//            }
//        }
//    }
//}
//private 

