using Applications.Extensions;
using Applications.Features.Guarantor.Command.UpdateGuarantor;
using Applications.Features.Guarantor.Dtos;
using Applications.Features.Guarantor.Queries.GetUpdateGuarantorById;
using Applications.Services.Lists;

namespace TrapchumApp.Pages.Lists.OtherNames.Guarantors
{
    public partial class GuarantorEditPage
    {
        private readonly IGuarantorServiceImpl _gService;
        public GuarantorEditPage(IGuarantorServiceImpl gService)
        {
            _gService = gService;

        }
        [Parameter] public string? NameId { get; set; }
        [Parameter] public EntityGuarantorCreate model { get; set; } = new();
        [Parameter] public IEnumerable<string> Salutations { get; set; } = [];
        [Parameter] public EventCallback ClosedTab { get; set; }

        private bool _isPageLoading = false;
        protected override async Task OnInitializedAsync()
        {

            if (model.Id.IsNullOrEmpty() || NameId.IsNotNullOrEmpty())
            {
                await GetUpdateGuarantor();
            }

            await base.OnInitializedAsync();
        }

        private async Task GetUpdateGuarantor()
        {

            try
            {
                _isPageLoading = true;

                var command = new GetUpdateGuarantorByIdQuery(NameId);

                var resp = await _gService.GetGuarantorEditAsync(NameId);

                if (resp.IsSuccess())
                {
                    model = resp.Value;
                }
                _isPageLoading = false;
            }
            catch (Exception ex)
            {
                _isPageLoading = false;

                _toastService.ShowError("", ex.Message!);
            }
            finally
            {
                _isPageLoading = false;
            }


        }

        protected async Task OnSaveChanges()
        {
            try
            {

                var command = new UpdateGuarantorCommand(model);

                var rsp = await mediator.Send(command);

                if (rsp.IsSuccess())
                {

                    _toastService.ShowSuccess("", "Guarantor was updated successfully.");

                    await TabClosing();
                }
                else
                {
                    _toastService.ShowError("", rsp.GetMessage());
                }
            }
            catch (Exception ex)
            {
                _toastService.ShowError("", ex.Message);
            }

        }
        async Task TabClosing()
        {
            await ClosedTab.InvokeAsync();
        }

    }
}

