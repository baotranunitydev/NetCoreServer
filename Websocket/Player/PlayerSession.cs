using NetCoreServer;
using System.Net.Sockets;
using System.Text;
using WebSocket.Sever;
using WebSocket.Ultils;
using Newtonsoft;
using Newtonsoft.Json;

namespace WebSocket.Player
{
    internal class PlayerSession : WssSession
    {
        private PlayerSessionModel playerSessionModel;

        public PlayerSessionModel GetPlayerSessionModel() => playerSessionModel;
        public void SetPlayerSessionModel(PlayerSessionModel playerSessionModel)
        {
            this.playerSessionModel = playerSessionModel;
        }

        public PlayerSession(WssServer server) : base(server)
        {

        }

        public override void OnWsConnected(HttpRequest request)
        {
            Console.WriteLine($"Chat WebSocket session with Id {Id} connected!");

            // Send invite message
            string message = "Hello from WebSocket! Please send a message or '!' to disconnect the client!";
            SendTextAsync(message);
        }

        public override void OnWsDisconnected()
        {
            Console.WriteLine($"WebSocket session with Id {Id} disconnected!");
        }

        public override void OnWsReceived(byte[] buffer, long offset, long size)
        {
            try
            {
                string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
                var messageData = JsonConvert.DeserializeObject<MessageData>(message);
                (Server as GameSever)?.HandleMessageFromSession(this, messageData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"WebSocket session caught an error with code {error}");
        }
    }

    internal struct PlayerSessionModel
    {
        public TypeStatePlayer statePlayer;
        public string name;
        public string id;
        public long point;
    }
}
