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
        CreateRoom,
        JoinRoom,
        FindRoom,
        ChangePriceRoom,
        ChangeStatusReady,
        QuitRoom,
    }
}
