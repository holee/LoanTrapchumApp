using Applications.Interfaces.Lists;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.OtherNames;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Lists.OtherNames
{
    public partial class GuarantorDetails
    {
        private readonly IGuarantorService _gService;

        public GuarantorDetails(IGuarantorService guarantorService)
        {
            _gService = guarantorService;
        }

        [Parameter] public Guid? Id { get; set; }
        [Parameter] public EventCallback Refresh { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
        VGuarantor? Guarantor = new VGuarantor();
        IEnumerable<VLoanGuarantors> LoanGuarantors = [];
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var resultResponse = await _gService.GetGuarantorByIdAsync(Id, false);

            if (resultResponse.IsSuccess)
            {
                Guarantor = resultResponse.Data!;
                LoanGuarantors = resultResponse.Data!.LoanGuarantors;
                return;
            }
            
            NotificationService.Notify(NotificationSeverity.Error, resultResponse.ErrMessage);
        }

        private async void Reload()
        {
            await OnInitializedAsync();

            await Refresh.InvokeAsync();

            StateHasChanged();
        }
    }
}