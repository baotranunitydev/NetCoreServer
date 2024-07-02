using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Ultils
{
    public enum TypeRequest
    {
        None,
        //Player
        ConnectWebSocket,
        GetUserModel,
        UpdateModel,
        //Room
        CreateRoom,
        FindRoom,
        ChangePriceRoom,
        ChangeStatusReady,
        JoinRoomWithPassword,
        QuitRoom,
        OnDisconnect,
    }

    public enum TypeResponse
    {
        None,
        Success,
        // Player
        SeverMax,
        Have2PlayerId,
        // Room
        RoomFull,
        RoomNotExist,
        EnterPassword,
        PasswordError,
        NotEnoughPointToJoin,
    }

    public enum TypeStateRoom
    {
        None,
        Wait,
        Full,
        Play,
    }

    public enum TypeStatePlayer
    {
        None,
        Idle,
        Ready,
        Play,
    }
}
