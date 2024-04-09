using ProtoBuf;
using Server.DataStructure;
using Server.Interfaces;
using Server.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Server.DataStructure.PacketData;

namespace Server.Implementations
{
    public class PacketProcessor
    {
        private UserDataManager _userDataManager;
        private ParkDataManager _parkDataManager;
        private ParkReviewManager _parkReviewManager;
        private ServerStateManager _serverStateManager;
        //private Logger _logger;
        private ImageManager _imageManager;
        private static ServerState currentState;

        private Action _stopServerCallback;

        public PacketProcessor(UserDataManager userDataManager, ParkDataManager parkDataManager, ParkReviewManager parkReviewManager, ImageManager imageManager, ServerStateManager serverStateManager)
        {
            _userDataManager = userDataManager;
            _parkDataManager = parkDataManager;
            _parkReviewManager = parkReviewManager;
            _imageManager = imageManager;
            _serverStateManager = serverStateManager;
        }

        public void ProcessPacket(Packet packet, ICommunicationChannel stream, TcpClient client)
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
                case Types.login_admin:
                    ProcessLoginAdminPacket(packet, stream, client);
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
                    ProcessAllParkImagesPacket(stream);
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
                    ProcessDeleteAParkPacket(stream, packet, Get_parkReviewManager());
                    break;

                // ADMIN - Add a Park -- Stopped here 
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

        public void ProcessLoginPacket(Packet packet, ICommunicationChannel stream, TcpClient client)
        {
            _serverStateManager.SetCurrentState(ServerState.Login);

            byte[] buffer = packet.GetBody().buffer;
            if (buffer != null && buffer.Length > 0)
            {
                UserDataManager.LoginData loginData = new UserDataManager.LoginData();
                loginData = loginData.deserializeLoginData(buffer);

                string message = _userDataManager.PerformLogin(loginData);
                SendAcknowledgement(stream, message);

                if (message == "Username and password are Correct!!! \\o/")
                {
                    _serverStateManager.SetCurrentState(ServerState.Idle);
                }
            }
            else
            {
                Console.WriteLine("Packet bodyBuffer is empty or null");
                stream.Close();
                client.Close();
            }
        }

        /*** Process Packet Type -> Admin Login ***/
        private void ProcessLoginAdminPacket(Packet packet, ICommunicationChannel stream, TcpClient client)
        {
            _serverStateManager.SetCurrentState(ServerState.AdminLogin);

            byte[] buffer = packet.GetBody().buffer;
            if (buffer != null && buffer.Length > 0)
            {
                UserDataManager.LoginData loginData = new UserDataManager.LoginData();
                loginData = loginData.deserializeLoginData(buffer);

                string message = _userDataManager.PerformAdminLogin(loginData);
                SendAcknowledgement(stream, message);

                if (message == "Username and password are Correct!!! \\o/")
                {
                    _serverStateManager.SetCurrentState(ServerState.Idle);
                }


            }
            else
            {
                Console.WriteLine("Packet bodyBuffer is empty or null");
                stream.Close();
                client.Close();
            }
        }

        /*** Process Packet Type -> Sign Up ***/
        private void ProcessSignUpPacket(Packet packet, ICommunicationChannel stream, TcpClient client)
        {
            _serverStateManager.SetCurrentState(ServerState.SignUp);

            byte[] buffer = packet.GetBody().buffer;
            if (buffer != null && buffer.Length > 0)
            {
                UserDataManager.SignUpData signUpData = new UserDataManager.SignUpData();
                signUpData = signUpData.deserializeSignUpData(buffer);

                string message = _userDataManager.PerformSignUp(signUpData);
                SendAcknowledgement(stream, message);

                if (message == "Successfully Sign Up \\o/")
                {
                    _serverStateManager.SetCurrentState(ServerState.Idle);
                }
            }
            else
            {
                Console.WriteLine("Packet bodyBuffer is empty or null");
                stream.Close();
                //client.Close();
            }
        }

        /*** Process Packet Type -> All Park Data ***/
        private void ProcessAllParkDataPacket(ICommunicationChannel stream)
        {

            _serverStateManager.SetCurrentState(ServerState.AllParkData);

            ParkData[] allParkData = _parkDataManager.ReadAllParkDataFromFile(Constants.ParkData_FilePath);

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

            _serverStateManager.SetCurrentState(ServerState.Idle);
        }

        /*** Process Packet Type -> Individual Park Data ***/
        private void ProcessOneParkDataPacket(ICommunicationChannel stream, Packet receivedPacket)
        {
            _serverStateManager.SetCurrentState(ServerState.OneParkData);

            string parkName = Encoding.UTF8.GetString(receivedPacket.GetBody().buffer);
            ParkData? parkData = _parkDataManager.ReadOneParkDataFromFile(Constants.ParkData_FilePath, parkName);

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

            _serverStateManager.SetCurrentState(ServerState.Idle);
        }

