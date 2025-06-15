using Applications.Requests.Lists.Addreses;
using Applications.Requests.Lists.OtherNames;
using Applications.Services.Lists;
using Domains.Models.Bases;
using Radzen;

namespace TrapchumApp.Pages.Lists.OtherNames;
public partial class GuarantorAddModal
{
    private readonly IGuarantorServiceImpl _gService;
    public GuarantorAddModal(IGuarantorServiceImpl guarantorService)
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
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }

        await base.OnInitializedAsync();
    }
    protected async Task OnSaveChanges()
    {
        model.EntType = EntType.OtherName.ToString();

        var rsp = await _gService.CreateAsync(model);

        if (rsp.IsSuccess)
        {
            NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
            model = new();
            DialogService.Close(true);
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
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
