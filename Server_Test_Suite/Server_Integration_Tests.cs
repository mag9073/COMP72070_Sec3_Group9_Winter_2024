using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.DataStructure;
using Server.Implementations;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Moq;
using Server_Test_Suite.Mock.Interfaces;
using Server.Interfaces;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using static Server_Test_Suite.PacketProcessorIntegrationTests;
using ProtoBuf;
using Server;
using LogiPark.MVVM.Model;
using UserDataManager = Server.Implementations.UserDataManager;
using ParkDataManager = Server.Implementations.ParkDataManager;
using ParkReviewManager = Server.Implementations.ParkReviewManager;
using ImageManager = Server.Implementations.ImageManager;
using static Server.DataStructure.PacketData;



namespace Server_Test_Suite
{
    [TestClass]
    public class PacketProcessorIntegrationTests
    {
        // This may be moved to its own class
        public class FakeCommunicationChannel : ICommunicationChannel
        {

            private NetworkStream _stream;

            public byte[] WrittenBytes { get; private set; }
            public int WrittenOffset { get; private set; }
            public int WrittenSize { get; private set; }
            public bool WriteCalled { get; private set; } = false;

            public byte[] ReadBytes { get; set; }
            public bool DataAvailable { get; set; } = true; 
            public bool CloseCalled { get; private set; } = false;
            public bool FlushCalled { get; private set; } = false;
            public List<byte[]> WrittenMessages { get; } = new List<byte[]>();

            public void Write(byte[] buffer, int offset, int size)
            {
                WrittenBytes = buffer;
                WrittenOffset = offset;
                WrittenSize = size;
                WriteCalled = true;
                byte[] writtenData = new byte[size];
                Array.Copy(buffer, offset, writtenData, 0, size);
                WrittenMessages.Add(writtenData);
            }

            public async Task WriteAsync(byte[] nameBytes, int v, int length)
            {
                await _stream.WriteAsync(nameBytes, v, length);
            }

            // Simulate as how network stream.read is done
            public int Read(byte[] buffer, int offset, int size)
            {
                if (ReadBytes == null || ReadBytes.Length == 0)
                {
                    return 0; 
                }

                int bytesToCopy = Math.Min(size, ReadBytes.Length);
                Array.Copy(ReadBytes, 0, buffer, offset, bytesToCopy);
                return bytesToCopy; 
            }

            public void Close()
            {
                CloseCalled = true;
            }

            public void Flush()
            {
                FlushCalled = true;
            }
        }

        // Attempt to deserilizeParkData to a list
        public List<ParkData> DeserializeParkDataList(byte[] data, int offset, int size)
        {
            using (MemoryStream ms = new MemoryStream(data, offset, size))
            {
                List<ParkData> listParkData = Serializer.Deserialize<List<ParkData>>(ms);
                return listParkData;
            }
        }

        [TestMethod]
            public void IT_SVR_001_ProcessLoginPacketTest()
            {
                // Arrange
                FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
                UserDataManager userDataManager = new UserDataManager();
                ParkDataManager parkDataManager = new ParkDataManager();
                ParkReviewManager parkReviewManager = new ParkReviewManager();
                ImageManager imageManager = new ImageManager();
                ServerStateManager serverStateManager = new ServerStateManager();
                string expectedAcknowledgementMessage = "Username and password are Correct!!! \\o/";


                PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager, serverStateManager);
                UserDataManager.LoginData loginData = new UserDataManager.LoginData
                {
                    username = "hang",
                    password = "1234",
                };


                byte[] loginDataBytes = loginData.SerializeToByteArray();

                PacketData.Packet packet = new PacketData.Packet();
                packet.SetPacketHead(1, 2, Server.DataStructure.PacketData.Types.login);

                packet.SetPacketBody(loginDataBytes, (uint)loginDataBytes.Length);

                // Act
                packetProcessor.ProcessPacket(packet, fakeChannel, null);   

                // Assert
                Assert.IsTrue(fakeChannel.WriteCalled, "Write method was not called.");

                string actualAcknowledgementMessage = Encoding.UTF8.GetString(fakeChannel.WrittenBytes, fakeChannel.WrittenOffset, fakeChannel.WrittenSize);
           
                Assert.AreEqual(expectedAcknowledgementMessage, actualAcknowledgementMessage, "The acknowledgement message is invalid.");
            }

