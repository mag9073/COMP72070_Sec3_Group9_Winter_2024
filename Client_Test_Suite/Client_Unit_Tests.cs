using LogiPark;
using LogiPark.MVVM.Model;

namespace Client_Test_Suite
{
    [TestClass]
    public class Client_Unit_Tests
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
        public void UT_CL_ProgramClient_008()
        {
            // Arrange
            string expected = "hi";

            // Act
            string result = "hi";

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}