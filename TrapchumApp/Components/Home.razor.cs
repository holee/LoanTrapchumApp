using Microsoft.JSInterop;

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
            //using var playWright = await Playwright.UpsertAsync();

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
            ///

            //Convert.FromBase64String()

            ////url = $"data:application/pdf;base64,{base64}";

            await js.InvokeVoidAsync("open", $"/Reports/Schedules", "_blank");


            //NavigationManager.NavigateTo("", true);



        }
    }


}