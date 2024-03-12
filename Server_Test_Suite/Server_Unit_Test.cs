using Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography.X509Certificates;
using Server.Implementations;
using Server.DataStructure;
namespace Server_Test_Suite
{
    [TestClass]
    public class Server_Unit_Test
    {

        [TestClass]
        public class ParkDataManagerTests
        {
            private string _testGoodFilePath = string.Empty;
            private string _testBadFilePath = string.Empty;
            private string _testNotValidFilePath = string.Empty;
            private ParkDataManager _parkDataManager;

            [TestInitialize]
            public void TestInitializer()
            {
                _testGoodFilePath = Path.GetTempFileName();

                File.WriteAllLines(_testGoodFilePath, new[]
                    {
                    "Kitchener Park",
                    "123 Park St",
                    "This is a beautiful with a lot of trash",
                    "9:00 AM - 5:00 PM",
                    "Waterloo Park",
                    "123 Loo Ave",
                    "This is Loo Park",
                    "Open 24 Hours",
                    "Cambridge Park",
                    "123 Fountain St",
                    "This is Loo Park",
                    "9:00 AM - 5:00 PM",
                });

                File.WriteAllLines("/Database/ParkData.txt", new[]
    {
                    "Kitchener Park",
                    "123 Park St",
                    "This is a beautiful with a lot of trash",
                    "9:00 AM - 5:00 PM",
                    "Waterloo Park",
                    "123 Loo Ave",
                    "This is Loo Park",
                    "Open 24 Hours",
                    "Cambridge Park",
                    "123 Fountain St",
                    "This is Loo Park",
                    "Open 24 Hours",
                });

                File.WriteAllLines("/Database/UserDB.txt", new[]
                {
                   "hang",
                   "1234",
                    "mino",
                    "12345",
                });

                _testBadFilePath += Path.GetTempFileName();

                File.WriteAllLines(_testBadFilePath, new string[] { });

                _parkDataManager = new ParkDataManager();

                _testNotValidFilePath = "Y://nonexistentdirectory//nonexistentfile.txt";
            }

            [TestMethod]
            public void ReadOneParkDataFromFile_Return_CorrectParkData()
            {
                // Arrange
                ParkData expectedResult = new ParkData
                {
                    parkName = "Kitchener Park",
                    parkAddress = "123 Park St",
                    parkDescription = "This is a beautiful with a lot of trash",
                    parkHours = "9:00 AM - 5:00 PM",
                };

                // Act
                ParkData actualResult = _parkDataManager.ReadOneParkDataFromFile(_testGoodFilePath, "Kitchener Park");

                // Assert
                Assert.IsNotNull(actualResult, "Expected not null Park Data object");
                Assert.AreEqual(expectedResult.parkName, actualResult.parkName, "Park name doesnot match");
                Assert.AreEqual(expectedResult.parkAddress, actualResult.parkAddress, "Park descriptions doesnt match");
                Assert.AreEqual(expectedResult.parkHours, actualResult.parkHours, "Park hours do not match");
            }

            [TestMethod]
            public void ReadOneParkDataFromFile_WithNoMatchingName_ReturnsNull()
            {
                // Act
                ParkData result = _parkDataManager.ReadOneParkDataFromFile(_testGoodFilePath, "South Park");

                // Assert
                Assert.IsNull(result, "Expected a null result for a nonexistent park name");
            }

            [TestMethod]
            public void ReadOneParkDataFromFile_NonExistentFile_CatchesException()
            {
                // Arrange
                string parkName = "South Park";

                // Act
                ParkData result;
                result = _parkDataManager.ReadOneParkDataFromFile(_testNotValidFilePath, parkName);
                // Assert within catch to ensure exception is caught and handled
                Assert.IsNull(result, "Expected result to be null when exception is caught.");
            }

            [TestMethod]
            public void ReadAllParkDataFromFile_ReturnsCorrectParkDataArray()
            {
                // Arrange
                ParkData expectedResult = new ParkData
                {
                    parkName = "Kitchener Park",
                    parkAddress = "123 Park St",
                    parkDescription = "This is a beautiful with a lot of trash",
                    parkHours = "9:00 AM - 5:00 PM",
                };

                // Act
                ParkData[] parks = _parkDataManager.ReadAllParkDataFromFile(_testGoodFilePath);

                // Assert
                Assert.AreEqual(3, parks.Length, "Expected one park entries to be read.");
                Assert.AreEqual(expectedResult.parkName, parks[0].parkName, "First park name does not match.");
                Assert.AreEqual(expectedResult.parkAddress, parks[0].parkAddress, "First park address does not match.");
                Assert.AreEqual(expectedResult.parkDescription, parks[0].parkDescription, "First park description does not match.");
                Assert.AreEqual(expectedResult.parkHours, parks[0].parkHours, "First park hours does not match.");

            }

            [TestMethod]
            public void AppendParkDataToFile_Returns_ValidData()
            {
                // Arrange
                string newFilePath = Path.GetTempFileName();

                ParkData testData = new ParkData
                {
                    parkName = "Kitchener Park",
                    parkAddress = "123 Park St",
                    parkDescription = "This is a beautiful with a lot of trash",
                    parkHours = "9:00 AM - 5:00 PM",
                };

                // Act
                _parkDataManager.AppendParkDataToFile(newFilePath, testData);

                // Assert
                string actualResults = File.ReadAllText(newFilePath);
                string expectedResults = $"{testData.parkName}\r\n{testData.parkAddress}\r\n{testData.parkDescription}\r\n{testData.parkHours}";

                Assert.AreEqual(expectedResults, actualResults, "The content appended to the file does not match the expected result");
            }


