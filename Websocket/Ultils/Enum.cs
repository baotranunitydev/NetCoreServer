using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Ultils
{
    internal enum TypeMessage
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
    }

    internal enum TypeStateRoom
    {
        None,
        Wait,
        Play,
    }

    internal enum TypeStatePlayer
    {
        None,
        Idle,
        InRoom,
        Ready,
        Play
    }

    internal enum TypePlayerInRoom
    {
        None,
        Owner,
        Other,
    }
}
