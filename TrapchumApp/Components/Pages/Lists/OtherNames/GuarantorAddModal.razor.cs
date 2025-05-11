using Applications.Interfaces.Lists;
using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.OtherNames;
using Mapster;

namespace TrapchumApp.Components.Pages.Lists.OtherNames;
public partial class GuarantorAddModal
{
    private readonly IGuarantorService _gService;
    public GuarantorAddModal(IGuarantorService guarantorService)
    {
        _gService = guarantorService;
    }

    protected IEnumerable<ProvinceDto>? provinceList = [];
    protected IEnumerable<CommuneDto>? communeList = [];
    protected IEnumerable<DistrictDto>? districtList = [];
    protected IEnumerable<VillageDto>? villageList = [];
    private EntityNamesCreate model = new EntityNamesCreate();
    private bool isValid = true;
    protected override async Task OnInitializedAsync()
    {
        try
        {
            provinceList = await _addressService!.GetsProvinces();

            model.Gender = "mr";
        }
        catch (Exception ex)
        {
            NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.Message);
        }

        await base.OnInitializedAsync();
    }
    protected async Task OnSaveChanges()
    {
        model.EntType = "Other Name";

        var req = model.Adapt<EntityNames>();


        var resultResponse = await _gService.CreateGaurantorAsync(req);

        if (resultResponse.IsSuccess)
        {
            NotificationService.Notify(Radzen.NotificationSeverity.Success, resultResponse.Data);
            DialogService.Close(true);
        }
        else
        {
            NotificationService.Notify(Radzen.NotificationSeverity.Error, resultResponse.ErrMessage);
        }
    }


    protected async void OnSelectGetDistrictByProvince()
    {
        districtList = await _addressService!.GetDistrictsByProvinceId(model.ProvinceRefId);
    }
    protected async void OnSelectGeCommunesByDistrict()
    {
        communeList = await _addressService!.GetCommunesByDistrictId(model.DistrictRefId);
    }
    private void Cancel()
    {
        DialogService.Close(false);
    }
    protected async void OnSelectVillagesByCommune()
    {
        villageList = await _addressService!.GetVillagesByCommuneId(model.CommuneRefId);
    }


}
