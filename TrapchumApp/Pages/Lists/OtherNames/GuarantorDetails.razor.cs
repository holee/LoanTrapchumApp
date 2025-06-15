using Applications.Responses.ViewModels.Loans;
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

        List<VGuarantorLoanRequest> LoanRequest = [];

        List<VGuarantorLoan> Loans = []; 
        protected override async Task OnInitializedAsync()
        {
            await GetGuarantorByIdAsync();

            await base.OnInitializedAsync();
        }

        private async Task GetGuarantorByIdAsync() 
        {

            Console.WriteLine(Id);

            var rsp = await _gService.GetGuarantorWithByIdAsync(Id);

            if (rsp.IsSuccess)
            {
                Guarantor = rsp.Data!;

                LoanRequest = rsp.Data!.LoanRequests.ToList();

                Loans = rsp.Data.Loans.ToList();
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