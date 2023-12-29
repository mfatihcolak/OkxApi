using Microsoft.AspNetCore.SignalR;
using OkxBackend.Hubs;
using System.Net.WebSockets;
using System.Text;

namespace OkxBackend.Manager
{
    public class WebSocketManager
    {
        private readonly ClientWebSocket _webSocket;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly string _okxWebSocketUrl = "wss://wspap.okx.com:8443/ws/v5/public?brokerId=9999";
        private readonly IHubContext<CoinInfo> _hubContext;

        public WebSocketManager(IHubContext<CoinInfo> hubContext)
        {
            _webSocket = new ClientWebSocket();
            _cancellationTokenSource = new CancellationTokenSource();
            _hubContext = hubContext;
        }

        public async Task ConnectAndReceiveData(string message)
        {
            try
            {
                await _webSocket.ConnectAsync(new Uri(_okxWebSocketUrl), _cancellationTokenSource.Token);

                while (_webSocket.State == WebSocketState.Open)
                {
                    await SendMessageAsync(message);
                    await ReceiveMessageLoop();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task ReceiveData()
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);

            while (!result.EndOfMessage)
            {
                result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);
            }

            string receivedData = Encoding.UTF8.GetString(buffer);
        }

        public async Task Disconnect()
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                _cancellationTokenSource.Cancel();
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                _webSocket.Dispose();
            }
        }

       public async Task SendMessageAsync(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<byte>(bytes);
            await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task ReceiveMessageLoop()
        {
            var buffer = new byte[1024];
            var segment = new ArraySegment<byte>(buffer);

            while (_webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(segment, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine(message);

                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }
            }
        }

    }
}
