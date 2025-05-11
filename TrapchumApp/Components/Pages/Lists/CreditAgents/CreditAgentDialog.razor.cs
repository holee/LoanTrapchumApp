using Applications.Requests.Loans.CreditAgents;
using Applications.Responses.Lists.Entities;
using Applications.Responses.ViewModels.Employees;
using Applications.Services.Lists;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Components.Pages.Lists.CreditAgents
{
    public partial class CreditAgentDialog
    {
        protected IEnumerable<EntEmployeeDto> Employees { get; set; } = [];
        private LoanCreditAgentDtoCreate model = new LoanCreditAgentDtoCreate();
        private List<VEmployeeCreateCreditAgent> CreditAgents = [];
        [Parameter] public long Id { get; set; }
        [Inject] protected ICreditAgentServiceImpl CreditAgentService { get; set; } = default!;

        private bool isValid = true;

        private string? searchText = "";
        protected override async Task OnInitializedAsync()
        {
            await LoadPageList();

            await base.OnInitializedAsync();
        }
        async Task SearchCreditAgent()
        {
            await LoadPageList(searchText);
        }

        private async Task AddCreditAgent(string? id)
        {
            try
            {
                var response = await CreditAgentService.AddCreditAgent(id, model);
                if (response!.IsSuccess)
                {
                    await CloseModal();
                    await LoadPageList(searchText);
                }
                else
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, response.ErrMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        async Task LoadPageList(string? filter = null)
        {
            try
            {

                var rsp = await CreditAgentService.GetEmployeeCreateCreditAgents(filter);

                if (rsp.IsSuccess)
                {
                    CreditAgents = rsp.Data!.ToList();
                }
                else
                {
                    NotificationService.Notify(Radzen.NotificationSeverity.Error, rsp.ErrMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        async Task CloseModal()
        {
            await Task.CompletedTask;
        }


    }
}