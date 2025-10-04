using Microsoft.AspNetCore.SignalR;

namespace ChatMessages.Application.Hubs;

public class QueryUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.GetHttpContext()?.Request.Query["userId"];
    }
}
