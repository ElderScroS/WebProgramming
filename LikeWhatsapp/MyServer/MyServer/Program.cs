using System.Threading;
using System;
using System.Net;
using System.Net.Sockets;

namespace MyServer
{
    internal class Program
    {
        private const string _serverHost = "localhost";
        private const int _serverPort = 27001;
        private static Thread _serverThread;

        static void Main(string[] args)
        {
            _serverThread = new Thread(StartServer);
            _serverThread.Start();

            while (true)
                HandleCommands(Console.ReadLine());
        }
        
        private static void HandleCommands(string cmd)
        {
            cmd = cmd.ToLower();
            if (cmd.Contains("/getusers"))
            {
                int countUsers = Server.Clients.Count;
                for (int i = 0; i < countUsers; i++)
                    Console.WriteLine("{0}: {1}", i, Server.Clients[i].UserName);
            }
        }
        private static void StartServer()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(_serverHost);
            IPAddress ipAddress = ipHost.AddressList[0];

            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, _serverPort);
            Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(ipEndPoint);
            socket.Listen(1000);
            Console.WriteLine($"Server started on IP: {ipEndPoint}.");

            while (true)
            {
                try
                {
                    Socket user = socket.Accept();
                    Server.NewClient(user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
