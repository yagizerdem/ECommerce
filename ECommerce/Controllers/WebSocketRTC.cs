using ECommerce.ChatHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ECommerce.Controllers
{
    public class WebSocketRTC : Controller
    {
        // Inject the hub context into your controller or service
        private readonly IHubContext<ChatHub.ChatHub> _hubContext;
        public WebSocketRTC(IHubContext<ChatHub.ChatHub> hubContext)
        {
            this._hubContext = hubContext;
        }
        public async Task<IActionResult> SendMessageToUser(string userId, string message)
        {
            // Send a private message to the specified user
            await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", message);

            return Ok();
        }
    }
}
