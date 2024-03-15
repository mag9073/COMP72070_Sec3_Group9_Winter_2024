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
using Server.Implementations;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using static Server_Test_Suite.PacketProcessorIntegrationTests;
using ProtoBuf;
using Server;
using Server.Interfaces;
using ProtoBuf;


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

            public void Write(byte[] buffer, int offset, int size)
            {
                WrittenBytes = buffer;
                WrittenOffset = offset;
                WrittenSize = size;
                WriteCalled = true;
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
            public void IT_001_ProcessLoginPacketTest()
            {
                // Arrange
                FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
                UserDataManager userDataManager = new UserDataManager();
                ParkDataManager parkDataManager = new ParkDataManager();
                ParkReviewManager parkReviewManager = new ParkReviewManager();
                ImageManager imageManager = new ImageManager();
                string expectedAcknowledgementMessage = "Username and password are Correct!!! \\o/";


                PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager);
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
            public void IT_002_ProcessSignUpPacketTest()
            {
                // Arrange
                FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
                UserDataManager userDataManager = new UserDataManager();
                ParkDataManager parkDataManager = new ParkDataManager();
                ParkReviewManager parkReviewManager = new ParkReviewManager();
                ImageManager imageManager = new ImageManager();
                string expectedAcknowledgementMessage = "Please enter username to register!!!! \\o/";


                PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager);
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
        public void IT_003_ProcessAdminLoginPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();
            string expectedAcknowledgementMessage = "Username and password are Correct!!! \\o/";


            PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager);
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
        public void IT_004_ProcessAllParkDataPacketTest()
        {
           //Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();

            List<ParkData> expectedParkData = new List<ParkData>
            {
                new ParkData
                {
                    parkName = "Kitchener Park",
                    parkAddress = "Kitchener Park",
                    parkDescription = "This is a beautiful with a lot of trash",
                    parkHours = "9:00 AM - 5:00 PM"
                },
                new ParkData
                {
                    parkName = "Cambridge Park",
                    parkAddress = "123 Fountain St",
                    parkDescription = "This is Loo Park",
                    parkHours = "Open 24 Hours",
                }
            };

            List<ParkData> actualParkData = new List<ParkData> ();

            PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager);

            // Act
            PacketData.Packet packet = new PacketData.Packet();
            packet.SetPacketHead(1, 2, Server.DataStructure.PacketData.Types.allparkdata);
            packetProcessor.ProcessPacket(packet, fakeChannel, null);
            actualParkData = DeserializeParkDataList(fakeChannel.WrittenBytes, fakeChannel.WrittenOffset, fakeChannel.WrittenSize);


            // Assert

            // Implement assertions

        }

        [TestMethod]
        public void IT_005_ProcessOneParkDataPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();
            PacketProcessor packetProcessor = new PacketProcessor(userDataManager, parkDataManager, parkReviewManager, imageManager);
            string targetParkName = "Waterloo Park";

            ParkData expectedParkData = new ParkData
            {
                parkName = targetParkName,
                parkAddress = "50 Young St W, Waterloo, ON",
                parkDescription = "Waterloo Park is an urban park situated in Waterloo, Ontario, Canada on land within Block 2 of the Haldimand Tract. Spanning 111 acres within the Uptown area of Waterloo, it opened in 1893 and is the oldest park in the city.",
                parkHours = "7 a.m. - 10 p.m.",
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


        [TestMethod]
        public void IT_006_ProcessAllParkDataPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();

            // Act



            // Assert




        }


        [TestMethod]
        public void IT_007_ProcessOneParkDataPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();

            // Act



            // Assert




        }



        [TestMethod]
        public void IT_008_ProcessOneParkImagePacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();

            // Act



            // Assert




        }



        [TestMethod]
        public void IT_009_ProcessAllReviewsPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();

            // Act



            // Assert




        }


        [TestMethod]
        public void IT_010_ProcessParkReviewPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();

            // Act



            // Assert




        }


        [TestMethod]
        public void IT_011_ProcessDeleteParkReviewPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();

            // Act



            // Assert




        }


        [TestMethod]
        public void IT_012_ProcessAddAParkPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();

            // Act



            // Assert




        }



        [TestMethod]
        public void IT_013_ProcessAddAParkReviewPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();

            // Act



            // Assert




        }


        [TestMethod]
        public void IT_014_ProcessEditAParkInfoPacketTest()
        {
            // Arrange
            FakeCommunicationChannel fakeChannel = new FakeCommunicationChannel();
            UserDataManager userDataManager = new UserDataManager();
            ParkDataManager parkDataManager = new ParkDataManager();
            ParkReviewManager parkReviewManager = new ParkReviewManager();
            ImageManager imageManager = new ImageManager();

            // Act



            // Assert




        }




    }



}