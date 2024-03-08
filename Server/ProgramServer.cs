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
    static class Constants
    {
        public const string ParkData_FilePath = "../../../Database/ParkData.txt";
        public const string ParkReviews_FilePath = "../../../Database/ParkReview.txt";
        public const string UserDB_FilePath = "../../../Database/UserDB.txt";
        public const string ParkImages_FilePath = "../../../Assets/ParkImages/";
    }

    public class ProgramServer
    {
        private static Logger logger = new Logger("../../../Log/ServerLog.txt");
        public static void Main(string[] args)
        {
            StartServer();
        }

        // Below needs to be restructure into its own class according to the class Diagram -> i.e: Transmission Manager, TCP Connection Manager?

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
                        ProcessPacketType(packet, stream, client);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        // Above needs to be restructure into its own class according to the class Diagram -> i.e: Transmission Manager, TCP Connection Manager?

        /**************************************************************************************************************
         *                                                      Packet                                                *
         * ************************************************************************************************************/

        /*** Receive Packet -> Deserialize into Packet Object ***/
        public static Packet ReceivePacket(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            logger.Log(buffer);

            using (MemoryStream memStream = new MemoryStream(buffer, 0, bytesRead))
            {
                return Serializer.Deserialize<Packet>(memStream);
            }
        }

        /*** Process Packet Type ***/
        public static void ProcessPacketType(Packet packet, NetworkStream stream, TcpClient client)
        {
            // Process packets based on type
            switch (packet.GetPacketHeader().GetType())
            {
                // Login State
                case Types.login:
                    ProcessLoginPacket(packet, stream, client);
                    break;

                // Register State
                case Types.register:
                    ProcessSignUpPacket(packet, stream, client);
                    break;

                // Not sure if we need at all? 
                case Types.log:

                    break;

                // All Park Data State
                case Types.allparkdata:
                    ProcessAllParkDataPacket(stream);
                    break;

                // Individual Park Data State
                case Types.a_park:
                    ProcessOneParkDataPacket(stream, packet);
                    break;

                // All Park Images State
                case Types.allparkimages:
                    //ProcessAllParkImagePacket(stream);
                    break;

                // Individual Park Image State
                case Types.an_image:
                    ProcessOneParkImagePacket(stream, packet);
                    break;

                // All Park Reviews
                case Types.all_reviews:
                    ProcessAllReviewsPacket(stream);
                    break;

                // Individual Park Reviews State
                case Types.review:
                    ProcessParkReviewPacket(stream, packet);
                    break;

                // ADMIN - Request delete a park review
                case Types.delete_review:
                    ProcessDeleteParkReviewPacket(stream, packet);
                    break;

                // ADMIN - Delete a park (Park Data, Park Reviews, Park Image)
                case Types.delete_park:
                    ProcessDeleteAParkPacket(stream, packet);
                    break;

                // ADMIN - Add a Park 
                case Types.add_park:
                    ProcessAddAParkPacket(stream, packet);
                    break;

                // CLIENT - Add a Park Review
                case Types.add_review:
                    ProcessAddAParkReviewPacket(stream, packet);
                    break;

                // ADMIN - Edit a Park Info
                case Types.edit_park:
                    ProcessEditAParkInfoPacket(stream, packet);
                    break;
            }
        }

        /**************************************************************************************************************
         *                                                      Packet                                                *
         * ************************************************************************************************************/


        /**************************************************************************************************************
         *                                              Process Packet Type                                           *
         * ************************************************************************************************************/

        /*** Process Packet Type -> Login ***/
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

        /*** Process Packet Type -> Sign Up ***/
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

        /*** Process Packet Type -> All Park Data ***/
        private static void ProcessAllParkDataPacket(NetworkStream stream)
        {
            ParkDataManager.ParkData[] allParkData = ReadAllParkDataFromFile(Constants.ParkData_FilePath);

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

        /*** Process Packet Type -> Individual Park Data ***/
        private static void ProcessOneParkDataPacket(NetworkStream stream, Packet receivedPacket)
        {
            string parkName = Encoding.UTF8.GetString(receivedPacket.GetBody().buffer);
            ParkData? parkData = ParkDataManager.ReadOneParkDataFromFile(Constants.ParkData_FilePath, parkName);

            // Verify if its not null then we can proceed to process the park data object
            if (parkData != null)
            {
                byte[] parkDataBuffer = parkData.SerializeToByteArray();

                // We convert parkdatbuffer length into byte [] as the first 4 bytes
                byte[] bufferLength = BitConverter.GetBytes(parkDataBuffer.Length);

                // 1. Send buffer length
                stream.Write(bufferLength, 0, bufferLength.Length);

                // 2. Send the data buffer back
                stream.Write(parkDataBuffer, 0, parkDataBuffer.Length); 
            }
            else
            {
                Console.WriteLine($"Park data for {parkName} not found.");
            }
        }


        /*** Process Packet Type -> All Park Image ***/



















        /*** Process Packet Type -> Individual Park Image ***/

        private static void ProcessOneParkImagePacket(NetworkStream stream, Packet receivedPacket)
        {
            string parkName = Encoding.UTF8.GetString(receivedPacket.GetBody().buffer);
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "../../../Assets/ParkImages/" + parkName + ".jpg"); // Only .jpg file will work!!!

            // https://stackoverflow.com/questions/16282933/c-sharp-filestream-reads-bytes-incorrectly
            if (File.Exists(imagePath))
            {
                FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                int chunkSize = 1024 * 1024; // 1 MB sent at a time for large image transfer/stream

                byte[] buffer = new byte[chunkSize];

                int bytesToRead;

                while ((bytesToRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, bytesToRead);
                }
                fileStream.Close();
            } 
            else
            {
                Console.WriteLine($"Image for park {parkName} cannot be found");
            }
        }


        /*** Process Packet Type -> All Park Reviews ***/
        private static void ProcessAllReviewsPacket(NetworkStream stream)
        {

            List<ParkReviewManager.ParkReviewData> allReviews = ParkReviewManager.ParkReviewData.ReadAllParkReviewsFromFile(Constants.ParkReviews_FilePath);

            byte[] reviewCount = BitConverter.GetBytes(allReviews.Count);
            stream.Write(reviewCount, 0, reviewCount.Length);

            // Iterate over all reviews
            for (int i = 0; i < allReviews.Count; i++)
            {
                // Serialize current review to byte array
                byte[] reviewBuffer = allReviews[i].SerializeToByteArray();

                // Convert the length of the review buffer to bytes []
                byte[] reviewBufferLength = BitConverter.GetBytes(reviewBuffer.Length);

                // Send the review buffer length first so the client knows what to expect
                stream.Write(reviewBufferLength, 0, reviewBufferLength.Length);

                // Send the serialized review
                stream.Write(reviewBuffer, 0, reviewBuffer.Length);
            }

        }


        /*** Process Packet Type -> Individual Park Reviews ***/
        private static void ProcessParkReviewPacket(NetworkStream stream, Packet receivedPacket)
        {
            // Deserialize the packet body to get the park name
            string parkName = Encoding.UTF8.GetString(receivedPacket.GetBody().buffer);

            // Read reviews from file
            List<ParkReviewManager.ParkReviewData> allReviews = Server.ParkReviewManager.ParkReviewData.ReadAllParkReviewsFromFile(Constants.ParkReviews_FilePath);

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

        /*** Process Packet Type -> Delete Park Review ***/
        private static void ProcessDeleteParkReviewPacket(NetworkStream stream, Packet receivedPacket)
        {

            // First, we need to deserialize the packet into ParkReviewData object form
            ParkReviewManager.ParkReviewData reviewDataToDelete = new ParkReviewManager.ParkReviewData().deserializeParkReviewData(receivedPacket.GetBody().buffer);

            bool isDeleted = false;

            // Second, we will go through the ParkReviews text file - SAVE it in a list of ParkReviewData object
            List<ParkReviewManager.ParkReviewData> allReviews = ParkReviewManager.ParkReviewData.ReadAllParkReviewsFromFile(Constants.ParkReviews_FilePath);

            // Third we use FirstOrDefault built-in method which essentially return the element that first match the condition (First matching review we would like to delete
            ParkReviewManager.ParkReviewData? reviewToRemove = allReviews.FirstOrDefault(review => review.ParkName == reviewDataToDelete.ParkName &&
                                                         review.UserName == reviewDataToDelete.UserName &&
                                                         review.DateOfPosting == reviewDataToDelete.DateOfPosting &&
                                                         review.Rating == reviewDataToDelete.Rating &&
                                                         review.Review == reviewDataToDelete.Review);

            // Fourth, we would like reassure that we successfully deleted the review from the List before moving on
            if (reviewToRemove != null)
            {
                allReviews.Remove(reviewToRemove);
                isDeleted = true;
            }

            // Fifth, we take the remaining reviews that were not request to be deleted (remaining reviews) and rewrite it back to the textfile
            ParkReviewManager.ParkReviewData.OverwriteAllParkReviewsToFile(Constants.ParkReviews_FilePath, allReviews);


            string message = "Review not found.";

            if (isDeleted)
            {
                message = "Review deleted successfully.";
            }

            // Send an acknowledgement message to the client whether the deletion process was successful
            SendAcknowledgement(stream, message);
        }

        /*** Process Packet Type -> Delete Park Review ***/
        private static void ProcessDeleteAParkPacket(NetworkStream stream, Packet receivedPacket)
        {
            string parkName = Encoding.UTF8.GetString(receivedPacket.GetBody().buffer);

            string parkImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + Constants.ParkImages_FilePath + parkName + ".jpg");
            //string parkDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + Constants.ParkData_FilePath);
            //string parkReviewsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + Constants.ParkReviews_FilePath);

            try
            {
                if (File.Exists(Constants.ParkImages_FilePath))
                {
                    File.Delete($"{AppDomain.CurrentDomain.BaseDirectory + Constants.ParkImages_FilePath + parkName}.jpg");
                }
                else
                {
                    Console.WriteLine($"Unable to delete {parkName}.jpg");
                }

                // Delete park data.
                DeleteParkData(parkName);

                // Delete park reviews.
                DeleteParkReviews(parkName);

                // Send a success message back to the client.
                SendAcknowledgement(stream, $"{parkName} has been deleted -> (park data, park image, park reviews)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                SendAcknowledgement(stream, $"Failed to delete park: {ex.Message}");
            }

        }

        /*** Process Packet Type -> Add a Park ***/
        public static void ProcessAddAParkPacket(NetworkStream stream, Packet receivedPacket)
        {
            try
            {
                ParkDataManager.ParkData parkData = Serializer.Deserialize<ParkDataManager.ParkData>(new MemoryStream(receivedPacket.GetBody().buffer));

                AppendParkDataToFile(Constants.ParkData_FilePath, parkData);

                if (stream.DataAvailable)
                {
                    SaveParkImageToImagesFolder(stream, parkData.parkName);
                }

                SendAcknowledgement(stream, "Park data added successfully.");

            } catch (Exception ex)
            {
                Console.WriteLine("Something went wrong while adding park data /o\\" + ex.ToString());
            }

        }

        /*** Process Packet Type -> Add a Park Review ***/
        public static void ProcessAddAParkReviewPacket(NetworkStream stream, Packet receivedPacket)
        {
            try
            {
                ParkReviewManager.ParkReviewData parkReviewData = Serializer.Deserialize<ParkReviewManager.ParkReviewData>(new MemoryStream( receivedPacket.GetBody().buffer));

                AppendReviewDataToFile(Constants.ParkReviews_FilePath, parkReviewData);

                SendAcknowledgement(stream, "Review added successfully");
            } 
            catch ( Exception ex )
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public static void ProcessEditAParkInfoPacket(NetworkStream stream, Packet receivedPacket)
        {
            try
            {
                ParkDataManager.ParkData parkData = Serializer.Deserialize<ParkDataManager.ParkData>(new MemoryStream(receivedPacket.GetBody().buffer));

                EditAParkDataToFile(Constants.ParkData_FilePath, parkData);

                if (stream.DataAvailable)
                {
                    // We reuse the method we did with add a park image
                    SaveParkImageToImagesFolder(stream, parkData.parkName);
                }

                SendAcknowledgement(stream, $"{parkData.parkName} has been successfully updated");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong while adding park data /o\\" + ex.ToString());
            }
        }


        /**************************************************************************************************************
         *                                    Helper Methods to Process Packet Type                                   *
         * ************************************************************************************************************/

        private static string PerformLogin(UserDataManager.LoginData loginData)
        {
            Console.WriteLine($"Username: {loginData.GetUserName()}");
            Console.WriteLine($"Password: {loginData.GetPassword()}");

            Login login = new Login(loginData);
            return login.LoginUser(Constants.UserDB_FilePath);
        }

        private static string PerformSignUp(UserDataManager.SignUpData signUpData)
        {
            Console.WriteLine($"Username:  {signUpData.GetUserName()}");
            Console.WriteLine($"Password:  {signUpData.GetPassword()}");

            SignUp signUp = new SignUp(signUpData);
            return signUp.SignUpUser(Constants.UserDB_FilePath);
        }


        // Below may not be needed for all the packet types

        /*** Helper Method for -> Individual Park Data ***/













        /*** Helper Method for -> All Park Images ***/












        /*** Helper Method for -> Individual Park Image ***/









        private static void DeleteParkData(string parkName)
        {
            // https://stackoverflow.com/questions/29975219/reading-lines-from-text-file-and-add-to-liststring

            // Read all the lines from the file and store it in the memory as list
            List<string> lines = File.ReadAllLines(Constants.ParkData_FilePath).ToList();

            // We then will use this to hold all the new lines that arent supposed to be deleted
            List<string> remainLines = new List<string>();

            // Iterate through each line looking for matching park name
            for (int i = 0; i < lines.Count; i++)
            {
                // When a park name match is found...
                if (lines[i].Trim().Equals(parkName))
                {
                    // We wanna skip the next 3 lines
                    i = i + 3;
                }
                else
                {
                    // If the current line is not part of the park to delete, we add them back to a line to be written back to our file
                    remainLines.Add(lines[i]);
                }
            }

            // Rewrite the updated park data back to the file
            File.WriteAllLines(Constants.ParkData_FilePath, remainLines.ToArray());

            Console.WriteLine($"{parkName} data deleted successfully.");
        }


        private static void DeleteParkReviews(string parkNameToDelete)
        {
            // https://stackoverflow.com/questions/29975219/reading-lines-from-text-file-and-add-to-liststring

            List<string> updatedContent = new List<string>();
            bool isThisReviewATargetPark = false; 

            // Read all the lines from the file and store it in the memory as list
            string[] allLines = File.ReadAllLines(Constants.ParkReviews_FilePath);
            for (int i = 0; i < allLines.Length; i++)
            {
                string line = allLines[i];

                // Check if the line is the start of a new review block
                if (line.StartsWith("ParkName: "))
                {
                    // Check if the current review block is for the park we want to delete
                    isThisReviewATargetPark = line.Contains(parkNameToDelete);
                }

                // If the current line is not part of the park to delete, we add them back to a line to be written back to our file
                if (!isThisReviewATargetPark)
                {
                    updatedContent.Add(line);
                }

                // If the line is empty, we know that it should be the end of the file then
                if (string.IsNullOrWhiteSpace(line))
                {
                    isThisReviewATargetPark = false;
                }
            }

            // Write updated file without the file we park we deleted
            File.WriteAllLines(Constants.ParkReviews_FilePath, updatedContent);
        }

        private static void SaveParkImageToImagesFolder(NetworkStream stream, string parkName)
        {
            string imagePath = Path.Combine(Constants.ParkImages_FilePath, $"{parkName}.jpg");

            // https://learn.microsoft.com/en-us/dotnet/api/system.io.filestream.read?view=net-8.0
            // Create a new file

            using (FileStream fileStream = new FileStream(imagePath, FileMode.Create))
            {
                // This is the size of incoming chunk
                byte[] sizeBuffer = new byte[4]; 

                int bytesToRead;

                // Keep reading til next chunk becomes 0
                while (true)
                {
                    // Read the size of the next chunk
                    bytesToRead = stream.Read(sizeBuffer, 0, sizeBuffer.Length);


                    if (bytesToRead == 0)
                    {
                        throw new Exception("Stream ended prematurely, something went wrong!!!");
                    }

                    // convert byte[] of the size buffet to integer value to know the next incoming chunk size
                    int nextChunkSize = BitConverter.ToInt32(sizeBuffer, 0);

                    if (nextChunkSize == 0)
                    {
                        return;
                    }

                    // We will use this to store buffer based on the incoming chunk size
                    byte[] buffer = new byte[nextChunkSize];

                    // Read the chunk data
                    int totalBytesToRead = 0;

                    // Keep receving it as long the numbe rof total bytes to read is more than incoming chunk
                    while (totalBytesToRead < nextChunkSize)
                    {
                        int readStream = stream.Read(buffer, totalBytesToRead, nextChunkSize - totalBytesToRead);
                        if (readStream == 0)
                        {
                            // This is end of the stream but something went wrong
                            throw new Exception("Stream ended prematurely, something went wrong!!!.");
                        }
                        totalBytesToRead += readStream;
                    }

                    // If we keep writing chunk of buffer to the file chunk at a time
                    fileStream.Write(buffer, 0, totalBytesToRead);
                }
            }
        }

        private static void AppendParkDataToFile(string filePath, ParkDataManager.ParkData parkData)
        {
            try
            {
                StringBuilder parkDataBuffer = new StringBuilder();
                parkDataBuffer.AppendLine(parkData.parkName);
                parkDataBuffer.AppendLine(parkData.parkAddress);
                parkDataBuffer.AppendLine(parkData.parkDescription);
                parkDataBuffer.Append(parkData.parkHours);

                File.AppendAllText(filePath, parkDataBuffer.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while attempt to append park data to text file!!! " + e.ToString());
            }

        }


        private static void AppendReviewDataToFile(string filePath, ParkReviewManager.ParkReviewData parkReviewData)
        {
            try
            {
                // https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1
                // Convert DateTime Format
                StringBuilder reviewDataBuffer = new StringBuilder();
                reviewDataBuffer.AppendLine($"ParkName: {parkReviewData.ParkName}");
                reviewDataBuffer.AppendLine($"Username: {parkReviewData.UserName} | Park Rating: {parkReviewData.Rating} | DateOfPosting: {parkReviewData.DateOfPosting.ToString("MM/dd/yyyy hh:mm tt")} | Review: {parkReviewData.Review}");

                File.AppendAllText( filePath, reviewDataBuffer.ToString() );
            } catch ( Exception e )
            {
                Console.WriteLine( e.ToString() );
            }

        }

        private static void EditAParkDataToFile(string filePath, ParkDataManager.ParkData updatedParkData)
        {
            try
            {

                ParkDataManager.ParkData[] parks = ReadAllParkDataFromFile(filePath);

                // Find the park to update and modify its information
                for (int i = 0; i < parks.Length; i++)
                {
                    if (parks[i].parkName.Equals(updatedParkData.parkName))
                    {
                        parks[i].parkAddress = updatedParkData.parkAddress;
                        parks[i].parkDescription = updatedParkData.parkDescription;
                        parks[i].parkHours = updatedParkData.parkHours;
                        return;
                    }
                }

                // Rewrite the file with updated parks data
                using (StreamWriter file = new StreamWriter(filePath))
                {
                    foreach (ParkDataManager.ParkData park in parks)
                    {
                        file.WriteLine(park.parkName);
                        file.WriteLine(park.parkAddress);
                        file.WriteLine(park.parkDescription);
                        file.WriteLine(park.parkHours);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while trying to update the park data: {ex.Message}");
            }
        }







        /**************************************************************************************************************
         *                                    Helper Methods to Process Packet Type                                   *
         * ************************************************************************************************************/


        /**************************************************************************************************************
         *                                        Acknowledge Message to Client                                       *
         * ************************************************************************************************************/

        public static void SendAcknowledgement(NetworkStream stream, string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            Console.WriteLine($"Acknowledgement: {message}");
            stream.Write(messageBytes, 0, messageBytes.Length);
            stream.Flush();
        }

        /**************************************************************************************************************
         *                                        Acknowledge Message to Client                                       *
         * ************************************************************************************************************/

    }
}
