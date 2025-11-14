using Applications.Features.Loans.Responses;
using Applications.Responses.ViewModels.Loans.Disbursements;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Lists.OtherNames
{
    public partial class GuarantorDetails
    {
        private readonly IGuarantorServiceImpl _gService;

        public GuarantorDetails(IGuarantorServiceImpl guarantorService)
        {
            _gService = guarantorService;
        }

        [Parameter] public string? Id { get; set; }
        [Parameter] public EventCallback Refresh { get; set; }
        [Parameter] public EventCallback<int> Closing { get; set; } 

        VGuarantor? Guarantor = new VGuarantor();

        List<VLoanGuarantors> LoanGuarantors = [];

        List<VGuarantorsLoanRequest> LoanRequest = [];

        List<VGuarantorLoan> Loans = []; 
        protected override async Task OnInitializedAsync()
        {
            await GetGuarantorByIdAsync();

            await base.OnInitializedAsync();
        }

        private async Task GetGuarantorByIdAsync() 
        {

            var rsp = await _gService.GetGuarantorByIdAsync(Id,default);

            if (rsp.IsSuccess && rsp.Data is not null)
            {
                Guarantor = rsp.Data;

                LoanRequest = Guarantor.LoanRequests.ToList();

                Loans = Guarantor.GuarantorLoans.ToList();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
            }

        }


        async Task CloseGuarantorTab()
        {
            await Closing.InvokeAsync(3);
        }
    }
}