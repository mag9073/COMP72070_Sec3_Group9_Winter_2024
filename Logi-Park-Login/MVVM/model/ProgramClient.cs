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
using System.Windows.Threading;

namespace LogiPark.MVVM.Model
{
    public class ProgramClient
    {

        /*** Member attributes of Program Client Class ***/
        private UserDataManager.SignUpData clientSignUpData = new UserDataManager.SignUpData();
        private UserDataManager.LoginData clientLoginData = new UserDataManager.LoginData();
        private ParkDataManager.ParkData clientParkData = new ParkDataManager.ParkData();
        private NetworkStream stream;
        private TcpConnectionManager connectionManager;

        public ProgramClient()
        {
            connectionManager = TcpConnectionManager.Instance;
            stream = connectionManager.stream;
        }

        /**************************************************************************************************************
         *                                             User Data Manager                                              *
         * ************************************************************************************************************/

        /*** Send Request for - Login ***/
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

        /*** Send Request for - Sign Up ***/
        public void SendSignUpRequest(UserDataManager.SignUpData signUpData)
        {
            this.clientSignUpData = signUpData;

            Packet sendPacket = new Packet();
            sendPacket.SetPacketHead(1, 2, Types.register);

            byte[] signUpDataBuffer = clientSignUpData.SerializeToByteArray();
            sendPacket.SetPacketBody(signUpDataBuffer, (uint)signUpDataBuffer.Length);

            byte[] packetBuffer = sendPacket.SerializeToByteArray();
            stream.Write(packetBuffer, 0, packetBuffer.Length);
        }

        /**************************************************************************************************************
         *                                             User Data Manager                                              *
         * ************************************************************************************************************/


        /**************************************************************************************************************
         *                                             Park Data Manager                                              *
         * ************************************************************************************************************/

        /*** Send Request for -> All Park Data ***/
        public void SendAllParkDataRequest()
        {
            Packet sendPacket = new Packet();
            sendPacket.SetPacketHead(1, 2, Types.allparkdata);

            // We dont need to send body in this request 
            byte[] packetBuffer = sendPacket.SerializeToByteArray();
            stream.Write(packetBuffer, 0, packetBuffer.Length);
        }

        /*** Send Request for -> a Specific Park Data ***/
        public void SendOneParkDataRequest(string parkname)
        {
            Packet sendPacket = new Packet();
            sendPacket.SetPacketHead(1, 2, Types.a_park);

            // convert string to bytes array
            byte[] parknameBuffer = Encoding.UTF8.GetBytes(parkname);
            sendPacket.SetPacketBody(parknameBuffer, (uint)parknameBuffer.Length);

            byte[] packetBuffer = sendPacket.SerializeToByteArray();
            stream.Write(packetBuffer, 0, packetBuffer.Length);

            Console.WriteLine("One Park Data sent from client");
        }

        /**************************************************************************************************************
         *                                             Park Data Manager                                              *
         * ************************************************************************************************************/


        /**************************************************************************************************************
         *                                             Image Manager                                                  *
         * ************************************************************************************************************/

        /*** Send Request for -> All Park Images ***/
        public void SendAllParkImagesRequest()
        {
            Packet sendPacket = new Packet();
            sendPacket.SetPacketHead(1, 2, Types.allparkimages);

            // We dont need to send body in this request 
            byte[] packetBuffer = sendPacket.SerializeToByteArray();
            stream.Write(packetBuffer, 0, packetBuffer.Length);
        }

        /*** Send Request for -> a Specific Park Image ***/
        public void SendOneParkImageRequest(string parkname)
        {
            Packet sendPacket = new Packet();
            sendPacket.SetPacketHead(1, 2, Types.an_image);

            // convert string to bytes array
            byte[] parknameBuffer = Encoding.UTF8.GetBytes(parkname);
            sendPacket.SetPacketBody(parknameBuffer, (uint) parkname.Length);

            byte[] packetBuffer = sendPacket.SerializeToByteArray();
            stream.Write(packetBuffer, 0, packetBuffer.Length);
        }