            [TestMethod]
            public void IT_SVR_002_ProcessSignUpPacketTest()
            {
                // Arrange
                FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
                UserDataManager userDataManager = new UserDataManager();
                ParkDataManager parkDataManager = new ParkDataManager();
                ParkReviewManager parkReviewManager = new ParkReviewManager();
                ImageManager imageManager = new ImageManager();
                ServerStateManager serverStateManager = new ServerStateManager();
                string expectedAcknowledgementMessage = "Successfully Sign Up \\o/";


                PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager, serverStateManager);
                UserDataManager.SignUpData signUpData = new UserDataManager.SignUpData
                {
                    username = "hangg",
                    password = "1234",
                };


                byte[] signUpDataBytes = signUpData.SerializeToByteArray();

                PacketData.Packet packet = new PacketData.Packet();
                packet.SetPacketHead(1, 2, Server.DataStructure.PacketData.Types.register);

                packet.SetPacketBody(signUpDataBytes, (uint)signUpDataBytes.Length);

                // Act
                packetProcessor.ProcessPacket(packet, fakeChannel, null);

                // Assert
                Assert.IsTrue(fakeChannel.WriteCalled, "Write method was not called.");

                string actualAcknowledgementMessage = Encoding.UTF8.GetString(fakeChannel.WrittenBytes, fakeChannel.WrittenOffset, fakeChannel.WrittenSize);

