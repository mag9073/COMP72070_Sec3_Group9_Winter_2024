using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.MVVM.Model
{
    public class ProgramClient
    {
        private UserDataManager.LoginData clientLoginData = new UserDataManager.LoginData();
        private ParkDataManager.ParkData clientParkData = new ParkDataManager.ParkData();
        private TcpClient clientTcpClient;
        private NetworkStream stream;

        // Only used for testing without GUI
        //static void Main(string[] args)
        //{
        //    ProgramClient client = new ProgramClient("127.0.0.1", 13000);
        //    try
        //    {
        //        client.PromptForCredentials();
        //        client.SendLoginRequest();
        //        string response = client.ReceiveServerResponse();
        //        Console.WriteLine(response);
        //    }
        //    finally
        //    {
        //        client.CloseConnection();
        //    }
        //}

        private TcpConnectionManager connectionManager;

        public ProgramClient()
        {
            connectionManager = TcpConnectionManager.Instance;
            stream = connectionManager.stream;
        }

        // Only used for testing without GUI
        //public void PromptForCredentials()
        //{
        //    Console.WriteLine("Enter username:");
        //    string username = Console.ReadLine();

        //    Console.WriteLine("Enter password:");
        //    string password = Console.ReadLine();



        //    clientLoginData.SetUserName(username);
        //    clientLoginData.SetPassword(password);
        //}

        /*** User Data Manager - Login ***/
        public void SendLoginRequest(UserDataManager.LoginData loginData)
        {
            this.clientLoginData = loginData;

            Packet sendPacket = new Packet();
            sendPacket.SetPacketHead(1, 2, Types.login);

            byte[] loginDataBuffer = clientLoginData.SerializeToByteArray();
            sendPacket.SetPacketBody(loginDataBuffer, (uint)loginDataBuffer.Length);

            byte[] packetBuffer = sendPacket.SerializeToByteArray();
            stream.Write(packetBuffer, 0, packetBuffer.Length);
        }

        /*** Park Data Manager ***/
        public void SendParkDataRetrivalAll()
        {
            Packet sendPacket = new Packet();
            sendPacket.SetPacketHead(1, 2, Types.send);

            byte[] parkDataBuffer = clientParkData.SerializeToByteArray();
            sendPacket.SetPacketBody(parkDataBuffer, (uint)parkDataBuffer.Length);

            byte[] packetBuffer = sendPacket.SerializeToByteArray();
            stream.Write(packetBuffer, 0, packetBuffer.Length);
        }

        public string ReceiveServerResponse()
        {
            byte[] responseBuffer = new byte[1024];
            int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
            return Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
        }

        public void CloseConnection()
        {
            stream.Close();
        }

    }
}
