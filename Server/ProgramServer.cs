using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ProtoBuf;
using static Server.UserDataManager;
using static Server.ParkDataManager;

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
                    ProcessSignUpPacket(packet, stream, client);
                    break;

                // Send image?
                case Types.send:

                    break;

                // Get individual park data
                case Types.park:

                    break;

                // Get individual park image
                case Types.image:

                    break;

                // Get individual park reviews
                case Types.review:
                    ProcessParkReviewRequest(stream, packet);
                    break;

                // For retrieving all park data
                case Types.allparkdata:
                    ProcessParkDataRetrievalAll(stream);
                    break;

                // For retrieving all park images
                case Types.allparkimages:
                    //ProcessImageRequest(stream);
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




        private static void ProcessSignUpPacket(Packet packet, NetworkStream stream, TcpClient client)
        {
            byte[] buffer = packet.GetBody().buffer;
            if (buffer != null && buffer.Length > 0)
            {
                UserDataManager.SignUpData signUpData = new UserDataManager.SignUpData();
                signUpData = signUpData.deserializeSignUpData(buffer);

                string message = PerformSignUp(signUpData);
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

        private static string PerformSignUp(UserDataManager.SignUpData signUpData)
        {
            Console.WriteLine($"Username:  {signUpData.GetUserName()}");
            Console.WriteLine($"Password:  {signUpData.GetPassword()}");

            SignUp signUp = new SignUp(signUpData);
            return signUp.SignUpUser("../../../UserDB.txt");
        }

        // still has yet to implement park data retrival because of issue with client connection loss 
        private static void ProcessParkDataRetrievalAll(NetworkStream stream)
        {
            ParkDataManager.ParkData[] allParkData = ReadAllParkDataFromFile("../../../ParkData.txt");

            // Sending the number of ParkData objects first 
            byte[] numberOfBytes = BitConverter.GetBytes(allParkData.Length);
            stream.Write(numberOfBytes, 0, numberOfBytes.Length);

            for (int i = 0; i < allParkData.Length; i++)
            {
                // Convert all park data into byte array
                byte[] buffer = allParkData[i].SerializeToByteArray();

                byte[] bufferLength = BitConverter.GetBytes(buffer.Length);
                stream.Write(bufferLength, 0, bufferLength.Length);

                stream.Write(buffer, 0, buffer.Length);
            }
            Console.WriteLine("All park data sent to client");
        }

        /* Send Image File */

        /* Get individual park reviews */
        private static void ProcessParkReviewRequest(NetworkStream stream, Packet receivedPacket)
        {
            // Deserialize the packet body to get the park name
            string parkName = Encoding.UTF8.GetString(receivedPacket.GetBody().buffer);

            // Read reviews from file
            List<ParkReviewManager.ParkReviewData> allReviews = Server.ParkReviewManager.ParkReviewData.ReadAllParkReviewsFromFile("../../../ParkReview.txt");

            // Filter reviews for the requested park
            // .Where is borrowed from the LINQ similar to how we did on MySQL => as long as the review contains the matching parkName -> then we will add it to the list to be stream back to the client
            List<ParkReviewManager.ParkReviewData> matchingReviews = allReviews.Where(review => review.ParkName.Equals(parkName)).ToList();

            // Preping to send the number of matching reviews first -> let the client know a head of time how many matching reviews 
            byte[] reviewCount = BitConverter.GetBytes(matchingReviews.Count);
            stream.Write(reviewCount, 0, reviewCount.Length);

            // Serialize and send each matching review
            for (int i = 0; i < matchingReviews.Count; i++)
            {
                ParkReviewManager.ParkReviewData review = matchingReviews[i]; // Get the review at the current index
                byte[] reviewBuffer = review.SerializeToByteArray();

                // Send the length of the review buffer first
                byte[] reviewBufferLength = BitConverter.GetBytes(reviewBuffer.Length);
                stream.Write(reviewBufferLength, 0, reviewBufferLength.Length);

                // Now send the review buffer itself
                stream.Write(reviewBuffer, 0, reviewBuffer.Length);
            }
        }


    }
}
