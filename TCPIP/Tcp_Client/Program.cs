using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace Tcp_Client
{
    public class Command
    {
        public const string ProcList = "PROCLIST";
        public const string Run = "RUN";

        public string Text { get; set; }
        public string Param { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var ip = IPAddress.Parse("127.0.0.1");
            var listener = new TcpListener(ip, 27001);
            var client = new TcpClient();
            client.Connect("127.0.0.1", 27001);
            var strem = client.GetStream();
            var br = new BinaryReader(strem);
            var bw = new BinaryWriter(strem);
            while (true)
            {
                var str = Console.ReadLine().ToUpper();
                if (str == "HELP")
                {
                    Console.WriteLine(Command.ProcList);
                    Console.WriteLine(Command.Run);
                    Console.WriteLine("HELP");
                    continue;
                }
                Command cmd = null;
                string response = null;

                var input = str.Split(' ');

                switch (input[0])
                {
                    case Command.ProcList:
                        cmd = new Command { Text = input[0] };
                        bw.Write(JsonSerializer.Serialize(cmd));
                        response = br.ReadString();

                        var processes = JsonSerializer.Deserialize<string[]>(response);

                        foreach (var process in processes)
                        {
                            Console.WriteLine(process);
                        }
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
