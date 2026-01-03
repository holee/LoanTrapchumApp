using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace TrapchumApp.Hubs;
public sealed class UserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?
            .FindFirst(ClaimTypes.NameIdentifier)
            ?.Value;
    }
}
