using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Player;

namespace WebSocket.Room
{
    internal class RoomRequest
    {
    }


    public struct CreateRoomRequest()
    {
        public string roomName;
        public int password;
        public int priceRoom;
        public PlayerSessionModel playerSessionModel;
    }

    public struct FindRoomRequest()
    {
        public int roomId;
        public PlayerSessionModel playerSessionModel;
    }
    public struct JoinRoomPassowrdRequest()
    {
        public int roomId;
        public int password;
        public PlayerSessionModel playerSessionModel;
    }

    public struct ChangeStatusRequest()
    {
        public int roomId;
        public PlayerSessionModel playerSessionModel;
    }

    public struct ChangePriceRoomRequest()
    {
        public int roomId;
        public int priceRoom;
        public PlayerSessionModel playerSessionModel;
    }

    public struct PlayerQuitRoomRequest()
    {
        public int roomId;
        public PlayerSessionModel playerSessionModel;
    }
}
