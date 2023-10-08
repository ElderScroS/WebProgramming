using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyServer
{
    public static class Server
    {
        public static List<Client> Clients = new List<Client>();

        public static void NewClient(Socket handle)
        {
            try
            {
                Client newClient = new Client(handle);
                Clients.Add(newClient);

                Console.WriteLine($"Client connected: {handle.RemoteEndPoint}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void EndClient(Client client)
        {
            client.End();
            Clients.Remove(client);

            Console.WriteLine($"User {client.UserName} disconnected.");
        }
        public static void UpdateAllChats()
        {
            try
            {
                for (int i = 0; i < Clients.Count; i++)
                    Clients[i].UpdateChat();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
