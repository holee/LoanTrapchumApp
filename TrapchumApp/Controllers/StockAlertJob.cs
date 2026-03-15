using TrapchumApp.Hubs;

public sealed class StockAlertJob : BackgroundService
{
    private readonly IAppNotificationService _notifications;

    public StockAlertJob(IAppNotificationService notifications)
    {
        _notifications = notifications;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            //if (StockLow())
            //{
            var n = _notifications.Create("Stock alert", "Item X is below threshold", AppNotificationLevel.Warning);
           
            await _notifications.SendToAllAsync(n);
            //}
            await Task.Delay(TimeSpan.FromMinutes(1), ct);
        }
    }
}