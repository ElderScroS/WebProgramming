using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace Tcp_Listener
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ip = IPAddress.Parse("127.0.0.1");
            var listener = new TcpListener(ip, 27001);
            listener.Start(100);

            while (true)
            {
                var client = listener.AcceptTcpClient();
                var strem = client.GetStream();
                var br = new BinaryReader(strem);
                var bw = new BinaryWriter(strem);
                while (true)
                {
                    var input = br.ReadString();
                    var command = JsonSerializer.Deserialize<Command>(input);
                }
            }
        }

        public class Command
        {
            public const string ProcList = "PROCLIST";
            public const string Run = "RUN";
        }
    }
}
