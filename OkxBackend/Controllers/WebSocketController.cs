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
        public const string ltcUsdt = "{\"op\": \"subscribe\", \"args\": [{\"channel\": \"books\", \"instId\": \"LTC-USDT\"}]}";
        public const string ethUsdt = "{\"op\": \"subscribe\", \"args\": [{\"channel\": \"books\", \"instId\": \"ETH-USDT\"}]}";
        public const string dogeUsdt = "{\"op\": \"subscribe\", \"args\": [{\"channel\": \"books\", \"instId\": \"DOGE-USDT\"}]}";
        public const string solUsdt = "{\"op\": \"subscribe\", \"args\": [{\"channel\": \"books\", \"instId\": \"SOL-USDT\"}]}";

        public WebSocketController(WebSocketManager webSocketManager)
        {
            _webSocketManager = webSocketManager;
        }

        [HttpGet("connect-btc-usdt")]
        public async Task<IActionResult> ConnectBtcUsdtWebSocket()
        {
            try
            {
                await _webSocketManager.ConnectAndReceiveData(btcUsdt);
                
                return Ok("WebSocket connection established for LTC-USDT.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("connect-ltc-usdt")]
        public async Task<IActionResult> ConnectLtcUsdtWebSocket()
        {
            try
            {
                await _webSocketManager.ConnectAndReceiveData(ltcUsdt);
                return Ok("WebSocket connection established for LTC-USDT.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("connect-eth-usdt")]
        public async Task<IActionResult> ConnectEthUsdtWebSocket()
        {
            try
            {
                await _webSocketManager.ConnectAndReceiveData(ethUsdt);
                return Ok("WebSocket connection established for LTC-USDT.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("connect-doge-usdt")]
        public async Task<IActionResult> ConnectDogeUsdtWebSocket()
        {
            try
            {
                await _webSocketManager.ConnectAndReceiveData(dogeUsdt);
                return Ok("WebSocket connection established for DOGE-USDT.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("connect-sol-usdt")]
        public async Task<IActionResult> ConnectSolUsdtWebSocket()
        {
            try
            {
                await _webSocketManager.ConnectAndReceiveData(solUsdt);
                return Ok("WebSocket connection established for SOL-USDT.");
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
