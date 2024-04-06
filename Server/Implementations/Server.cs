using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.Interfaces;
using ProtoBuf;
using static Server.DataStructure.PacketData;

namespace Server.Implementations
{
    public class Server: IServer
    {
        private static TcpListener _tcpListener;
        public bool _isRunning;
        private static UserDataManager userDataManager = new UserDataManager();
        private static ParkDataManager parkDataManager = new ParkDataManager();
        private static ParkReviewManager parkReviewManager = new ParkReviewManager();
        private static ServerStateManager serverStateManager = new ServerStateManager();
        private static UserDataManager.LoginData loginDataManager = new UserDataManager.LoginData();
        private static Logger logger = new Logger("../../../Database/log.txt"); 
        private static ImageManager imageManager = new ImageManager();
        private static PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager, serverStateManager);
        private static List<TcpClient> clients = new List<TcpClient>();     // To store client connections in a list of pool 

        public Server()
        {
            _isRunning = true;
            // Need to integrate state machine here too
        }

        public void StartServer(int port)
        {
            _isRunning = true;
            _tcpListener = new TcpListener(IPAddress.Loopback, port);
            _tcpListener.Start();
            Console.WriteLine($"Server started on port {port}.");

            serverStateManager.SetCurrentState(ServerState.Connected);

            _ = ThreadPool.QueueUserWorkItem(new WaitCallback(AcceptClients));
        }

        private static void AcceptClients(object state)
        {
            while (true)
            {
                TcpClient client = _tcpListener.AcceptTcpClient();

                    clients.Add(client);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(HandleClient), client);

            }
        }


        private static void HandleClient(Object state)
        {
            TcpClient client = (TcpClient)state;

            try
            {
                ICommunicationChannel stream = new NetworkStreamCommunication(client.GetStream());
                while (client.Connected)
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);

                        Packet packet;
                        using (MemoryStream ms = new MemoryStream(buffer, 0, bytesRead))
                        {
                            packet = Serializer.Deserialize<Packet>(ms);
                        }
                        logger.LogPacket("Receive", packet, serverStateManager, loginDataManager);

                        packetProcessor.ProcessPacket(packet, stream, client);
                    }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                // Remove connection
                clients.Remove(client);
                client.Close();
            }
        }

        public void StopServer()
        {
            _isRunning = false;
            _tcpListener.Stop();
            Console.WriteLine("Server stopped.");
        }

    }
}
