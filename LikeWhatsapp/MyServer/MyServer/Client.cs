using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace MyServer
{
    public class Client
    {
        private string _userName;
        private Socket _handler;
        private Thread _userThread;

        public string UserName
        {
            get { return _userName; }
        }

        public Client(Socket socket)
        {
            _handler = socket;
            _userThread = new Thread(Listener);
            _userThread.IsBackground = true;
            _userThread.Start();
        }

        private void Listener()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRec = _handler.Receive(buffer);
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRec);

                    HandleCommand(data);
                }
                catch (Exception)
                {
                    Server.EndClient(this);
                    return;
                }
            }
        }

        public void End()
        {
            try
            {
                _handler.Close();
                try
                {
                    _userThread.Abort();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void HandleCommand( string data)
        {
            if (data.Contains("#setname"))
            {
                _userName = data.Split('&')[1];
                UpdateChat();

                return;
            }
            if (data.Contains("#newmsg"))
            {
                string message = data.Split('&')[1];
                ChatController.AddMessage(_userName, message);

                return;
            }
        }

        public void Send(string command)
        {
            try
            {
                int bytesSent = _handler.Send(Encoding.UTF8.GetBytes(command));

                if (bytesSent > 0)
                    Console.WriteLine("success!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void UpdateChat() => Send(ChatController.GetChat());
    }
}
