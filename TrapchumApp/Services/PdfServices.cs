
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using TrapchumApp.Templates.Reports;
namespace TrapchumApp.Services;

public sealed class NullNavigationManager : NavigationManager
{
    public NullNavigationManager()
    {
        // Initialize with dummy values
        Initialize("http://localhost/", "http://localhost/");
    }

    protected override void NavigateToCore(string uri, bool forceLoad) { }
}
public sealed class PdfServices
{
    private readonly IServiceProvider _serviceProvider;

    public PdfServices(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<string> GeneratePdfFromHtmlAsync()
    {

        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();

        var services = new ServiceCollection();

        services.AddSingleton<NavigationManager, NullNavigationManager>();

        await using var htmlRenderer = new HtmlRenderer(_serviceProvider, loggerFactory);

        var html = await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var parameter = ParameterView.Empty;

            var rendered = await htmlRenderer.RenderComponentAsync<MySchedule>(parameter);

            return rendered.ToHtmlString();
        });

        return html;
    }



}

