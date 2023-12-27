using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OkxBackend.Manager;
using WebSocketManager = OkxBackend.Manager.WebSocketManager;

namespace OkxBackend.Controllers
{
    [ApiController]
    [Route("api/websocket")]
    public class WebSocketController : ControllerBase
    {
        private readonly WebSocketManager _webSocketManager;

        public const string btcUsdt = "{\"op\": \"subscribe\", \"args\": [{\"channel\": \"books\", \"instId\": \"BTC-USDT\"}]}";

        public WebSocketController(WebSocketManager webSocketManager)
        {
            _webSocketManager = webSocketManager;
        }

        [HttpGet("connect")]
        public async Task<IActionResult> ConnectWebSocket([FromBody]string request)
        {
            try
            {
                await _webSocketManager.ConnectAndReceiveData(request);

                
                return Ok("WebSocket connection established and data receiving started.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("disconnect")]
        public async Task<IActionResult> DisconnectWebSocket()
        {
            try
            {
                await _webSocketManager.Disconnect();
                return Ok("WebSocket connection closed.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
