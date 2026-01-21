using Applications.Features.Guarantor.Command.CreateGuarantor;
using Applications.Features.Guarantor.Dtos;
using Applications.Services.Lists;
using Domains.Models.Bases;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Lists.OtherNames;

public partial class GuarantorAddModal
{
    private EntityGuarantorCreate model { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        model.Gender = "mr";

        await base.OnInitializedAsync();
    }
    protected async Task OnSaveChanges()
    {
        model.EntType = EntType.OtherName.ToString();

        var command = new CreateGuarantorCommand(model);

        var rsp = await mediator.Send(command);

        if (rsp.IsSuccess())
        {
            NotificationService.Notify(NotificationSeverity.Success, "New Guarantor was created successfully.");

            model = new();

            DialogService.Close(rsp.Value);
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
        }
    }

    private void Cancel()
    {
        DialogService.Close(null);
    }


}
