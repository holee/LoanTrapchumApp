using Microsoft.Playwright;

namespace TrapchumApp.Pages.Reports.Services
{
    public enum PageOption
    {
        A3,
        A4,
        A5
    }
    public sealed class ScheduleReportService
    {
        public class PdfGeneratorService
        {
            public async Task<byte[]> GeneratePdfFromHtmlAsync(string htmlContent)
            {
                using var playwright = await Playwright.CreateAsync();

                await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = true
                });

                var page = await browser.NewPageAsync();

                await page.SetContentAsync(htmlContent, new PageSetContentOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                var pdfBytes = await page.PdfAsync(new PagePdfOptions
                {
                    Format = PageOption.A4.ToString(),

                    PrintBackground = true
                });

                return pdfBytes;
            }
        }

    }
}
