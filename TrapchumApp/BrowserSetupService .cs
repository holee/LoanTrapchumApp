using PuppeteerSharp;

namespace TrapchumApp
{
    public sealed class BrowserSetupService :BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var browserFetcher = new BrowserFetcher();

            await browserFetcher.DownloadAsync();
        }
    }
}
