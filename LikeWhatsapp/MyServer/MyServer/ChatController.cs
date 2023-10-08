using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer
{
    public static class ChatController
    {
        private const int _maxMessage = 100;
        public static List<Message> Chat = new List<Message>();

        public struct Message
        {
            public string _userName;
            public string _data;

            public Message(string userName, string data)
            {
                this._userName = userName;
                this._data = data;
            }
        }

        public static void AddMessage(string userName, string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(msg))
                    return;
                
                int countMessages = Chat.Count;
                if (countMessages> _maxMessage)
                    ClearChat();

                Message message = new Message(userName, msg);
                Chat.Add(message);

                Console.WriteLine($"New message from {userName}.");
                Server.UpdateAllChats();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void ClearChat() => Chat.Clear();

        public static string GetChat()
        {
            try
            {
                string data = "#updatechat&";

                int countMessages = Chat.Count;
                if (countMessages <= 0)
                    return string.Empty;

                for (int i = 0; i < countMessages; i++)
                    data += String.Format("{0}~{1}|", Chat[i]._userName, Chat[i]._data);

                return data;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
