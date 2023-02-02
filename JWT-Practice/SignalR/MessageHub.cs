using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace JWT_Practice.SignalR
{
   // [Authorize]
    public class MessageHub:Hub<IMessageHubClient>
    {
        public async Task SendOffersToUser(string message)
        {
            await Clients.All.SendOffersToUser(message);
        }
    }
}   
