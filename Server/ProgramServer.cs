using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ProtoBuf;
using static Server.UserDataManager;

namespace Server
{
    public class ProgramServer
    {
        public static void Main(string[] args)
        {
            StartServer();
        }

        public static void StartServer()
        {
            InitializeServer(13000);
        }

        private static TcpListener CreateServer(int portNumber)
        {
            return new TcpListener(IPAddress.Loopback, portNumber);
        }

        public static void InitializeServer(int portNumber)
        {
            TcpListener server = CreateServer(portNumber);
            server.Start();

            Console.WriteLine("Server started on port " + portNumber);

            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    HandleClient(client);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SocketException: " + ex.Message);
            }
            finally
            {
                server.Stop();
            }
        }

        public static void HandleClient(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                try
                {
                    // Enter a loop to continuously process incoming packets
                    while (client.Connected)    // doesn't seem to keep connection 
                    {
                        Packet packet = ReceivePacket(stream);
                        ProcessPacket(packet, stream, client);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public static Packet ReceivePacket(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            using (MemoryStream memStream = new MemoryStream(buffer, 0, bytesRead))
            {
                return Serializer.Deserialize<Packet>(memStream);
            }
        }

        public static void SendAcknowledgement(NetworkStream stream, string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            Console.WriteLine($"Acknowledgement: {message}");
            stream.Write(messageBytes, 0, messageBytes.Length);
            stream.Flush();
        }

        public static void ProcessPacket(Packet packet, NetworkStream stream, TcpClient client)
        {
            // Process packets based on type
            switch (packet.GetPacketHeader().GetType())
            {
                case Types.login:
                    ProcessLoginPacket(packet, stream, client);
                    break;

                case Types.register:

                    break;

                // Send image?
                case Types.send:

                    break;
            }
        }

        /*** Helper Functions ***/

        private static void ProcessLoginPacket(Packet packet, NetworkStream stream, TcpClient client)
        {
            byte[] buffer = packet.GetBody().buffer;
            if (buffer != null && buffer.Length > 0)
            {
                UserDataManager.LoginData loginData = new UserDataManager.LoginData();
                loginData = loginData.deserializeLoginData(buffer);

                string message = PerformLogin(loginData);
                SendAcknowledgement(stream, message);
            }
            else
            {
                Console.WriteLine("Packet bodyBuffer is empty or null");
                stream.Close();
                client.Close();
            }
        }

        private static string PerformLogin(UserDataManager.LoginData loginData)
        {
            Console.WriteLine($"Username: {loginData.GetUserName()}");
            Console.WriteLine($"Password: {loginData.GetPassword()}");

            Login login = new Login(loginData);
            return login.LoginUser("../../../UserDB.txt");
        }

        // still has yet to implement park data retrival because of issue with client connection loss 
        private static void ProcessParkDataRetrievalAll(Packet packet, NetworkStream stream)
        {
            ParkDataManager.ParkData[] parks = RetrieveParkDataFromFile("../../../ParkData.txt");

            for (int i = 0; i < parks.Length; i++)
            {
                byte[] buffer = parks[i].SerializeToByteArray();
                stream.Write(buffer, 0, buffer.Length);
            }
            Console.WriteLine("All park data sent to client");
        }

        private static ParkDataManager.ParkData[] RetrieveParkDataFromFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            ParkDataManager.ParkData[] parks = new ParkDataManager.ParkData[lines.Length / 4];

            for (int i = 0; i < parks.Length; i++)
            {
                int index = i * 4;
                // this setup is based on the parkdata.txt file
                parks[i] = new ParkDataManager.ParkData
                {
                    parkName = lines[index],
                    parkReview = float.Parse(lines[index + 1]),
                    parkDescription = lines[index + 2],
                    numberOfReviews = uint.Parse(lines[index + 3]),
                };
            }
            return parks;
        }
    }
}
