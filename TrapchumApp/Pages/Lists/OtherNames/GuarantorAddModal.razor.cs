using Applications.Requests.Lists.OtherNames;
using Applications.Services.Lists;
using Domains.Models.Bases;

namespace TrapchumApp.Pages.Lists.OtherNames;
public partial class GuarantorAddModal
{
    private readonly IGuarantorServiceImpl _gService;
    public GuarantorAddModal(IGuarantorServiceImpl guarantorService)
    {
        _gService = guarantorService;
    }

    private EntityGuarantorCreate model { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        model.Gender = "mr";
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

    private void Cancel()
    {
        DialogService.Close(false);
    }


}
