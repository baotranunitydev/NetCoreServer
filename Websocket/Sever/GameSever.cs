using NetCoreServer;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using WebSocket.Player;
using WebSocket.Room;
using WebSocket.Ultils;

namespace WebSocket.Sever
{
    internal class GameSever : WssServer
    {
        private PlayerSessionManager playerSessionManager;
        private RoomManager roomManager;
        private int maxPlayerInSever = 1000;

        public GameSever(SslContext context, IPAddress address, int port) : base(context, address, port)
        {

        }

        public void InitSever()
        {
            playerSessionManager = new PlayerSessionManager();
            roomManager = new RoomManager();
        }

        protected override SslSession CreateSession()
        {
            return new PlayerSession(this);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"WebSocket server caught an error with code {error}");
        }

        public void HandleMessageFromSession(PlayerSession playerSession, MessageData message)
        {
            try
            {
                var response = "";
                switch (message.typeMessage)
                {
                    case TypeMessage.None:
                        break;
                    case TypeMessage.ConnectWebSocket:
                        OnTryAddPlayerSessionToDic(playerSession, out response);
                        break;
                    case TypeMessage.GetUserModel:
                        OnGetPlayerSessionModel(playerSession, out response);
                        break;
                    case TypeMessage.UpdateModel:
                        OnUpdatePlayerSessionModel(playerSession, message, out response);
                        break;
                    case TypeMessage.CreateRoom:
                        break;
                    case TypeMessage.FindRoom:
                        break;
                    case TypeMessage.ChangePriceRoom:
                        break;
                    case TypeMessage.ChangeStatusReady:
                        break;
                    case TypeMessage.QuitRoom:
                        break;
                }
                playerSession.SendBinary(response);
                var jsonMessage = JsonConvert.SerializeObject(message);
                Console.WriteLine($"--------------------------");
                Console.WriteLine($"Session - {playerSession.Id} - TypeMessage: {message.typeMessage}");
                Console.WriteLine($"\nMessage form client: {jsonMessage}");
                Console.WriteLine($"\nResponse to client: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void OnTryAddPlayerSessionToDic(PlayerSession playerSession, out string response)
        {
            var isCanAdd = playerSessionManager.TryAddPlayerSessionToDic(playerSession);
            if (isCanAdd)
            {
                response = "Add Complete";
            }
            else
            {
                response = "Full";
            }
        }

        private void OnGetPlayerSessionModel(PlayerSession playerSession, out string response)
        {
            var getSessionModel = playerSession.GetPlayerSessionModel();
            var jsonPlayerSessionModel = JsonConvert.SerializeObject(getSessionModel);
            response = jsonPlayerSessionModel;
        }

        private void OnUpdatePlayerSessionModel(PlayerSession playerSession, MessageData message, out string response)
        {
            var playerSessionModel = JsonConvert.DeserializeObject<PlayerSessionModel>(message.message);
            playerSession.SetPlayerSessionModel(playerSessionModel);
            var jsonPlayerSessionModel = JsonConvert.SerializeObject(playerSession.GetPlayerSessionModel());
            response = jsonPlayerSessionModel;
        }
    }

    internal struct MessageData
    {
        public TypeMessage typeMessage;
        public string message;
    }
}
