using Applications.Features.Customers.Services;
using Applications.Requests.Lists.Entities;
using Applications.Responses.ViewModels.Customers;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Lists.Customers
{
    public partial class CustomerModalIncome
    {

        private readonly IIncomeExpendServiceImpl _oiService;

        public CustomerModalIncome(IIncomeExpendServiceImpl customerService)
        {
            _oiService = customerService;
        }

        List<CustomerIncomeDtoCreate> CustomerIncomes = new List<CustomerIncomeDtoCreate>();
        List<VCustomerIncomes> Incomes = [];
        [Parameter] public string? CustomerId { get; set; }
        RadzenDataGrid<VCustomerIncomes> incomeGrid = default!;
        List<VCustomerIncomes> insertGrid = [];
        private void AddNewRow()
        {
            CustomerIncomes?.Add(new CustomerIncomeDtoCreate());
        }
        private void RemoveRow(CustomerIncomeDtoCreate row)
        {
            CustomerIncomes?.Remove(row);
        }

        protected override void OnInitialized()
        {
            AddNewRow();
            AddNewRow();
            AddNewRow();
            AddNewRow();
            base.OnInitialized();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetCustomerIncome(CustomerId);
        }

        private void Close()
        {
            DialogService.Close(false);
        }

        async Task GetCustomerIncome(string? customerId)
        {
            var resp = await _oiService.GetCustomerCustomerIncomes(customerId);
            if (resp.IsSuccess)
            {
                Incomes = resp.Data!.ToList();
            }
        }



        private bool isValid = true;

        async Task SaveChanged()
        {
            try
            {
                var model = Incomes.Where(x => x.IncomeAmount > 0)
                                    .Select(x => new CustomerIncomeDtoCreate(x.IncomeId, CustomerId, x.IncomeSource, x.IncomeAmount, x.InDescription))
                                    .ToList();

                if (!model.Any()) return;

                var rsp = await _oiService.AddOrUpdateIncomesAsync(CustomerId, model);

                if (rsp.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success,"Added.");
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);

                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }

        void Reset()
        {
            insertGrid.Clear();
        }

        void Reset(VCustomerIncomes income)
        {
            Incomes.Remove(income);
        }

        async void InsertedRow()
        {
            var income = new VCustomerIncomes();
            Incomes.Add(income);
            await incomeGrid.InsertRow(income);
        }

        async Task DeleteRow(VCustomerIncomes income)
        {
            Reset(income);
            if (Incomes.Contains(income))
            {
                await incomeGrid.Reload();
            }
            else
            {
                incomeGrid.CancelEditRow(income);
                await incomeGrid.Reload();
            }
        }


        void CancelEdit(VCustomerIncomes income)
        {
            Reset(income);
            incomeGrid.CancelEditRow(income);
        }

        async Task SaveRow(VCustomerIncomes income)
        {

            if (income.IncomeId <= 0)
            {

            }
            await incomeGrid.UpdateRow(income);
        }

        async Task EditRow(VCustomerIncomes income)
        {
            await incomeGrid.EditRow(income);
        }
    }
}