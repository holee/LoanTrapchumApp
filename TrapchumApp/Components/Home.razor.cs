using Microsoft.JSInterop;
using TrapchumApp.Components.Datatabes;
using TrapchumApp.Components.Messageboxes;
using TrapchumApp.Components.Modals.SweetAlerts;

namespace TrapchumApp.Components
{
    public partial class Home
    {
        //private readonly IDocument _schedule;

        //public Home(IDocument schedule)
        //{
        //    _schedule = schedule;
        //}

        string url = string.Empty;

        async Task Print()
        {
            //ILoggerFactory loggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();

            //using var htmlRender = new HtmlRenderer(ServiceProvider, loggerFactory);

            //var html = await htmlRender.Dispatcher.InvokeAsync(async () =>
            //{
            //    var output = await htmlRender.RenderComponentAsync<TestRazor>();
            //    return output.ToHtmlString();
            //});

            //// Ensure Playwright is properly referenced
          
            //using var playWright = await Playwright.CreateAsync();

            ////await Playwright.();

            //var browser = await playWright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            //{
            //    Headless = true
            //});

            //var page = await browser.NewPageAsync();

            //await page.SetContentAsync(html);

            ////await page.EmulateMediaAsync(new PageEmulateMediaOptions { Media = Media.Print });

            //var pp = await page.PdfAsync(new PagePdfOptions
            //{
            //    Format = "A4",
            //    Path = Path.Combine(Env.WebRootPath, "schedules/test.pdf"),
            //});

            //await browser.CloseAsync();

            ////var stream = new MemoryStream();


            ////_schedule.GeneratePdf(stream);

            ////stream.Position = 0;

            ////var bytes = stream.ToArray();

            //var xxx = Path.Combine(Env.WebRootPath, "schedules/test.pdf");

            //var base64 = Convert.ToBase64String(pp);
            

            ////Convert.FromBase64String()

            //url = $"data:application/pdf;base64,{base64}";

            //await js.InvokeVoidAsync("open", $"/Reports/Schedules", "_blank");


            ////NavigationManager.NavigateTo("", true);



        }


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
                "Id" => args.SortDescending ? query.OrderByDescending(e => e.Id) : query.OrderBy(e => e.Id),
                "FullName" => args.SortDescending ? query.OrderByDescending(e => e.FullName) : query.OrderBy(e => e.FullName),
                "Position" => args.SortDescending ? query.OrderByDescending(e => e.Position) : query.OrderBy(e => e.Position),
                "Department" => args.SortDescending ? query.OrderByDescending(e => e.Department) : query.OrderBy(e => e.Department),
                "Salary" => args.SortDescending ? query.OrderByDescending(e => e.Salary) : query.OrderBy(e => e.Salary),
                "JoinDate" => args.SortDescending ? query.OrderByDescending(e => e.JoinDate) : query.OrderBy(e => e.JoinDate),
                _ => query.OrderBy(e => e.Id)
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

        // ── SweetAlert Demo ───────────────────────────────────────
        private string _swalLastResult = "—";

        private async Task SwalDemo_Success()
        {
            var r = await Swal.SuccessAsync("Saved!", "Your record has been saved successfully.");
            _swalLastResult = r.ToString();
        }

        private async Task SwalDemo_Error()
        {
            var r = await Swal.ErrorAsync("Oops!", "Something went wrong. Please try again.");
            _swalLastResult = r.ToString();
        }

        private async Task SwalDemo_Warning()
        {
            var r = await Swal.WarningAsync("Warning", "You have unsaved changes.");
            _swalLastResult = r.ToString();
        }

        private async Task SwalDemo_Info()
        {
            var r = await Swal.InfoAsync("Did you know?", "SweetAlert2 is fully customisable.");
            _swalLastResult = r.ToString();
        }

        private async Task SwalDemo_Question()
        {
            var r = await Swal.QuestionAsync("Are you sure?", "This action cannot be undone.");
            _swalLastResult = r.ToString();
        }

