using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Websocket
{
    internal class GameSever : WssServer
    {
        public GameSever(SslContext context, IPAddress address, int port) : base(context, address, port)
        {

        }

        protected override SslSession CreateSession()
        {
            return new GameSession(this);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"WebSocket server caught an error with code {error}");
        }
    }
}
