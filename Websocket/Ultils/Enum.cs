using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Ultils
{
    public enum TypeMessage
    {
        None,
        ConnectWebSocket,
        GetUserModel,
        UpdateModel,
        CreateRoom,
        FindRoom,
        ChangePriceRoom,
        ChangeStatusReady,
        QuitRoom,
        OnDisconnect,
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
        InRoom,
        Ready,
        Play
    }

    public enum TypePlayerInRoom
    {
        None,
        Owner,
        Other,
    }
}