        /**************************************************************************************************************
         *                                             Image Manager                                                  *
         * ************************************************************************************************************/


        /**************************************************************************************************************
         *                                             Park Review Manager                                            *
         * ************************************************************************************************************/

        /*** Send Request for - Individual Park Reviews */
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

        /**************************************************************************************************************
         *                                             Park Review Manager                                            *
         * ************************************************************************************************************/


        /**************************************************************************************************************
         *                                             Universal Server Response                                      *
         * ************************************************************************************************************/

        /*** Receive from Server -> Response ***/
        public string ReceiveServerResponse()
        {
            byte[] responseBuffer = new byte[1024];
            int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
            return Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
        }

        /**************************************************************************************************************
         *                                             Universal Server Response                                      *
         * ************************************************************************************************************/


        /**************************************************************************************************************
         *                                                 Park Data Manager                                          *
         * ************************************************************************************************************/

        /*** Receive from Server -> All Park Data  ***/
        public ParkDataManager.ParkData[] ReceiveAllParkDataResponse()
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

        /**************************************************************************************************************
         *                                                Park Data Manager                                           *
         * ************************************************************************************************************/


        /**************************************************************************************************************
         *                                                Park Review Manager                                         *
         * ************************************************************************************************************/

        /*** Receive from Server -> Park Reviews  ***/
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

        /**************************************************************************************************************
         *                                                Park Review Manager                                         *
         * ************************************************************************************************************/


        /**************************************************************************************************************
         *                                                 Park Data Manager                                          *
         * ************************************************************************************************************/

        /*** Receive from Server -> Individual Park Data ***/

        public ParkDataManager.ParkData ReceiveOneParkDataResponse()
        {
            byte[] lengthBuffer = new byte[4];

            // 1. Get the buffer length from the server - the first 4 bytes
            int bytesRead = stream.Read(lengthBuffer, 0, 4);

            if (bytesRead != 4)
            { 
                throw new Exception("Failed to read park data length.");
            } 

            int dataLength = BitConverter.ToInt32(lengthBuffer, 0);
            byte[] parkDataBuffer = new byte[dataLength];

            // 2. Get the park data buffer from the server
            bytesRead = stream.Read(parkDataBuffer, 0, dataLength);
            if (bytesRead != dataLength)
            {
                throw new Exception("Failed to read complete park data.");
            }

            // We deserialize the stream data we got back from the server into Park Data object
            using (MemoryStream ms = new MemoryStream(parkDataBuffer))
            {
                return Serializer.Deserialize<ParkDataManager.ParkData>(ms);
            }
        }








        /**************************************************************************************************************
         *                                                 Park Data Manager                                          *
         * ************************************************************************************************************/


        /**************************************************************************************************************
         *                                                 Park Image Manager                                         *
         * ************************************************************************************************************/

        /*** Receive from Server -> All Park Image ***/





        /*** Receive from Server -> Individual Park Image ***/
        public BitmapImage ReceiveOneParkImageResponse()
        {
            MemoryStream imageStream = new MemoryStream();

            int chunkSize = 1024 * 1024; // 1 MB sent at a time for large image transfer/stream

            byte[] buffer = new byte[chunkSize];

            int bytesToRead = 0;

            // We receive stream of byte [] in chunk of 1 MB at a time -> will read til there is nothing left 
            do
            {
                bytesToRead = stream.Read(buffer, 0, buffer.Length);
                imageStream.Write(buffer, 0, bytesToRead);
            } while (bytesToRead == buffer.Length);

            // https://www.codeproject.com/Questions/648495/Convert-byte-to-BitmapImage-in-WPF-application-usi
            imageStream.Position = 0; 
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = imageStream;
            image.EndInit();
            image.Freeze();

            return image;
        }



        /**************************************************************************************************************
         *                                                 Park Image Manager                                         *
         * ************************************************************************************************************/


        // Helper function to close streaming connection
        public void CloseConnection()
        {
            stream.Close();
        }

    }
}
