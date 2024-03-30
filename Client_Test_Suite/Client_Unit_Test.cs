//using Client;
using System.IO;
using ProtoBuf;
using LogiPark.MVVM.Model;
using LogiPark.MVVM.View;
using LogiPark.MVVM.ViewModel;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography.X509Certificates;

namespace Client_Test_Suite
{
    [TestClass]
    public class Client_Unit_Test
    {

        [TestMethod]
        public void UT_CL_UDM_001()
        {
            // Arrange
            string expected = "hang";
            // Act
            UserDataManager.LoginData loginData = new UserDataManager.LoginData
            {
                username = "hang",
                password = "1234"
            };
            string result = loginData.GetUserName();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_UDM_002()
        {
            // Arrange
            string expected = "1234";
            // Act
            UserDataManager.LoginData loginData = new UserDataManager.LoginData
            {
                username = "hang",
                password = "1234"
            };
            string result = loginData.GetPassword();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_UDM_003()
        {
            // Arrange
            string expected = "hang";
            // Act
            UserDataManager.LoginData loginData = new UserDataManager.LoginData();
            loginData.SetUserName("hang");
            string result = loginData.GetUserName();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_UDM_004()
        {
            // Arrange
            string expected = "1234";
            // Act
            UserDataManager.LoginData loginData = new UserDataManager.LoginData();
            loginData.SetPassword("1234");
            string result = loginData.GetPassword();

            // Assert
            Assert.AreEqual(expected, result);
        }

        //[TestMethod]
        //public void UT_CL_UDM_005()
        //{
        //    // Arrange
        //    string expected = "hang";
        //    // Act
        //    UserDataManager.LoginData loginData = new UserDataManager.LoginData
        //    {
        //        username = "hang",
        //        password = "1234"
        //    };
            

        //    // Assert
        //    Assert.AreEqual(expected, result);
        //}

        [TestMethod]
        public void UT_CL_UDM_006()
        {
            // Arrange
            string expected = "hang";
            // Act
            UserDataManager.LoginData loginData = new UserDataManager.LoginData
            {
                username = "hang",
                password = "1234"
            };
            UserDataManager.Login loginUser = new UserDataManager.Login(loginData);
            UserDataManager.LoginData resultUser = loginUser.GetUserData();
            string result = resultUser.GetUserName();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_UDM_007()
        {
            // Arrange
            string expected = "nick";
            // Act
            UserDataManager.LoginData loginData = new UserDataManager.LoginData
            {
                username = "hang",
                password = "1234"
            };
            UserDataManager.Login loginUser = new UserDataManager.Login(loginData);
            loginUser.SetUserData("nick", "4321");
            UserDataManager.LoginData resultUser = loginUser.GetUserData();
            string result = resultUser.GetUserName();

            // Assert
            Assert.AreEqual(expected, result);
        }

        //[TestMethod]
        //public void UT_CL_UDM_008()
        //{
        //    // Arrange
            
        //    // Act

        //    // Assert
        //}

        [TestMethod]
        public void UT_CL_UDM_009()
        {
            // Arrange
            string expected = "hang";
            // Act
            UserDataManager.SignUpData signData = new UserDataManager.SignUpData
            {
                username = "hang",
                password = "1234"
            };
            string result = signData.GetUserName();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_UDM_010()
        {
            // Arrange
            string expected = "1234";
            // Act
            UserDataManager.SignUpData signData = new UserDataManager.SignUpData
            {
                username = "hang",
                password = "1234"
            };
            string result = signData.GetPassword();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_UDM_011()
        {
            // Arrange
            string expected = "nick";
            // Act
            UserDataManager.SignUpData signData = new UserDataManager.SignUpData
            {
                username = "hang",
                password = "1234"
            };
            signData.SetUserName("nick");
            string result = signData.GetUserName();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_UDM_012()
        {
            // Arrange
            string expected = "4321";
            // Act
            UserDataManager.SignUpData signData = new UserDataManager.SignUpData
            {
                username = "hang",
                password = "1234"
            };
            signData.SetPassword("4321");
            string result = signData.GetPassword();

            // Assert
            Assert.AreEqual(expected, result);
        }

        //[TestMethod]
        //public void UT_CL_UDM_013()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        [TestMethod]
        public void UT_CL_UDM_014()
        {
            // Arrange
            string expected = "hang";
            // Act
            UserDataManager.SignUpData signData = new UserDataManager.SignUpData
            {
                username = "hang",
                password = "1234"
            };
            UserDataManager.SignUp signUser = new UserDataManager.SignUp(signData);
            UserDataManager.SignUpData resultUser = signUser.GetSignUpData();
            string result = resultUser.GetUserName();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_UDM_015()
        {
            // Arrange
            string expected = "nick";
            // Act
            UserDataManager.SignUpData signData = new UserDataManager.SignUpData
            {
                username = "hang",
                password = "1234"
            };
            UserDataManager.SignUp signUser = new UserDataManager.SignUp(signData);
            signUser.SetSignUpData("nick", "4321");
            UserDataManager.SignUpData resultUser = signUser.GetSignUpData();
            string result = resultUser.GetUserName();

            // Assert
            Assert.AreEqual(expected, result);
        }

        //[TestMethod]
        //public void UT_CL_UDM_016()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}


        [TestMethod]
        public void UT_CL_PDM_001() // should pass
        {
            //Arrange
            string expected = "Waterloo Park";

            //Act
            ParkDataManager.ParkData park = new ParkDataManager.ParkData
            {
                parkName = "Waterloo Park",
                parkAddress = "101 king street",
                parkDescription = "Description",
                parkHours = "7am - 12am"
            };

            string result = park.GetParkName();

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_PDM_002() // should pass
        {
            //Arrange
            string expected = "101 king street";

            //Act
            ParkDataManager.ParkData park = new ParkDataManager.ParkData
            {
                parkName = "Waterloo Park",
                parkAddress = "101 king street",
                parkDescription = "Description",
                parkHours = "7am - 12am"
            };

            string result = park.GetParkAddress();

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_PDM_003() // should pass
        {
            //Arrange
            string expected = "Description";

            //Act
            ParkDataManager.ParkData park = new ParkDataManager.ParkData
            {
                parkName = "Waterloo Park",
                parkAddress = "101 king street",
                parkDescription = "Description",
                parkHours = "7am - 12am"
            };

            string result = park.GetParkDescription();

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_PDM_004() // should pass
        {
            //Arrange
            string expected = "7am - 12am";

            //Act
            ParkDataManager.ParkData park = new ParkDataManager.ParkData
            {
                parkName = "Waterloo Park",
                parkAddress = "101 king street",
                parkDescription = "Description",
                parkHours = "7am - 12am"
            };

            string result = park.GetParkHours();

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_PDM_005() // should pass
        {
            //Arrange
            var parkData = new ParkDataManager.ParkData();
            var expectedName = "Central park";

            //Act
            parkData.SetParkName(expectedName);

            //Assert
            Assert.AreEqual(expectedName, parkData.parkName);
        }

        [TestMethod]
        public void UT_CL_PDM_006() // should pass
        {
            //Arrange
            var parkData = new ParkDataManager.ParkData();
            var expectedAddress = "London";

            //Act
            parkData.SetParkAddress(expectedAddress);

            //Assert
            Assert.AreEqual(expectedAddress, parkData.parkAddress);
        }

        [TestMethod]
        public void UT_CL_PDM_007() // should pass
        {
            //Arrange
            var parkData = new ParkDataManager.ParkData();
            var expectedDescription = "park info";

            //Act
            parkData.SetParkDescription(expectedDescription);

            //Assert
            Assert.AreEqual(expectedDescription, parkData.parkDescription);
        }

        [TestMethod]
        public void UT_CL_PDM_008() // should pass
        {
            //Arrange
            var parkData = new ParkDataManager.ParkData();
            var expectedHours = "time";

            //Act
            parkData.SetParkHours(expectedHours);

            //Assert
            Assert.AreEqual(expectedHours, parkData.parkHours);
        }

        [TestMethod]
        public void UT_CL_PRM_001() // should pass
        {
            //Arrange
            var expected = "bob smith";

            //Act
            ParkReviewManager.ParkReviewData reviewData = new ParkReviewManager.ParkReviewData
            {
                ParkName = "Waterloo Park",
                UserName = "bob smith",
                Rating = 4f,
                DateOfPosting = DateTime.Now,
                Review = "nice park, clean."
            };

            string result = reviewData.GetUserName();

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_PRM_002() // should pass
        {
            //Arrange
            var expected = 4f;

            //Act
            ParkReviewManager.ParkReviewData reviewData = new ParkReviewManager.ParkReviewData
            {
                ParkName = "Waterloo Park",
                UserName = "bob smith",
                Rating = 4f,
                DateOfPosting = DateTime.Now,
                Review = "nice park, clean."
            };

            var result = reviewData.GetParkRating();

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_PRM_003() // should pass
        {
            //Arrange
            var expected = new DateTime(2020, 02, 20);

            //Act
            ParkReviewManager.ParkReviewData reviewData = new ParkReviewManager.ParkReviewData
            {
                ParkName = "Waterloo Park",
                UserName = "bob smith",
                Rating = 4f,
                DateOfPosting = new DateTime(2020, 02, 20),
                Review = "nice park, clean."
            };

            var result = reviewData.GetDateOfPosting();

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_PRM_004() // should pass
        {
            //Arrange
            var expected = "nice park, clean.";

            //Act
            ParkReviewManager.ParkReviewData reviewData = new ParkReviewManager.ParkReviewData
            {
                ParkName = "Waterloo Park",
                UserName = "bob smith",
                Rating = 4f,
                DateOfPosting = DateTime.Now,
                Review = "nice park, clean."
            };

            string result = reviewData.GetReview();

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_PRM_005() // should pass
        {
            //Arrange
            var review = new ParkReviewManager.ParkReviewData();
            string expectedName = "jown dow";

            review.SetUserName(expectedName);

            //Assert
            Assert.AreEqual(expectedName, review.UserName);
        }

        [TestMethod]
        public void UT_CL_PRM_006() // should pass
        {
            //Arrange
            var review = new ParkReviewManager.ParkReviewData();
            var expectedRating = 3f;

            review.SetParkRating(expectedRating);

            //Assert
            Assert.AreEqual(expectedRating, review.Rating);
        }

        [TestMethod]
        public void UT_CL_PRM_007() // should pass
        {
            //Arrange
            var review = new ParkReviewManager.ParkReviewData();
            var expectedTime = new DateTime(2024, 01, 07);

            review.SetDateOfPosting(expectedTime);

            //Assert
            Assert.AreEqual(expectedTime, review.DateOfPosting);
        }

        [TestMethod]
        public void UT_CL_PRM_008() // should pass
        {
            //Arrange
            var review = new ParkReviewManager.ParkReviewData();
            string expectedReview = "decent park cant complain";

            review.SetReview(expectedReview);

            //Assert
            Assert.AreEqual(expectedReview, review.Review);
        }
    }

    [TestClass]
    public class Client_Packet_Unit_Tests
    {
        [TestMethod]
        public void UT_Packet_001()
        {
            // Arrange
            Packet pkt = new Packet();

            // Act

            // Assert
            Assert.IsNotNull(pkt);
        }

        [TestMethod]
        public void UT_Packet_002()
        {
            // Arrange
            Packet pkt = new Packet();

            // Act
            pkt.SetPacketHead(1, 2, Types.login);
            Header tempHead = pkt.GetPacketHeader();
            Types type = tempHead.GetType();

            // Assert
            Assert.AreEqual(Types.login, type);
        }

        [TestMethod]
        public void UT_Packet_003()
        {
            // Arrange
            Packet pkt = new Packet();

            // Act
            byte[] tempByte = { (byte)'a', (byte)'b', (byte)'c' };
            pkt.SetPacketBody(tempByte, 3);
            Body tempBody = pkt.GetBody();
            byte[] result = tempBody.GetBodyBuffer();

            // Assert
            Assert.AreEqual(tempByte, result);
        }

    }
}