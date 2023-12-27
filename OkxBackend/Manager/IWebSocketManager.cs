namespace OkxBackend.Manager
{
    public interface IWebSocketManager
    {
        public Task ConnectAndReceiveData();

        public Task ReceiveData();

        public Task Disconnect();
        public Task ReceiveMessageLoop();
    }
}
