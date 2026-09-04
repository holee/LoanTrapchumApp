using Application.Features.Notifications.Command.CreateNotification;
using Application.Features.Notifications.Dto;
using Application.Features.Notifications.Queries.GetNotification;
using Application.Responses.Paginations;
using Application.Utils;
using Domains.Enumerations;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Transactions;

public class NotificationsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetNotifications(
        [FromQuery] string? userId = null,
        [FromQuery] int? branchId = null,
        [FromQuery] int take = 10)
    {
        var result = await mediator.Send(new GetNotificationQuery(userId, branchId, take));
        return Ok(result);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetNotificationsPaged(
        [FromQuery] string? userId = null,
        [FromQuery] int? branchId = null,
        [FromQuery] NotificationStatus state = NotificationStatus.Unread,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "CreatedAt",
        [FromQuery] bool isDescending = true,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetNotificationAsPagingQuery(userId, branchId, state, param));
        return Ok(result);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetUnreadNotificationSummary(
        [FromQuery] string? userId = null,
        [FromQuery] int? branchId = null,
        [FromQuery] int take = 10)
    {
        var result = await mediator.Send(new GetUnreadNotificationSummaryQuery(userId, branchId, take));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNotification([FromBody] NotificationDtoCreate request)
    {
        var result = await mediator.Send(new CreateNotificationCommand(request));
        return HandleResult(result);
    }

    [HttpPatch("{id:guid}/mark-unread")]
    public async Task<IActionResult> MarkUnread(Guid id, [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new MarkUnreadNotificationCommand(id, branchId));
        return HandleResult(result);
    }
}
