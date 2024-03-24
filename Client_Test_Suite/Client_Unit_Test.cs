//using Client;
using LogiPark.MVVM.Model;
using LogiPark.MVVM.View;
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
        //    string result = "hang";

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

        [TestMethod]
        public void UT_CL_UDM_008()
        {
            // Arrange
            
            // Act

            // Assert
        }

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

        [TestMethod]
        public void UT_CL_UDM_013()
        {
            // Arrange

            // Act

            // Assert
        }

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

        [TestMethod]
        public void UT_CL_UDM_016()
        {
            // Arrange

            // Act

            // Assert
        }

    }
}