            [TestMethod]
            public void AppendParkDataToFile_Returns_InValidData()
            {
                // Arrange
                ParkData testData = new ParkData
                {
                    parkName = "Kitchener Park",
                    parkAddress = "123 Park St",
                    parkDescription = "This is a beautiful with a lot of trash",
                    parkHours = "9:00 AM - 5:00 PM",
                };

                // Act
                try
                {
                    _parkDataManager.AppendParkDataToFile(_testNotValidFilePath, testData);
                    Assert.Fail("Expected an exception to be thrown due to an invalid file path, but none was thrown.");
                } catch (Exception ex)
                {
                    Assert.IsNotNull(ex, "An exception should be thrown when attempting to write to an invalid file path.");
                }
            }

            [TestMethod]
            public void DeleteParkData_RemoveSpecificParkData()
            {
                
                // Arrange
                ParkData testData = new ParkData
                {
                    parkName = "Waterloo Park",
                    parkAddress = "123 Loo Ave",
                    parkDescription = "This is Loo Park",
                    parkHours = "Open 24 Hours",
                };

                string targetParkName = "Waterloo Park";

                // Act
                _parkDataManager.DeleteParkData(targetParkName);

                // Assert
                string[] remainingLines = File.ReadAllLines("../../../Database/ParkData.txt");
                Console.WriteLine(remainingLines);
                Assert.IsFalse(remainingLines.Contains(targetParkName), $"{targetParkName} should be deleted.");
                Assert.AreEqual(8, remainingLines.Length, "Expected number of lines after deletion is incorrect.");
                Assert.IsFalse(remainingLines.Contains(testData.parkName), "Associated Address should be deleted.");
                Assert.IsFalse(remainingLines.Contains(testData.parkAddress), "Associated Description should be deleted.");
            }

            [TestMethod]    
            public void EditAParkDataToFile_Rreturn_UpdatedParkData()
            {
                // Arrange
                ParkData updatedParkData = new ParkData
                {
                    parkName = "Cambridge Park",
                    parkAddress = "123 Fountain St",
                    parkDescription = "This is Loo Park",
                    parkHours = "9:00 AM - 5:00 PM",
                };

                // Act
                _parkDataManager.EditAParkDataToFile(_testGoodFilePath, updatedParkData);

                ParkData[] updatedParks = _parkDataManager.ReadAllParkDataFromFile(_testGoodFilePath);
                ParkData updatedPark = updatedParks.FirstOrDefault(p => p.parkName == updatedParkData.parkName);

                Assert.IsNotNull(updatedPark, "Updated park should exist.");

                Assert.AreEqual(updatedParkData.parkAddress, updatedPark.parkAddress, "Park address should be updated.");
                Assert.AreEqual(updatedParkData.parkDescription, updatedPark.parkDescription, "Park description should be updated.");
                Assert.AreEqual(updatedParkData.parkHours, updatedPark.parkHours, "Park hours should be updated.");
            }

        }

        [TestClass]
        public class UserDataManagerTests
        {
            [TestMethod]
            public void PerformLogin_ValidUser_ReturnsSuccessMessage()
            {
                // Arrange
                UserDataManager userDataManager = new UserDataManager();
                UserDataManager.LoginData loginData = new UserDataManager.LoginData
                {
                    username = "hang",
                    password = "1234",
                };

                // Act
                string result = userDataManager.PerformLogin(loginData);

                // Assert
                Assert.AreEqual("Username and password are Correct!!! \\o/", result, "Expected successful login message.");
            }

            [TestMethod]
            public void PerformAdminLogin_ValidAdmin_ReturnsSuccessMessage()
            {
                // Arrange
                UserDataManager userDataManager = new UserDataManager();
                UserDataManager.LoginData loginData = new UserDataManager.LoginData
                {
                    username = "admin",
                    password = "123",
                };

                // Act
                string result = userDataManager.PerformAdminLogin(loginData);

                // Assert
                Assert.AreEqual("Username and password are Correct!!! \\o/", result, "Expected successful admin login message.");
            }

            [TestMethod]
            public void PerformSignUp_NewUser_ReturnsSuccessMessage()
            {
                // Arrange
                UserDataManager userDataManager = new UserDataManager();
                UserDataManager.SignUpData signUpData = new UserDataManager.SignUpData
                {
                    username = "ducky12",
                    password = "1234",
                };

                // Act
                string result = userDataManager.PerformSignUp(signUpData);

                // Assert
                Assert.AreEqual("Please enter username to register!!!! \\o/", result, "Expected successful signup message.");
            }
        }

        [TestClass]
        public class LoggerTests
        {
            [TestMethod]
            public void UT_LOG_001()
            {
                //Arrange
                bool expected = true;
                byte[] data = { (byte)'a', (byte)'b' };
                Logger logger = new Logger("../../../TestLog.txt");

                //Act
                bool result = logger.Log(data);

                //Assert
                Assert.AreEqual(expected, result);
            }
        }


    }
}