        static List<string> GetImages(string imgdir)
        {
            List<string> images = new List<string>();

            // Iterate through the folder
            foreach (string imagePath in Directory.GetFiles(imgdir))
            {
                if (imagePath.IndexOf(".jpg") == -1)
                {
                    continue;
                }

                string imageName = Path.GetFileNameWithoutExtension(imagePath);
                
                // Add it to the list of images
                images.Add(imageName);
            }

            return images;
        }


        /*** Process Packet Type -> All Park Image ***/
        private void ProcessAllParkImagesPacket(ICommunicationChannel stream)
        {
            _serverStateManager.SetCurrentState(ServerState.AllParkImages);

            // Get the image path
            string imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "../../../Assets/ParkImages/");

            // Get list of images
            List<string> images = GetImages(imageFolder);

            string buffer = "";
            foreach (string imageName in images)
            {
                buffer += imageName;
                buffer += "|";
            }

            // Need to be splitted to 1MB chunk

            // Send the images name list
            byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(buffer);
            stream.WriteAsync(nameBytes, 0, nameBytes.Length);

        }


















        /*** Process Packet Type -> Individual Park Image ***/

        private void ProcessOneParkImagePacket(ICommunicationChannel stream, Packet receivedPacket)
        {
            _serverStateManager.SetCurrentState(ServerState.OneParkImage);

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

            _serverStateManager.SetCurrentState(ServerState.Idle);
        }


        /*** Process Packet Type -> All Park Reviews ***/
        private void ProcessAllReviewsPacket(ICommunicationChannel stream)
        {

            _serverStateManager.SetCurrentState(ServerState.AllReviews);

            List<ParkReviewData> allReviews = _parkReviewManager.ReadAllParkReviewsFromFile(Constants.ParkReviews_FilePath);

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

            _serverStateManager.SetCurrentState(ServerState.Idle);

        }


        /*** Process Packet Type -> Individual Park Reviews ***/
        private void ProcessParkReviewPacket(ICommunicationChannel stream, Packet receivedPacket)
        {
            _serverStateManager.SetCurrentState(ServerState.ParkReview);
            // Deserialize the packet body to get the park name
            string parkName = Encoding.UTF8.GetString(receivedPacket.GetBody().buffer);

            // Read reviews from file
            List<ParkReviewData> allReviews = _parkReviewManager.ReadAllParkReviewsFromFile(Constants.ParkReviews_FilePath);

            // Filter reviews for the requested park
            // .Where is borrowed from the LINQ similar to how we did on MySQL => as long as the review contains the matching parkName -> then we will add it to the list to be stream back to the client
            List<ParkReviewData> matchingReviews = allReviews.Where(review => review.ParkName.Equals(parkName)).ToList();

            // Preping to send the number of matching reviews first -> let the client know a head of time how many matching reviews 
            byte[] reviewCount = BitConverter.GetBytes(matchingReviews.Count);
            stream.Write(reviewCount, 0, reviewCount.Length);

            // Serialize and send each matching review
            for (int i = 0; i < matchingReviews.Count; i++)
            {
                ParkReviewData review = matchingReviews[i]; // Get the review at the current index
                byte[] reviewBuffer = review.SerializeToByteArray();

                // Send the length of the review buffer first
                byte[] reviewBufferLength = BitConverter.GetBytes(reviewBuffer.Length);
                stream.Write(reviewBufferLength, 0, reviewBufferLength.Length);

                // Now send the review buffer itself
                stream.Write(reviewBuffer, 0, reviewBuffer.Length);
            }
            _serverStateManager.SetCurrentState(ServerState.Idle);
        }

