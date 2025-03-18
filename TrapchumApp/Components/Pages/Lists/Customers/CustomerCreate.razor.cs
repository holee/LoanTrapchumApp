using AppilcationServices.Interfaces;
using ApplicationServices.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Radzen.Blazor;
using TCLMS.Shared.Entities.Dtos.Lists;
using TCLMS.Shared.Entities.Dtos.Lists.Addreses;
using TCLMS.Shared.Entities.Dtos.Lists.Customers;
using TCLMS.Shared.Entities.Dtos.Lists.Entities;
using TCLMS.Shared.Entities.Models.Addresses;
using TCLMS.Shared.Entities.Models.Lists;

namespace TCLMS.Front.Pages.Lists.Customers
{
    public partial class CustomerCreate
    {
        [Parameter] public EventCallback Closed { get; set; } = default!;
        [Parameter] public EventCallback<int?> SelectedValueChanged { get; set; }
        IEnumerable<string> Salutations = [];
        IEnumerable<ProvinceDto> Provinces = [];
        IEnumerable<string> Cities = [];
        IEnumerable<StOcupationDto> Occupations = [];
        IEnumerable<Country> Countries = [];
        protected CustomerDtoCreate model = new();
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


//private async Task GetSalutations()
//{
//    var salutations = await _salutationService.GetSalutions();
//    if (salutations.IsSuccess)
//    {
//        Salutations = salutations!.Data!.Select(x => x.Title!).ToList();
//    }
//    else
//    {
//        Salutations = [];
//    }

//}

//protected async void GetDistrictsByprovince()
//{
//    Districts = await _addressService.GetDistrictByProvinceAsync(model.ProvinceRefId);
//    StateHasChanged();
//}
//protected async void GetCommunesByDistrict()
//{
//    Communes = await _addressService.GetCommuneByDistricteAsync(model.DistrictRefId);
//    StateHasChanged();
//}
//protected async void GetVillagesByCommune()
//{
//    Villages = await _addressService.GetVillageByCommuneAsync(model.CommuneRefId);
//    StateHasChanged();
//}