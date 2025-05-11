using Applications.Interfaces.Lists;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.OtherNames;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Threading.Tasks;

namespace TrapchumApp.Components.Pages.Lists.OtherNames
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

        IEnumerable<VLoanGuarantors> LoanGuarantors = [];
        protected override async Task OnInitializedAsync()
        {
            await GetGuarantorByIdAsync();

            await base.OnInitializedAsync();
        }

        private async Task GetGuarantorByIdAsync() 
        {

            Console.WriteLine(Id);

            var resultResponse = await _gService.GetGuarantorByIdAsync(Id, true,true);

            if (resultResponse.IsSuccess)
            {
                Guarantor = resultResponse.Data!;

                LoanGuarantors = resultResponse.Data!.LoanGuarantors;
            }
            else
            {

                NotificationService.Notify(NotificationSeverity.Error, resultResponse.ErrMessage);
            }

        }


        async Task CloseGuarantorTab()
        {
            await Closing.InvokeAsync(3);
        }
    }
}