                Assert.AreEqual(expectedAcknowledgementMessage, actualAcknowledgementMessage, "The acknowledgement message is invalid.");
            }


            [TestMethod]
            public void IT_SVR_003_ProcessAdminLoginPacketTest()
            {
                // Arrange
                FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
                UserDataManager userDataManager = new UserDataManager();
                ParkDataManager parkDataManager = new ParkDataManager();
                ParkReviewManager parkReviewManager = new ParkReviewManager();
                ImageManager imageManager = new ImageManager();
                ServerStateManager serverStateManager = new ServerStateManager();
                string expectedAcknowledgementMessage = "Username and password are Correct!!! \\o/";


                PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager, serverStateManager);
                UserDataManager.LoginData loginData = new UserDataManager.LoginData
                {
                    username = "admin",
                    password = "123",
                };


                byte[] loginDataBytes = loginData.SerializeToByteArray();

                PacketData.Packet packet = new PacketData.Packet();
                packet.SetPacketHead(1, 2, Server.DataStructure.PacketData.Types.login_admin);

                packet.SetPacketBody(loginDataBytes, (uint)loginDataBytes.Length);

                // Act
                packetProcessor.ProcessPacket(packet, fakeChannel, null);

                // Assert
                Assert.IsTrue(fakeChannel.WriteCalled, "Write method was not called.");

                string actualAcknowledgementMessage = Encoding.UTF8.GetString(fakeChannel.WrittenBytes, fakeChannel.WrittenOffset, fakeChannel.WrittenSize);

                Assert.AreEqual(expectedAcknowledgementMessage, actualAcknowledgementMessage, "The acknowledgement message is invalid.");
            }


        [TestMethod]
        public void IT_SVR_005_ProcessOneParkDataPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();
            ServerStateManager serverStateManager = new ServerStateManager();
            PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager, serverStateManager);
            string targetParkName = "Kitchener Park";

            ParkData expectedParkData = new ParkData
            {
                parkName = targetParkName,
                parkAddress = "123 Park St",
                parkDescription = "This is a beautiful with a lot of trash",
                parkHours = "9:00 AM - 5:00 PM",
            };
            ParkData actualParkData = new ParkData();


            // Act
            PacketData.Packet packet = new PacketData.Packet();
            packet.SetPacketHead(1, 2, Server.DataStructure.PacketData.Types.a_park);
            byte[] parknameBuffer = Encoding.UTF8.GetBytes(targetParkName);
            packet.SetPacketBody(parknameBuffer, (uint)parknameBuffer.Length);

            packetProcessor.ProcessPacket(packet, fakeChannel, null);
            actualParkData.deserializeParkData(fakeChannel.WrittenBytes, fakeChannel.WrittenOffset, fakeChannel.WrittenSize);

            // Assert
            Assert.IsTrue(fakeChannel.WriteCalled, "Write method was not called.");

            Assert.AreEqual(expectedParkData.parkName, actualParkData.parkName, "Park name do not match");
            Assert.AreEqual(expectedParkData.parkAddress, actualParkData.parkAddress, "Park address do not match");
            Assert.AreEqual(expectedParkData.parkDescription, actualParkData.parkDescription, "Park Descriptions does not match");
            Assert.AreEqual(expectedParkData.parkHours, actualParkData.parkHours, "Park hours do not match");
        }

        // Incomplete
        [TestMethod]
        public void IT_SVR_006_ProcessAllParkImagesTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();
            ServerStateManager serverStateManager = new ServerStateManager();

            // Act



            // Assert




        }


        // Incomplete
        [TestMethod]
        public void IT_SVR_007_ProcessOneParkImageTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();
            ServerStateManager serverStateManager = new ServerStateManager();

            // Act



            // Assert




        }


        // Incomplete
        [TestMethod]
        public void IT_SVR_008_ProcessAllParkReviewsTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ImageManager imageManager = new ImageManager();
            ServerStateManager serverStateManager = new ServerStateManager();

            ParkReviewManager parkReviewManager = new ParkReviewManager
            {
                
            };

            // Act



            // Assert




        }



        [TestMethod]
        public void IT_SVR_009_ProcessAReviewPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();
            ServerStateManager serverStateManager = new ServerStateManager();
            string targetParkName = "Waterloo Park";

            ParkReviewData expectedParkReviewData = new ParkReviewData
            {
                ParkName = targetParkName,
                UserName = "Katherine Slattery",
                Rating = 4,
                DateOfPosting = new DateTime(2024, 3, 8, 0, 43, 8),
                Review = "I have many good memories walking through this park. I like the path around the lake and the trails through the woods. There are some nice flowering trees which are so peaceful to sit under in the summer.",
            };

            ParkReviewData actualParkReviewData = new ParkReviewData();
            PacketData.Packet sendPacket = new PacketData.Packet();
            PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager, serverStateManager);


            // Act
            sendPacket.SetPacketHead(1, 2, Server.DataStructure.PacketData.Types.review);
            byte[] parkReviewBuffer = Encoding.UTF8.GetBytes(targetParkName);
            sendPacket.SetPacketBody(parkReviewBuffer, (uint)parkReviewBuffer.Length);
            packetProcessor.ProcessPacket(sendPacket, fakeChannel, null);
            actualParkReviewData.deserializeParkReviewData(fakeChannel.WrittenBytes, fakeChannel.WrittenOffset, fakeChannel.WrittenSize);

            // Assert
            Assert.IsTrue(fakeChannel.WriteCalled, "Write method was not called");
            Assert.AreEqual(expectedParkReviewData.ParkName, actualParkReviewData.ParkName, "Park Name Data do not match");
            Assert.AreEqual(expectedParkReviewData.UserName, actualParkReviewData.UserName, "Park UserName Data do not match");
            Assert.AreEqual(expectedParkReviewData.Rating, actualParkReviewData.Rating, "Park Rating Data do not match");
            Assert.AreEqual(expectedParkReviewData.DateOfPosting, actualParkReviewData.DateOfPosting, "Park DateOfPosting Data do not match");
            Assert.AreEqual(expectedParkReviewData.Review, actualParkReviewData.Review, "Park Review Data do not match");

        }



        [TestMethod]
        public void IT_SVR_011_ProcessDeleteParkReviewPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();
            ServerStateManager serverStateManager = new ServerStateManager();
            string targetParkName = "Clair Lake Park";

            ParkReviewData parkReviewData = new ParkReviewData
            {
                ParkName = "Clair Lake Park",
                UserName = "Barry Smylie",
                Rating = 3,
                DateOfPosting = new DateTime(2024, 3, 8, 0, 43, 8),
                Review = "The trail doesn't follow the banks of the reservoir.  It is a sports park with swimming pool, tennis courts, and field sports.  There is one access to the water",
            };

            PacketData.Packet sendPacket = new PacketData.Packet();
            PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager, serverStateManager);

            // Act
            sendPacket.SetPacketHead(1, 2, Server.DataStructure.PacketData.Types.delete_review);
            byte[] deleteReviewsDataBuffer = parkReviewData.SerializeToByteArray();
            sendPacket.SetPacketBody(deleteReviewsDataBuffer, (uint)deleteReviewsDataBuffer.Length);
            packetProcessor.ProcessPacket(sendPacket, fakeChannel, null);



            // Assert
            Assert.IsTrue(fakeChannel.WriteCalled, "Write method was not called");
            string expectedAcknowledgementMessage = "Review deleted successfully.";
            string actualAcknowledgementMessage = Encoding.UTF8.GetString(fakeChannel.WrittenBytes, fakeChannel.WrittenOffset, fakeChannel.WrittenSize);
            Assert.AreEqual(expectedAcknowledgementMessage, actualAcknowledgementMessage, "Acknowledgement message does not match expected.");




        }


        [TestMethod]
        public void IT_SVR_012_ProcessDeleteAParkPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();
            ServerStateManager serverStateManager = new ServerStateManager();
            string targetParkName = "Hillside Park";

            PacketData.Packet sendPacket = new PacketData.Packet();
            PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager, serverStateManager);


            // Act
            sendPacket.SetPacketHead(1, 2, Server.DataStructure.PacketData.Types.delete_park);
            byte[] parkNameBuffer = Encoding.UTF8.GetBytes(targetParkName);
            sendPacket.SetPacketBody(parkNameBuffer, (uint)parkNameBuffer.Length);
            packetProcessor.ProcessPacket(sendPacket, fakeChannel, null);



            // Assert
            Assert.IsTrue(fakeChannel.WriteCalled, "Write method was not called");
            string expectedAcknowledgementMessage = $"{targetParkName} has been deleted -> (park data, park image, park reviews)";
            string actualAcknowledgementMessage = Encoding.UTF8.GetString(fakeChannel.WrittenBytes, fakeChannel.WrittenOffset, fakeChannel.WrittenSize);
            Assert.AreEqual(expectedAcknowledgementMessage, actualAcknowledgementMessage, "Acknowledgement message does not match expected.");



        }

        [TestMethod]
        public void IT_SVR_013_ProcessAddAParkPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();
            ServerStateManager serverStateManager = new ServerStateManager();

            PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager, serverStateManager);
            ParkData newParkData = new ParkData
            {
                parkName = "Toronto Park",
                parkAddress = "123 Toronto Park Ave, Toronto, ON",
                parkDescription = "A brand new park for everyone.",
                parkHours = "7 a.m. - 7 p.m."
            };

            byte[] newParkDataBytes = newParkData.SerializeToByteArray();
            PacketData.Packet packet = new PacketData.Packet();
            packet.SetPacketHead(1, 2, Server.DataStructure.PacketData.Types.add_park);
            packet.SetPacketBody(newParkDataBytes, (uint)newParkDataBytes.Length);

            string expectedAcknowledgementMessage = "Park data added successfully.";

            // Act
            packetProcessor.ProcessPacket(packet, fakeChannel, null);

            // Assert
            Assert.IsTrue(fakeChannel.WriteCalled, "Write method was not called.");
            string actualAcknowledgementMessage = Encoding.UTF8.GetString(fakeChannel.WrittenBytes, fakeChannel.WrittenOffset, fakeChannel.WrittenSize);
            Assert.AreEqual(expectedAcknowledgementMessage, actualAcknowledgementMessage, "The acknowledgement message is invalid.");
        }


        [TestMethod]
        public void IT_SVR_014_ProcessAddAParkReviewPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();
            ServerStateManager serverStateManager = new ServerStateManager();

            ParkReviewData expectedParkReviewData = new ParkReviewData
            {
                ParkName = "Clair Lake Park",
                UserName = "Test",
                Rating = 5,
                DateOfPosting = new DateTime(2024, 3, 8, 0, 43, 8),
                Review = "This is a foo Park",
            };

            PacketData.Packet sendPacket = new PacketData.Packet();
            PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager, serverStateManager);
            ParkReviewData actualParkReviewData = new ParkReviewData();

            // Act
            sendPacket.SetPacketHead(1, 2, Server.DataStructure.PacketData.Types.add_review);
            byte[] parkReviewBuffer = expectedParkReviewData.SerializeToByteArray();

            sendPacket.SetPacketBody(parkReviewBuffer, (uint)parkReviewBuffer.Length);

            packetProcessor.ProcessPacket(sendPacket, fakeChannel, null);

            

            // Assert
            Assert.IsTrue(fakeChannel.WriteCalled, "Write method was not called");
            string expectedAcknowledgementMessage = "Review added successfully";
            string actualAcknowledgementMessage = Encoding.UTF8.GetString(fakeChannel.WrittenBytes, fakeChannel.WrittenOffset, fakeChannel.WrittenSize);
            Assert.AreEqual(expectedAcknowledgementMessage, actualAcknowledgementMessage, "Acknowledgement message does not match expected.");


        }


    }



}