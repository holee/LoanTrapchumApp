using Applications.Features.Guarantor.Dtos;
using Applications.Features.Guarantor.Queries.GetGuarantorDetailById;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Responses.ViewModels.Loans.Disbursements;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Components;
using Radzen;
using TrapchumApp.Helpers.Extensions;

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
        [Parameter] public EventCallback ClosedTab { get; set; }

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

            try
            {
                Tracker.Show();

                var command = new GetGuarantorDetailByIdQuery(Id);

                var rsp = await mediator.Send(command);

                if (!rsp.IsError)
                {
                    Guarantor = rsp.Value;

                    LoanRequest = Guarantor.LoanRequests.ToList();

                    Loans = Guarantor.GuarantorLoans.ToList();
                }
                else
                {
                    _toastService.ShowError("Guarantor", rsp.GetMessage());
                }

                Tracker.Hide();
            }
            finally
            {
                Tracker.Hide();
            }

            StateHasChanged();
        }

        async Task CloseGuarantorTab()
        {
            await ClosedTab.InvokeAsync();
        }
    }
}