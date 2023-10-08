//Na primere napisannogo na uroke TcpClienta
//Napisat interfeys na Winforms
//Takje dobavit funkcional echo soobweniy.

//Klient otpravlaet 'ping' na server i ot servera vozvrawaetsa otvetniy ping

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeWor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ConfirmBtn_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 1)
            {
                MessageBox.Show("Error.");
            }
            var client = new TcpClient();
            client.Connect("127.0.0.1", 27001);
            var strem = client.GetStream();
            var br = new BinaryReader(strem);
            var bw = new BinaryWriter(strem);
            while (true)
            {
                var str = textBox1.Text.ToUpper();
                if (str == "HELP")
                {
                listBox1.Items.Add(Command.ProcList);
                listBox1.Items.Add(Command.Run);
                listBox1.Items.Add("HELP");
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
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

      

        public class Command
        {
            public const string ProcList = "PROCLIST";
            public const string Run = "RUN";

            public string Text { get; set; }
            public string Param { get; set; }
        }
    }
}
