using Applications.Features.Branches.Queries.GetBranchByUser;
using Applications.Features.Dashbaord.Accountants.Dtos;
using Applications.Features.Dashbaord.Accountants.Queries.GetDbAccountLoanSummaery;
using Applications.Features.Dashbaord.Accountants.Queries.GetDbApprovedLoanRequest;
using Applications.Features.Dashbaord.CreditOfficers.Dtos;
using Applications.Features.Dashbaord.CreditOfficers.Queries.GetDbCreditOfficeLoanSummary;
using Applications.Features.Dashbaord.Managers.Dto;
using Applications.Features.Dashbaord.Managers.Queries.GetDbManagerLoanSummary;
using Applications.Interfaces.Lists;
using TrapchumApp.Helpers.Enums;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages
{
    public partial class Home
    {

        [Inject] private INationalityService NationalityService { get; set; } = default!;

        [CascadingParameter] Task<AuthenticationState> AuthState { get; set; } = default!;
        [CascadingParameter] CascadingAppState bState { get; set; } = default!;

        [PersistentState]
        public IEnumerable<AccountantLoanSummary>? AccSummaries { get; set; }

        [PersistentState]
        public IEnumerable<DbCoLoanSummary>? CoSummaries { get; set; }

        [PersistentState]
        public DbManagerLoanSummary? ManagerSummaries { get; set; }

        [PersistentState]
        public string? UserId { get; set; }

        private int _numberOfLoanRequest = 0;

        private string _selectedPeriod = "month";
        private readonly List<PeriodItem> _periods =
        [
            new("today",    "Today"),
            new("week",     "This Week"),
            new("month",    "This Month"),
            new("quarter",  "This Quarter"),
            new("semester", "This Semester"),
            new("year",     "This Year"),
            new("custom",   "Custom"),
        ];

        private record PeriodItem(string Key, string Label);
        protected override async Task OnInitializedAsync()
        {

            var auth = await AuthState;

            if (auth.User is not null && string.IsNullOrEmpty(UserId))
            {
                await bState.GetAuthenticatedUser(auth.User);

                UserId = auth.GetUserId();

                await ShowDashboard();

            }

            await base.OnInitializedAsync();
        }

        void OnParentClicked(object? args)
        {

        }

        private async Task ShowDashboard()
        {

            var command1 = new GetBranchByUserQuery(UserId);

            var rsp1 = await mediator.Send(command1);

            if(rsp1.IsError)
            {
                return;
            }

            var branchId = rsp1.Value.BranchId;

            if (bState.IsAccount && branchId.HasValue && branchId > 0)
            {
                var command2 = new GetDbAccountLoanSummaryQuery(branchId, null);

                var rsp2 = await mediator.Send(command2);

                AccSummaries = rsp2.Items;

                var command3 = new GetDbApprovedRequestQuery(branchId);

                var rsp3 = await mediator.Send(command3);

                _numberOfLoanRequest = rsp3.Value;

            }

            if (bState.IsCreditAgent && branchId.HasValue && branchId > 0)
            {
                var command3 = new GetDbCreditOfficeLoanSummaryQuery(branchId, UserId, null);

                var rsp3 = await mediator.Send(command3);

                CoSummaries ??= rsp3.Items;

            }


            if (bState.IsManager && branchId.HasValue && branchId > 0)
            {
                var command4 = new GetDbManagerLoanSummaryQuery(branchId, UserId);

                var rsp4 = await mediator.Send(command4);

                ManagerSummaries ??= rsp4.Value; 
            }



        }



        private ColorEnum GetGradientColor(int? currencyId)
        {
            return currencyId switch
            {
                1 => ColorEnum.Primary,
                2 => ColorEnum.Success,
                3 => ColorEnum.Warning,
                4 => ColorEnum.Info,
                _ => ColorEnum.Secondary
            };
        }

        private string? GetTitle(int? currencyId)
        {
            return currencyId switch
            {
                1 => "Loan Due In Khmer",
                2 => "Loan Due In USD",
                3 => "Loan Due In THB",
                4 => "Loan Due In VND",
                _ => "Loan Due In Other"
            };
        }

        private string? GetTitlePriciple(int? currencyId)
        {
            return currencyId switch
            {
                1 => "Total Principle Khmer",
                2 => "Total Principle USD",
                3 => "Total Principle THB",
                4 => "Total Principle VND",
                _ => "Total Principle Other"
            };
        }

        private string? GetTitleInterest(int? currencyId)
        {
            return currencyId switch
            {
                1 => "Total Interest Khmer",
                2 => "Total Interest USD",
                3 => "Total Interest THB",
                4 => "Total Interest VND",
                _ => "Total Interest Other"
            };
        }



        // [Inject]
        // private IServiceProvider ServiceProvider { get; set; }
        // bool multiple = true;



        // private async Task PrintPage()
        // {
        //     await js.InvokeVoidAsync("printPage");

        //     var logFactory = ServiceProvider.GetRequiredService<ILoggerFactory>


        //     var htmlRender = new HtmlRenderer(ServiceProvider, logFactory);

        //     var htmlString =await htmlRender.Dispatcher.InvokeAsync(async () =>
        //     {

        //         var parameters = new Dictionary<string, object?>
        //         {
        //             {"Name","Mon Minh"}
        //         };


        //         var paraView = ParameterView.FromDictionary(parameters);

        //         var output =await htmlRender.RenderComponentAsync<TestRazor>


        //         return output.ToHtmlString();

        //     });

        //     Console.WriteLine(htmlString);
        // }


        private void HandleMoreInfo()
        {
            // Handle more info click
        }

        private class DataItem
        {
            public string? Quarter { get; set; }
            public double Revenue { get; set; }
            public int Size { get; set; }
        }

        DataItem[] revenue1 = new DataItem[]
        {
            new DataItem { Quarter = "Q1", Revenue = 234000,Size=4 },
            new DataItem { Quarter = "Q2", Revenue = 284000,Size=5 },
            new DataItem { Quarter = "Q3", Revenue = 274000,Size=10 },
            new DataItem { Quarter = "Q4", Revenue = 294000,Size=1 }
        };

        DataItem[] revenue2 = new DataItem[]
        {
            new DataItem { Quarter = "Q1", Revenue = 324000 },
            new DataItem { Quarter = "Q2", Revenue = 224000 },
            new DataItem { Quarter = "Q3", Revenue = 444000 },
            new DataItem { Quarter = "Q4", Revenue = 564000 }
        };

       
    }
}