using System.Net.WebSockets;
using System.Text;

namespace OkxBackend.Manager
{
    public class WebSocketManager
    {
        private readonly ClientWebSocket _webSocket;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly string _okxWebSocketUrl = "wss://wspap.okx.com:8443/ws/v5/public?brokerId=9999\r\n";

        public WebSocketManager()
        {
            _webSocket = new ClientWebSocket();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task ConnectAndReceiveData(string message)
        {
            try
            {
                await _webSocket.ConnectAsync(new Uri(_okxWebSocketUrl), _cancellationTokenSource.Token);

                while (_webSocket.State == WebSocketState.Open)
                {
                    await SendMessageAsync(message);
                    // Alınan verileri işleyerek React uygulamasına gönderme
                    await ReceiveMessageLoop();
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi burada yapılabilir
            }
        }

        public async Task ReceiveData()
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);

            while (!result.EndOfMessage)
            {
                // Veri alımı işlemleri burada yapılabilir
                result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);
            }

            // Gelen verileri işleyip React uygulamasına gönderme işlemleri burada yapılabilir
            string receivedData = Encoding.UTF8.GetString(buffer);
            // React uygulamasına gönderme işlemi burada gerçekleştirilebilir
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
                    Console.WriteLine("Received message: " + message);
                    // Burada gelen mesajı işleyebilirsiniz
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }
            }
        }

    }
}