        /*** Process Packet Type -> Delete Park Review ***/
        private void ProcessDeleteParkReviewPacket(ICommunicationChannel stream, Packet receivedPacket)
        {

            _serverStateManager.SetCurrentState(ServerState.DeleteParkReview);

            // First, we need to deserialize the packet into ParkReviewData object form
            ParkReviewData reviewDataToDelete = new ParkReviewData().deserializeParkReviewData(receivedPacket.GetBody().buffer);

            bool isDeleted = false;

            // Second, we will go through the ParkReviews text file - SAVE it in a list of ParkReviewData object
            List<ParkReviewData> allReviews = _parkReviewManager.ReadAllParkReviewsFromFile(Constants.ParkReviews_FilePath);

            // Third we use FirstOrDefault built-in method which essentially return the element that first match the condition (First matching review we would like to delete
            ParkReviewData? reviewToRemove = allReviews.FirstOrDefault(review => review.ParkName == reviewDataToDelete.ParkName &&
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
            _parkReviewManager.OverwriteAllParkReviewsToFile(Constants.ParkReviews_FilePath, allReviews);


            string message = "Review not found.";

            if (isDeleted)
            {
                message = "Review deleted successfully.";
            }

            // Send an acknowledgement message to the client whether the deletion process was successful
            SendAcknowledgement(stream, message);

            _serverStateManager.SetCurrentState(ServerState.Idle);
        }

        private ParkReviewManager Get_parkReviewManager()
        {
            return _parkReviewManager;
        }

        /*** Process Packet Type -> Delete Park Review ***/
        public void ProcessDeleteAParkPacket(ICommunicationChannel stream, Packet receivedPacket, ParkReviewManager _parkReviewManager)
        {

            _serverStateManager.SetCurrentState(ServerState.DeleteAPark);
            string parkName = Encoding.UTF8.GetString(receivedPacket.GetBody().buffer);

            string parkImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + Constants.ParkImages_FilePath + parkName + ".jpg");
            //string parkDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + Constants.ParkData_FilePath);
            //string parkReviewsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + Constants.ParkReviews_FilePath);

            try
            {
                if (File.Exists(parkImagePath))
                {
                    File.Delete($"{AppDomain.CurrentDomain.BaseDirectory + Constants.ParkImages_FilePath + parkName}.jpg");
                }
                else
                {
                    Console.WriteLine($"Unable to delete {parkName}.jpg");
                }

                // Delete park data.
                _parkDataManager.DeleteParkData(parkName);
                _parkReviewManager.DeleteParkReviews(parkName, Constants.ParkReviews_FilePath);

                // Send a success message back to the client.
                SendAcknowledgement(stream, $"{parkName} has been deleted -> (park data, park image, park reviews)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                SendAcknowledgement(stream, $"Failed to delete park: {ex.Message}");
            }

            _serverStateManager.SetCurrentState(ServerState.Idle);

        }

        /*** Process Packet Type -> Add a Park ***/
        public void ProcessAddAParkPacket(ICommunicationChannel stream, Packet receivedPacket)
        {
            _serverStateManager.SetCurrentState(ServerState.AddAPark);

            try
            {
                ParkData parkData = Serializer.Deserialize<ParkData>(new MemoryStream(receivedPacket.GetBody().buffer));

                _parkDataManager.AppendParkDataToFile(Constants.ParkData_FilePath, parkData);

                if (stream.DataAvailable)
                {
                    _imageManager.SaveParkImageToImagesFolder(stream, parkData.parkName);
                }

                SendAcknowledgement(stream, "Park data added successfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong while adding park data /o\\" + ex.ToString());
            }

            _serverStateManager.SetCurrentState(ServerState.Idle);

        }

        /*** Process Packet Type -> Add a Park Review ***/
        public void ProcessAddAParkReviewPacket(ICommunicationChannel stream, Packet receivedPacket)
        {

            _serverStateManager.SetCurrentState(ServerState.AddAParkReview);

            try
            {
                ParkReviewData parkReviewData = Serializer.Deserialize<ParkReviewData>(new MemoryStream(receivedPacket.GetBody().buffer));

                _parkReviewManager.AppendReviewDataToFile(Constants.ParkReviews_FilePath, parkReviewData);

                SendAcknowledgement(stream, "Review added successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            _serverStateManager.SetCurrentState(ServerState.Idle);

        }

        public void ProcessEditAParkInfoPacket(ICommunicationChannel stream, Packet receivedPacket)
        {
            _serverStateManager.SetCurrentState(ServerState.EditAParkInfo);

            try
            {
                ParkData parkData = Serializer.Deserialize<ParkData>(new MemoryStream(receivedPacket.GetBody().buffer));

                _parkDataManager.EditAParkDataToFile(Constants.ParkData_FilePath, parkData);

                if (stream.DataAvailable)
                {
                    // We reuse the method we did with add a park image
                    _imageManager.SaveParkImageToImagesFolder(stream, parkData.parkName);
                }

                SendAcknowledgement(stream, $"{parkData.parkName} has been successfully updated");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong while adding park data /o\\" + ex.ToString());
            }

            _serverStateManager.SetCurrentState(ServerState.Idle);
        }



        /**************************************************************************************************************
         *                                        Acknowledge Message to Client                                       *
         * ************************************************************************************************************/

        public void SendAcknowledgement(ICommunicationChannel stream, string message)
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
