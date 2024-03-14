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
        private static bool _isRunning;
        private static UserDataManager userDataManager = new UserDataManager();
        private static ParkDataManager parkDataManager = new ParkDataManager();
        private static ParkReviewManager parkReviewManager = new ParkReviewManager();
        private static Logger logger = new Logger("log.txt"); 
        private static ImageManager imageManager = new ImageManager();
        private static PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager);

        public Server()
        {
            _isRunning = true;
        }

        public void StartServer(int port)
        {
            _isRunning = true;
            _tcpListener = new TcpListener(IPAddress.Loopback, port);
            _tcpListener.Start();
            Console.WriteLine($"Server started on port {port}.");

            while (_isRunning)
            {
                TcpClient client = _tcpListener.AcceptTcpClient();
                Console.WriteLine("Client connected.");
                HandleClient(client);
            }
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                ICommunicationChannel stream = new NetworkStreamCommunication(client.GetStream());
                {
                    while (client.Connected)
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);

                        Packet packet;
                        using (MemoryStream ms = new MemoryStream(buffer, 0, bytesRead))
                        {
                            packet = Serializer.Deserialize<Packet>(ms);
                        }

                        packetProcessor.ProcessPacket(packet, stream, client);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
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
