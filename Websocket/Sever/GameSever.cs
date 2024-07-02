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
        private PlayerSessionManager? playerSessionManager;
        private RoomManager? roomManager;
        private int maxPlayerInSever = 1000;

        public GameSever(SslContext context, IPAddress address, int port) : base(context, address, port)
        {

        }

        public void InitSever()
        {
            playerSessionManager = new PlayerSessionManager();
            playerSessionManager.SetMaxPlayerSessionInSever(maxPlayerInSever);
            roomManager = new RoomManager();
        }

        protected override void OnStarted()
        {
            base.OnStarted();
            Console.WriteLine("Start Sever");
        }

        protected override SslSession CreateSession()
        {
            return new PlayerSession(this);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"WebSocket server caught an error with code {error}");
        }

        public void HandleMessageFromSession(PlayerSession playerSession, SeverRequest message)
        {
            try
            {
                var response = "";
                switch (message.typeMessage)
                {
                    case TypeRequest.None:
                        break;
                    case TypeRequest.UpdateModel:
                        OnUpdatePlayerSessionModel(playerSession, message, out response);
                        break;
                    case TypeRequest.ConnectWebSocket:
                        OnTryAddPlayerSessionToDic(playerSession, out response);
                        break;
                    case TypeRequest.GetUserModel:
                        OnGetPlayerSessionModel(playerSession, out response);
                        break;
                    case TypeRequest.CreateRoom:
                        break;
                    case TypeRequest.FindRoom:
                        break;
                    case TypeRequest.JoinRoomWithPassword:
                        break;
                    case TypeRequest.ChangePriceRoom:
                        break;
                    case TypeRequest.ChangeStatusReady:
                        break;
                    case TypeRequest.QuitRoom:
                        break;
                    case TypeRequest.OnDisconnect:
                        OnDisconnected(playerSession, message, out response);
                        break;
                }
                playerSession.SendAsync(response);
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
            if (playerSessionManager == null)
            {
                response = "Error";
                return;
            };
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

        private void OnUpdatePlayerSessionModel(PlayerSession playerSession, SeverRequest message, out string response)
        {
            var playerSessionModel = JsonConvert.DeserializeObject<PlayerSessionModel>(message.message);
            playerSession.SetPlayerSessionModel(playerSessionModel);
            var jsonPlayerSessionModel = JsonConvert.SerializeObject(playerSession.GetPlayerSessionModel());
            response = jsonPlayerSessionModel;
        }

        private void OnDisconnected(PlayerSession playerSession, SeverRequest message, out string response)
        {
            response = "";
            try
            {
                if (playerSessionManager == null || message.message == null)
                {
                    return;
                }
                var playerSessionModel = JsonConvert.DeserializeObject<PlayerSessionModel>(message.message);
                playerSessionManager.RemovePlayerSessionFormDics(playerSessionModel.id);
                var jsonPlayerSessionModel = JsonConvert.SerializeObject(playerSession.GetPlayerSessionModel());
                response = jsonPlayerSessionModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
