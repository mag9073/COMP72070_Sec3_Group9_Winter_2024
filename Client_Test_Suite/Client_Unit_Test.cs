//using Client;
using LogiPark.MVVM.Model;
using LogiPark.MVVM.View;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography.X509Certificates;
namespace Client_Test_Suite
{
    [TestClass]
    public class Client_Unit_Test
    {
        [TestMethod]
        public void UT_CL_001() // should pass
        {
            //var view = new ClientHomeView();
            //Assert.IsNotNull(view.Client);
        }

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
    }
}