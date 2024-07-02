using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Ultils;

namespace WebSocket.Sever
{
    public struct SeverRequest
    {
        public TypeRequest typeMessage;
        public string message;
    }

    public struct ServerResponse
    {
        public TypeResponse typeResponse;
        public string message;
    }
}
