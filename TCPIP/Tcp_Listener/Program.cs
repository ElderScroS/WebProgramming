using System;
using System.Net.Sockets;
using System.Text.Json;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Linq;

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
                    if (command == null)
                    {
                        continue;
                    }
                    Console.WriteLine(command.Text);
                    Console.WriteLine(command.Param);

                    switch (command.Text)
                    {
                        case Command.ProcList:
                            var processes = Process.GetProcesses();
                            bw.Write(JsonSerializer.Serialize(processes.Select(p => p.ProcessName)));
                            break;
                        case Command.Run:
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
    public class Command
    {
        public const string ProcList = "PROCLIST";
        public const string Run = "RUN";

        public string Text { get; set; }
        public string Param { get; set; }
    }

}
