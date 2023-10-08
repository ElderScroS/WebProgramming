using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        async private void StartBtn_Click(object sender, EventArgs e)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            IPAddress Ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint Ep = new IPEndPoint(Ip, 1024);

            socket.Bind(Ep);
            socket.Listen(10);

            try
            {
                while (true)
                {
                    Socket newS = await socket.AcceptAsync();
                    SocketListBox.Items.Add(newS.RemoteEndPoint.ToString());
                    newS.Send(System.Text.Encoding.ASCII.GetBytes(DateTime.Now.ToString()));
                    newS.Shutdown(SocketShutdown.Both);
                    newS.Close();
                }
            }
            catch (SocketException Ex)
            {
                MessageBox.Show(Ex.Message);
                throw;
            }
        }
    }
}
