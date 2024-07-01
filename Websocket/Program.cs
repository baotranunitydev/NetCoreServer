using NetCoreServer;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using WebSocket.Sever;
namespace WebSocket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // WebSocket server port
            int port = 7000;
            if (args.Length > 0)
                port = int.Parse(args[0]);
            // WebSocket server content path
            //string www = "../../../../www/wss";
            //if (args.Length > 1)
            //    www = args[1];

            Console.WriteLine($"WebSocket server port: {port}");
            //Console.WriteLine($"WebSocket server static content path: {www}");
            //Console.WriteLine($"WebSocket server website: https://localhost:{port}/game/index.html");

            Console.WriteLine();

            // Create and prepare a new SSL server context
            var context = new SslContext(SslProtocols.Tls13, new X509Certificate2("server.pfx", "qwerty"));

            // Create a new WebSocket server
            var server = new GameSever(context, IPAddress.Any, port);
            //server.AddStaticContent(www, "/game");
            server.InitSever();
            // Start the server
            Console.Write("Server starting...");
            server.Start();
            Console.WriteLine("Done!");

            Console.WriteLine("Press Enter to stop the server or '!' to restart the server...");

            // Perform text input
            for (; ; )
            {
                string? line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                // Restart the server
                if (line == "!")
                {
                    Console.Write("Server restarting...");
                    server.Restart();
                    Console.WriteLine("Done!");
                }

                // Multicast admin message to all sessions
                line = "(admin) " + line;
                server.MulticastText(line);
            }

            // Stop the server
            Console.Write("Server stopping...");
            server.Stop();
            Console.WriteLine("Done!");
        }
    }
}
