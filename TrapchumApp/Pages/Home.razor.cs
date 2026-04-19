using Applications.Features.Branches.Queries.GetBranchByUser;
using Applications.Features.Dashbaord.Accountants.Dtos;
using Applications.Features.Dashbaord.Accountants.Queries.GetDbAccountLoanSummaery;
using Applications.Features.Dashbaord.Accountants.Queries.GetDbApprovedLoanRequest;
using Applications.Features.Dashbaord.CreditOfficers.Dtos;
using Applications.Features.Dashbaord.CreditOfficers.Queries.GetDbCreditOfficeLoanSummary;
using Applications.Features.Dashbaord.Managers.Dto;
using Applications.Features.Dashbaord.Managers.Queries.GetDbManagerLoanSummary;
using Applications.Interfaces.Lists;
using Domains.Models.Lists;
using TrapchumApp.Components.Datatabes;
using TrapchumApp.Helpers.Enums;
using TrapchumApp.Helpers.Extensions;
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

        void OnParentClicked(MenuItemEventArgs args)
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

        // ── BlabSelect2 Demo ────────────────────────────────────────
        private int _selectedItemId;
        private IEnumerable<int>? _selectedItemIds = [];
        private int _templateItemId;
        private int _asyncNationalityId;

        private readonly List<DemoItem> _demoItems =
        [
            new(1, "Cambodia",      "🇰🇭"),
            new(2, "United States", "🇺🇸"),
            new(3, "Japan",         "🇯🇵"),
            new(4, "France",        "🇫🇷"),
            new(5, "Thailand",      "🇹🇭"),
            new(6, "Vietnam",       "🇻🇳"),
            new(7, "South Korea",   "🇰🇷"),
            new(8, "Germany",       "🇩🇪"),
            new(9, "Australia",     "🇦🇺"),
            new(10, "Canada",       "🇨🇦"),
        ];

        //private async Task<IEnumerable<Nationality>> SearchNationalitiesAsync(
        //    string query, CancellationToken ct)
        //{
        //    return await NationalityService.GetNationalities(query);
        //}

        private sealed record DemoItem(int Id, string Name, string Flag = "");

        // ── BlabTable Demo ──────────────────────────────────────────
        private IEnumerable<EmployeeDemo>? _tableData;
        private int _tableTotalCount;
        private IEnumerable<EmployeeDemo>? _tableData2;
        private int _tableTotalCount2;
        private string? _selectedEmployee;
        private string? _lastEditAction;
        private string? _lastCellEdit;

        private static readonly List<EmployeeDemo> _allEmployees =
        [
            new() { Id = 1,  FullName = "Sok Dara",       Position = "Developer",      Department = "IT",        Salary = 1200, JoinDate = new(2021, 3, 15) },
            new() { Id = 2,  FullName = "Chan Sophea",    Position = "Designer",       Department = "Marketing", Salary = 1050, JoinDate = new(2020, 7, 1) },
            new() { Id = 3,  FullName = "Nhem Veasna",    Position = "Manager",        Department = "HR",        Salary = 1800, JoinDate = new(2018, 1, 10) },
            new() { Id = 4,  FullName = "Ly Meng",        Position = "Accountant",     Department = "Finance",   Salary = 1400, JoinDate = new(2019, 5, 22) },
            new() { Id = 5,  FullName = "Chea Kunthea",   Position = "Tester",         Department = "IT",        Salary = 1100, JoinDate = new(2022, 9, 3) },
            new() { Id = 6,  FullName = "Phan Rith",      Position = "Credit Officer", Department = "Loan",      Salary = 1350, JoinDate = new(2020, 11, 18) },
            new() { Id = 7,  FullName = "Sorn Pisey",     Position = "Developer",      Department = "IT",        Salary = 1250, JoinDate = new(2023, 2, 5) },
            new() { Id = 8,  FullName = "Keo Makara",     Position = "Support",        Department = "Operation", Salary = 950,  JoinDate = new(2021, 6, 30) },
            new() { Id = 9,  FullName = "Heng Socheata",  Position = "Branch Manager", Department = "Admin",     Salary = 2200, JoinDate = new(2017, 4, 12) },
            new() { Id = 10, FullName = "Tep Sreymom",    Position = "Teller",         Department = "Finance",   Salary = 1000, JoinDate = new(2022, 12, 1) },
            new() { Id = 11, FullName = "Vin Chanthol",   Position = "Developer",      Department = "IT",        Salary = 1300, JoinDate = new(2020, 8, 14) },
            new() { Id = 12, FullName = "Ream Bopha",     Position = "HR Officer",     Department = "HR",        Salary = 1150, JoinDate = new(2019, 10, 20) },
            new() { Id = 13, FullName = "Sam Vibol",      Position = "Loan Officer",   Department = "Loan",      Salary = 1450, JoinDate = new(2021, 1, 8) },
            new() { Id = 14, FullName = "Meas Sothea",    Position = "Analyst",        Department = "Finance",   Salary = 1550, JoinDate = new(2018, 11, 25) },
            new() { Id = 15, FullName = "Kong Chanra",    Position = "Designer",       Department = "Marketing", Salary = 1100, JoinDate = new(2023, 5, 17) },
            new() { Id = 16, FullName = "Noun Pheakdey",  Position = "Team Lead",      Department = "IT",        Salary = 1700, JoinDate = new(2017, 9, 2) },
            new() { Id = 17, FullName = "Yim Raksmey",    Position = "Intern",         Department = "IT",        Salary = 600,  JoinDate = new(2024, 1, 15) },
            new() { Id = 18, FullName = "Ros Chenda",     Position = "Secretary",      Department = "Admin",     Salary = 900,  JoinDate = new(2022, 4, 10) },
        ];

        private static IEnumerable<EmployeeDemo> ApplyQuery(IEnumerable<EmployeeDemo> source, BlabTableLoadDataArgs args)
        {
            var query = source;

            if (!string.IsNullOrWhiteSpace(args.SearchText))
            {
                query = query.Where(e =>
                    e.FullName.Contains(args.SearchText, StringComparison.OrdinalIgnoreCase) ||
                    e.Position.Contains(args.SearchText, StringComparison.OrdinalIgnoreCase) ||
                    e.Department.Contains(args.SearchText, StringComparison.OrdinalIgnoreCase));
            }

            query = args.SortColumn switch
            {
                "Id"         => args.SortDescending ? query.OrderByDescending(e => e.Id)         : query.OrderBy(e => e.Id),
                "FullName"   => args.SortDescending ? query.OrderByDescending(e => e.FullName)   : query.OrderBy(e => e.FullName),
                "Position"   => args.SortDescending ? query.OrderByDescending(e => e.Position)   : query.OrderBy(e => e.Position),
                "Department" => args.SortDescending ? query.OrderByDescending(e => e.Department) : query.OrderBy(e => e.Department),
                "Salary"     => args.SortDescending ? query.OrderByDescending(e => e.Salary)     : query.OrderBy(e => e.Salary),
                "JoinDate"   => args.SortDescending ? query.OrderByDescending(e => e.JoinDate)   : query.OrderBy(e => e.JoinDate),
                _            => query.OrderBy(e => e.Id)
            };

            return query;
        }

        private Task OnTableLoadData(BlabTableLoadDataArgs args)
        {
            var query = ApplyQuery(_allEmployees, args);
            _tableTotalCount = query.Count();
            _tableData = query.Skip(args.Skip).Take(args.Top).ToList();
            return Task.CompletedTask;
        }

        private Task OnTableLoadData2(BlabTableLoadDataArgs args)
        {
            var query = ApplyQuery(_allEmployees, args);
            _tableTotalCount2 = query.Count();
            _tableData2 = query.Skip(args.Skip).Take(args.Top).ToList();
            return Task.CompletedTask;
        }

        private void OnTableRowClick(EmployeeDemo employee)
        {
            _selectedEmployee = $"{employee.FullName} ({employee.Position})";
        }

        private Task OnRowEditSave(EmployeeDemo employee)
        {
            _lastEditAction = $"Saved: {employee.FullName} – {employee.Position} – {employee.Department} – {employee.Salary:N2}$";
            return Task.CompletedTask;
        }

        private Task OnRowEditCancel(EmployeeDemo employee)
        {
            _lastEditAction = $"Cancelled edit for: {employee.FullName}";
            return Task.CompletedTask;
        }

        private Task OnCellEditSave(BlabTableCellEditArgs<EmployeeDemo> args)
        {
            _lastCellEdit = $"Saved cell [{args.Property}] for {args.Item.FullName}";
            return Task.CompletedTask;
        }

        private sealed class EmployeeDemo
        {
            public int Id { get; set; }
            public string FullName { get; set; } = string.Empty;
            public string Position { get; set; } = string.Empty;
            public string Department { get; set; } = string.Empty;
            public decimal Salary { get; set; }
            public DateOnly JoinDate { get; set; }
        }
    }
}