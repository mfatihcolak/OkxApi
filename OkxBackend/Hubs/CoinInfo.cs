using Microsoft.AspNetCore.SignalR;

namespace OkxBackend.Hubs
{
    public class CoinInfo : Hub
    {
        public async Task SendMessageToReact(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
