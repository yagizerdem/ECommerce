using Microsoft.AspNetCore.SignalR;

namespace ECommerce.ChatHub
{
    public class ChatHub : Hub
    {
        public async Task SendPrivateMessage(string userId, string message)
        {
            // Send the private message to the specified user
            await Clients.User(userId).SendAsync("ReceiveMessage", message);
        }
    }
}