        private async Task SwalDemo_Confirm()
        {
            var confirmed = await Swal.ConfirmAsync("Delete record?", "This will permanently remove the item.");
            _swalLastResult = confirmed ? "Confirmed" : "Cancelled";
        }

        private async Task SwalDemo_ConfirmDeny()
        {
            var r = await Swal.ConfirmDenyAsync("Save changes?", "Choose an action below.", SwalIcon.Question, "Save", "Discard", "Cancel");
            _swalLastResult = r.ToString();
        }

        private async Task SwalDemo_Input()
        {
            var value = await Swal.InputAsync("Enter your name", "We'll use this to greet you.", placeholder: "Full name", required: true);
            _swalLastResult = value is null ? "Cancelled" : $"Input: {value}";
        }

        private async Task SwalDemo_Html()
        {
            var r = await Swal.FireAsync(new SwalOptions
            {
                Title = "HTML <u>Content</u>",
                Html  = "You can use <strong>bold</strong>, <em>italic</em>, and even <a href='#'>links</a>.",
                Icon  = SwalIcon.Info
            });
            _swalLastResult = r.ToString();
        }

        private async Task SwalDemo_Timed()
        {
            var r = await Swal.TimedAsync("Auto-closing!", "This dialog closes in 3 seconds.", timerMs: 3000);
            _swalLastResult = r.ToString();
        }

        private async Task SwalDemo_CustomFooter()
        {
            var r = await Swal.FireAsync(new SwalOptions
            {
                Title             = "Custom Dialog",
                Text              = "This dialog has a footer and a wider width.",
                Icon              = SwalIcon.Success,
                Width             = "42em",
                Footer            = "<a href='#'>Why do I have this issue?</a>",
                ShowCloseButton   = true,
                AllowOutsideClick = true
            });
            _swalLastResult = r.ToString();
        }

        // ── MessageBox Demo ────────────────────────────────────────
        private string _msgBoxLastResult = "—";

        private async Task MsgDemo_Info()
        {
            var r = await MsgBox.InfoAsync("The operation completed successfully.");
            _msgBoxLastResult = r.ToString();
        }

        private async Task MsgDemo_Success()
        {
            var r = await MsgBox.SuccessAsync("Record has been saved to the database.");
            _msgBoxLastResult = r.ToString();
        }

        private async Task MsgDemo_Warning()
        {
            var r = await MsgBox.WarningAsync("You have unsaved changes. Please save before leaving.");
            _msgBoxLastResult = r.ToString();
        }

        private async Task MsgDemo_Error()
        {
            var r = await MsgBox.ErrorAsync("Unable to connect to the server. Please try again later.");
            _msgBoxLastResult = r.ToString();
        }

        private async Task MsgDemo_YesNo()
        {
            var r = await MsgBox.ConfirmAsync("Are you sure you want to delete this record?");
            _msgBoxLastResult = r.ToString();
        }

        private async Task MsgDemo_YesNoCancel()
        {
            var r = await MsgBox.ConfirmCancelAsync("Do you want to save changes before closing?");
            _msgBoxLastResult = r.ToString();
        }

        private async Task MsgDemo_OKCancel()
        {
            var r = await MsgBox.ShowAsync(
                "This action will reset all settings to default.",
                "Reset Settings",
                MsgBoxButtons.OKCancel,
                MsgBoxBoxIcon.Warning);
            _msgBoxLastResult = r.ToString();
        }

        private async Task MsgDemo_RetryCancel()
        {
            var r = await MsgBox.ShowAsync(
                "The file could not be uploaded. Would you like to retry?",
                "Upload Failed",
                MsgBoxButtons.RetryCancel,
                MsgBoxBoxIcon.Error);
            _msgBoxLastResult = r.ToString();
        }

        private async Task MsgDemo_AbortRetryIgnore()
        {
            var r = await MsgBox.ShowAsync(
                "A critical error occurred during data processing.",
                "Critical Error",
                MsgBoxButtons.AbortRetryIgnore,
                MsgBoxBoxIcon.Error
                );
            _msgBoxLastResult = r.ToString();
        }

    }


}