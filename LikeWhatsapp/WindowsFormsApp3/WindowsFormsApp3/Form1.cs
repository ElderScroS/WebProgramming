using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private delegate void printer(string data);
        private delegate void cleaner();

        printer Printer;
        cleaner Cleaner;

        private Socket _serverSocket;
        private Thread _clientThread;

        private const string _serverHost = "localhost";
        private const int _serverPort = 27001;

        #region Funcs

        private void SendMessages()
        {
            try
            {
                if (string.IsNullOrEmpty(MessageTextBox.Text))
                    return;

                Send("#newmsg&" + MessageTextBox.Text);
                MessageTextBox.Text = "";
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return;    
            }
        }
        private void Listener()
        {
            while (_serverSocket.Connected)
            {
                byte[] buffer = new byte[1024];
                int bytesrec = _serverSocket.Receive(buffer);
                string data = Encoding.UTF8.GetString(buffer, 0, bytesrec);

                if (data.Contains("#updatechat&"))
                {
                    UpdateChat(data);
                    continue;
                }
            }
        }
        private void Connect()
        {
            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry(_serverHost);
                IPAddress ipAdress = ipHost.AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAdress, _serverPort);

                _serverSocket = new Socket(ipAdress.AddressFamily,SocketType.Stream,ProtocolType.Tcp);
                _serverSocket.Connect(ipEndPoint);
            }
            catch(Exception ex)
            {
                Print("Server is not available. . . ");
            }
        }
        private void ClearChat()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(Cleaner);
                return;
            }

            ChatTextBox.Clear();
        }
        private void Print(string msg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(Printer, msg);
                return;
            }
            if (ChatTextBox.Text.Length == 0)
                ChatTextBox.AppendText(msg);
            else
                ChatTextBox.AppendText(Environment.NewLine + msg);
        }
        private void UpdateChat(string data)
        {
            ClearChat();

            string[] messages = data.Split('&')[1].Split('|');
            if (messages.Length <= 0)
                return;

            for (int i = 0; i < messages.Length; i++)
            {
                try
                {
                    if (string.IsNullOrEmpty(messages[i]))
                        continue;

                    Print(String.Format($"[{messages[i].Split('~')[0]}]: {messages[i].Split('~')[1]}"));
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

        }
        private void Send(string data)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int bytesSent = _serverSocket.Send(buffer);
            }
            catch (Exception ex)
            {
                Print(ex.Message);
            }
        }

        #endregion

        public Form1()
        {
            InitializeComponent();

            ChatTextBox.Enabled = false;
            MessageTextBox.Enabled = false;
            SendMessageBtn.Enabled = false;

            Printer = new printer(Print);
            Cleaner = new cleaner(ClearChat);

            Connect();

            _clientThread = new Thread(Listener);
            _clientThread.IsBackground = true;
            _clientThread.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                SendMessages();
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameTextBox.Text))
                return;

            Send("#setname&" + UsernameTextBox.Text);

            ConnectBtn.Enabled = false;
            UsernameTextBox.Enabled = false;

            ChatTextBox.Enabled = true;
            MessageTextBox.Enabled = true;
            SendMessageBtn.Enabled = true;
        }

        private void SendMessageBtn_Click(object sender, EventArgs e) => SendMessages();
        
    }
}
