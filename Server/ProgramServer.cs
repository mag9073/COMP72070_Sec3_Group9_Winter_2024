using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.Implementations;
using ProtoBuf;
using static Server.DataStructure.PacketData;
using Server.Interfaces;

namespace Server
{
    public class ProgramServer
    {
        private static TcpListener _tcpListener;
        private static bool _isRunning;
        private static UserDataManager userDataManager = new UserDataManager();
        private static ParkDataManager parkDataManager = new ParkDataManager();
        private static ParkReviewManager parkReviewManager = new ParkReviewManager();
        private static Logger logger = new Logger("log.txt"); // Adjust the path as necessary
        private static ImageManager imageManager = new ImageManager();
        private static PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager);

        static void Main(string[] args)
        {
            StartServer(13000);
            Console.WriteLine("Server is running. Press Enter to stop.");
            Console.ReadLine();
            StopServer();
        }

        private static void StartServer(int port)
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

        private static void HandleClient(TcpClient client)
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

        private static void StopServer()
        {
            _isRunning = false;
            _tcpListener.Stop();
            Console.WriteLine("Server stopped.");
        }
    }
}
