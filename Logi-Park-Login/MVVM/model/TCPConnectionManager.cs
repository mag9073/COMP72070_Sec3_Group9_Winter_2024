using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LogiPark.MVVM.Model
{
    // This will act as a central TCP Connection Manager for the Client Side to main DRY techniques
    public class TcpConnectionManager
    {
        private static TcpConnectionManager TCPInstance = new TcpConnectionManager();
        public TcpClient client;
        public NetworkStream stream;
        private string serverAddress = "127.0.0.1";
        private int serverPort = 13000;

        private TcpConnectionManager()
        {
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                client = new TcpClient(serverAddress, serverPort);
                stream = client.GetStream();
                Console.WriteLine("Connection established successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to server: {ex.Message}");
            }
        }

        public static TcpConnectionManager Instance
        {
            get
            {
                return TCPInstance;
            }
        }

        public void CloseConnection()
        {
            stream.Close();
            client.Close();
        }
    }
}
