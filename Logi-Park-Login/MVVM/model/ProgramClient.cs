using LogiPark.MVVM.Model;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LogiPark.MVVM.Model
{
    public class ProgramClient
    {
        private UserDataManager.LoginData clientLoginData = new UserDataManager.LoginData();
        private ParkDataManager.ParkData clientParkData = new ParkDataManager.ParkData();
        //private TcpClient clientTcpClient;
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
        public void SendParkDataAllRequest()
        {
            Packet sendPacket = new Packet();
            sendPacket.SetPacketHead(1, 2, Types.allparkdata);

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

        public ParkDataManager.ParkData[] ReceiveParkDataAllResponse()
        {
            
            byte[] countBuffer = new byte[4];
            
            // this will read the first 4 bytes from the stream we sent back from the server and is the total number of park data objects in int
            int bytesRead = stream.Read(countBuffer, 0, 4);

            if (bytesRead != 4) throw new Exception("Failed to read park data count.");

            // we need to convert the first 4 bytes from stream read into integer 
            int count = BitConverter.ToInt32(countBuffer, 0);

            // this hold park data obj based on the num of count
            ParkDataManager.ParkData[] parks = new ParkDataManager.ParkData[count];

            // loop thorugh count - num of park data obj expected to receive
            for (int i = 0; i < count; i++)
            {
                byte[] lengthBuffer = new byte[4];
                
                // again we read the next 4 bytes from stream read which contains the length of the park data obj
                bytesRead = stream.Read(lengthBuffer, 0, 4);

                if (bytesRead != 4) throw new Exception("Failed to read park data length.");

                // we need to convert the first 4 bytes from stream read into integer, it is the length of the park data obj
                int dataLength = BitConverter.ToInt32(lengthBuffer, 0);

                byte[] parkBuffer = new byte[dataLength];

                // we read the park data obj from stream 
                bytesRead = stream.Read(parkBuffer, 0, dataLength);
                if (bytesRead != dataLength) throw new Exception("Failed to read complete park data.");

                using (MemoryStream ms = new MemoryStream(parkBuffer))
                {
                    // finally, we will deserialize the serialize park data object and format it back to park data obj
                    parks[i] = Serializer.Deserialize<ParkDataManager.ParkData>(ms);
                }
            }

            return parks;
        }

        /* Park All Images Request */
        public void SendImageRequest()
        {
            Packet sendPacket = new Packet();
            sendPacket.SetPacketHead(1, 2, Types.allparkimages);

            // We dont need to send body in this request 
            byte[] packetBuffer = sendPacket.SerializeToByteArray();
            stream.Write(packetBuffer, 0, packetBuffer.Length);
        }

        /* Park Review Manager - Individual Park Reviews */
        public void SendParkReviewsRequest(string parkName)
        {
            Packet sendPacket = new Packet();
            sendPacket.SetPacketHead(1, 2, Types.review);

            // Serialize the park name and set as packet body
            byte[] parkNameBuffer = Encoding.UTF8.GetBytes(parkName);
            sendPacket.SetPacketBody(parkNameBuffer, (uint)parkNameBuffer.Length);

            // Send the packet
            byte[] packetBuffer = sendPacket.SerializeToByteArray();
            stream.Write(packetBuffer, 0, packetBuffer.Length);
        }

        public List<ParkReviewManager.ParkReviewData> ReceiveParkReviewsResponse()
        {
            List<ParkReviewManager.ParkReviewData> reviews = new List<ParkReviewManager.ParkReviewData>();

            try
            {
                byte[] countBuffer = new byte[4];

                // Read the count of park review objects to expect
                int bytesRead = stream.Read(countBuffer, 0, 4);

                int count = BitConverter.ToInt32(countBuffer, 0);

                // Read each park review object
                for (int i = 0; i < count; i++)
                {
                    byte[] lengthBuffer = new byte[4];

                    // Read the length of the next park review object
                    bytesRead = stream.Read(lengthBuffer, 0, 4);

                    int dataLength = BitConverter.ToInt32(lengthBuffer, 0);
                    byte[] reviewBuffer = new byte[dataLength];

                    // Read the park review data
                    bytesRead = stream.Read(reviewBuffer, 0, dataLength);

                    using (MemoryStream ms = new MemoryStream(reviewBuffer))
                    {
                        // Deserialize the review data
                        var review = Serializer.Deserialize<ParkReviewManager.ParkReviewData>(ms);
                        reviews.Add(review);
                    }
                }
            }
            catch (Exception ex)
            {
                // Errors handling
                Console.WriteLine($"Error receiving park reviews: {ex.Message}");
            }

            return reviews;
        }

        /* Park Data Manager - Individual Park Data */

        /* Park Image Manager - Individual Park Image */


        public void CloseConnection()
        {
            stream.Close();
        }

    }
}
