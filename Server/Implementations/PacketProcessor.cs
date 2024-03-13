using ProtoBuf;
using Server.DataStructure;
using Server.Interfaces;
using Server.Utilities;
using System;
using System.Collections.Generic;
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
        //private Logger _logger;
        private ImageManager _imageManager;
        private static ServerState currentState;

        private Action _stopServerCallback;

        public PacketProcessor(UserDataManager userDataManager, ParkDataManager parkDataManager, ParkReviewManager parkReviewManager, ImageManager imageManager)
        {
            _userDataManager = userDataManager;
            _parkDataManager = parkDataManager;
            _parkReviewManager = parkReviewManager;
            _imageManager = imageManager;
        }

        public void ProcessPacket(Packet packet, ICommunicationChannel channel, TcpClient client)
        {
            // Process packets based on type
            switch (packet.GetPacketHeader().GetType())
            {
                // Login State
                case Types.login:
                    ProcessLoginPacket(packet, channel, client);
                    break;

                // Register State
                case Types.register:
                    ProcessSignUpPacket(packet, channel, client);
                    break;

                // Not sure if we need at all? 
                case Types.login_admin:
                    ProcessLoginAdminPacket(packet, channel, client);
                    break;

                // All Park Data State
                case Types.allparkdata:
                    ProcessAllParkDataPacket(channel);
                    break;

                // Individual Park Data State
                case Types.a_park:
                    ProcessOneParkDataPacket(channel, packet);
                    break;

                // All Park Images State
                case Types.allparkimages:
                    //ProcessAllParkImagePacket(stream);
                    break;

                // Individual Park Image State
                case Types.an_image:
                    ProcessOneParkImagePacket(channel, packet);
                    break;

                // All Park Reviews
                case Types.all_reviews:
                    ProcessAllReviewsPacket(channel);
                    break;

                // Individual Park Reviews State
                case Types.review:
                    ProcessParkReviewPacket(channel, packet);
                    break;

                // ADMIN - Request delete a park review
                case Types.delete_review:
                    ProcessDeleteParkReviewPacket(channel, packet);
                    break;

                // ADMIN - Delete a park (Park Data, Park Reviews, Park Image)
                case Types.delete_park:
                    ProcessDeleteAParkPacket(channel, packet, Get_parkReviewManager());
                    break;

                // ADMIN - Add a Park -- Stopped here 
                case Types.add_park:
                    ProcessAddAParkPacket(channel, packet);
                    break;

                // CLIENT - Add a Park Review
                case Types.add_review:
                    ProcessAddAParkReviewPacket(channel, packet);
                    break;

                // ADMIN - Edit a Park Info
                case Types.edit_park:
                    ProcessEditAParkInfoPacket(channel, packet);
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
        public void ProcessLoginPacket(Packet packet, ICommunicationChannel stream, TcpClient client)
        {
            byte[] buffer = packet.GetBody().buffer;
            if (buffer != null && buffer.Length > 0)
            {
                UserDataManager.LoginData loginData = new UserDataManager.LoginData();
                loginData = loginData.deserializeLoginData(buffer);

                string message = _userDataManager.PerformLogin(loginData);
                SendAcknowledgement(stream, message);

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
            byte[] buffer = packet.GetBody().buffer;
            if (buffer != null && buffer.Length > 0)
            {
                UserDataManager.LoginData loginData = new UserDataManager.LoginData();
                loginData = loginData.deserializeLoginData(buffer);

                string message = _userDataManager.PerformAdminLogin(loginData);
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
        private void ProcessSignUpPacket(Packet packet, ICommunicationChannel stream, TcpClient client)
        {
            byte[] buffer = packet.GetBody().buffer;
            if (buffer != null && buffer.Length > 0)
            {
                UserDataManager.SignUpData signUpData = new UserDataManager.SignUpData();
                signUpData = signUpData.deserializeSignUpData(buffer);

                string message = _userDataManager.PerformSignUp(signUpData);
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
        private void ProcessAllParkDataPacket(ICommunicationChannel stream)
        {
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
        }

        /*** Process Packet Type -> Individual Park Data ***/
        private void ProcessOneParkDataPacket(ICommunicationChannel stream, Packet receivedPacket)
        {
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
        }


        /*** Process Packet Type -> All Park Image ***/
        private static void ProcessAllParkImagesPacket(NetworkStream stream)
        {

            // Count how many images are in the Assets/ParkImages/ folder

            // Based on that create a while loop

            // Within the loop -> 



        }


















        /*** Process Packet Type -> Individual Park Image ***/

        private static void ProcessOneParkImagePacket(ICommunicationChannel stream, Packet receivedPacket)
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
        private void ProcessAllReviewsPacket(ICommunicationChannel stream)
        {

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

        }


        /*** Process Packet Type -> Individual Park Reviews ***/
        private void ProcessParkReviewPacket(ICommunicationChannel stream, Packet receivedPacket)
        {
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
        }

        /*** Process Packet Type -> Delete Park Review ***/
        private void ProcessDeleteParkReviewPacket(ICommunicationChannel stream, Packet receivedPacket)
        {

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
        }

        private ParkReviewManager Get_parkReviewManager()
        {
            return _parkReviewManager;
        }

        /*** Process Packet Type -> Delete Park Review ***/
        public void ProcessDeleteAParkPacket(ICommunicationChannel stream, Packet receivedPacket, ParkReviewManager _parkReviewManager)
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
                _parkDataManager.DeleteParkData(parkName);
                _parkReviewManager.DeleteParkReviews(parkName);

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
        public void ProcessAddAParkPacket(ICommunicationChannel stream, Packet receivedPacket)
        {
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

        }

        /*** Process Packet Type -> Add a Park Review ***/
        public void ProcessAddAParkReviewPacket(ICommunicationChannel stream, Packet receivedPacket)
        {
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

        }

        public void ProcessEditAParkInfoPacket(ICommunicationChannel stream, Packet receivedPacket)
        {
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
        }



        /**************************************************************************************************************
         *                                        Acknowledge Message to Client                                       *
         * ************************************************************************************************************/

        public static void SendAcknowledgement(ICommunicationChannel stream, string message)
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
