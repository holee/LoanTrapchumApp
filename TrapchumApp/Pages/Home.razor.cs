using Applications.Features.Dashbaord.Accountants.Dtos;
using Applications.Features.Dashbaord.CreditOfficers.Dtos;
using Applications.Features.Dashbaord.Managers.Dto;
using BlazorExpress.ChartJS;
using TrapchumApp.Helpers.Enums;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages
{
    public partial class Home
    {
        private bool _isPageLoading = true;
        [CascadingParameter] Task<AuthenticationState> AuthState { get; set; } = default!;
        [CascadingParameter] CascadingAppState bState { get; set; } = default!;

        [PersistentState]
        public AccountSummary? AccSummaries { get; set; }

        [PersistentState]
        public CoLoanSummary? CoSummaries { get; set; }

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



        private BarChart barChart = default!;
        private BarChartOptions barChartOptions = default!;
        private ChartData chartData = default!;



        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthState;

            await bState.GetAuthenticatedUser(auth.User);

            UserId = auth.GetUserId();

            await ShowDashboard();


            var labels = new List<string> { "Chrome", "Firefox", "Safari", "Edge" };

            var datasets = new List<IChartDataset>();

            var dataset1 = new BarChartDataset()
            {
                Data = new List<double?> { 55000, 15000, 18000, 21000 },

                BackgroundColor = new List<string> { ColorUtility.CategoricalTwelveColors[1] },

                BorderColor = new List<string> { ColorUtility.CategoricalTwelveColors[0] },

                BorderWidth = new List<double> { 0 },
            };
            datasets.Add(dataset1);

            chartData = new ChartData { Labels = labels, Datasets = datasets };

            barChartOptions = new BarChartOptions();

            barChartOptions.Responsive = true;

            barChartOptions.Interaction = new Interaction { Mode = InteractionMode.X };

            barChartOptions.IndexAxis = "x";

            barChartOptions.Scales.X!.Title = new ChartAxesTitle { Text = "Visitors", Display = true };

            barChartOptions.Scales.Y!.Title = new ChartAxesTitle { Text = "Browser", Display = true };

            barChartOptions.Plugins.Legend.Display = false;


            await base.OnInitializedAsync();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await barChart.InitializeAsync(chartData, barChartOptions);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        void OnParentClicked(object? args)
        {

        }

        private async Task ShowDashboard()
        {

            if (bState.IsAccount)
            {
                if (AccSummaries is null)
                {
                    await dashboardServices.GetAccDashboard();

                    AccSummaries = dashboardServices.AccSummaries;
                }

            }

            if (bState.IsCreditAgent)
            {
                if (CoSummaries is null)
                {
                    await dashboardServices.GetCoDashboard();

                    CoSummaries = dashboardServices.CoSummaries;
                }

            }


            if (bState.IsManager)
            {
                if (ManagerSummaries is null)
                {
                    await dashboardServices.GetManagerDashboard();

                    ManagerSummaries = dashboardServices.ManagerSummaries;
                }
            }

            _isPageLoading = false;

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

        private ColorEnum GetGradientColor(string? state)
        {
            return state switch
            {
                nameof(RequestStatus.New) => ColorEnum.Primary,
                nameof(RequestStatus.Approved) => ColorEnum.Success,
                nameof(RequestStatus.Rejected) => ColorEnum.Warning,
                nameof(RequestStatus.Denied) => ColorEnum.Danger,
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

        private string[] ChartLabels => revenue1.Select(d => d.Quarter ?? "").ToArray();
        private double[] Chart1Values => revenue1.Select(d => d.Revenue).ToArray();
        private double[] Chart2Values => revenue2.Select(d => d.Revenue).ToArray();


    }
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

