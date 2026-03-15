using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrapchumApp.Hubs;

namespace TrapchumApp.Controllers;

[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
[ApiController]
public class NotifcationsController : ControllerBase
{
    private readonly IAppNotificationService _notifications;

    public NotifcationsController(IAppNotificationService notificationService)
    {
        _notifications = notificationService;
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder()
    {
        var orderId = new Random().Next(1000, 9999); // Simulate order ID generation

        var n = _notifications.Create(
            "Order confirmed",
            $"Order #{orderId} has been placed successfully.", AppNotificationLevel.Success
        );

        await _notifications.SendToUserAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!, n);

        return Ok();
    }

}
