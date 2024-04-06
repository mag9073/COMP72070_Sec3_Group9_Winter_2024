using Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography.X509Certificates;
using Server.Implementations;
using Server.DataStructure;
using System.Globalization;
using UserDataManager = Server.Implementations.UserDataManager;
using ParkDataManager = Server.Implementations.ParkDataManager;
using PacketData = Server.DataStructure.PacketData;
using Types = Server.DataStructure.PacketData.Types;
using Header = Server.DataStructure.PacketData.Header;
using Body = Server.DataStructure.PacketData.Body;
using System.Text;
using static Server.DataStructure.PacketData;
using ServerStateManager = Server.Implementations.ServerStateManager;
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
            private Server.Implementations.ParkDataManager _parkDataManager;

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
            public void UT_SVR_041_ReadOneParkDataFromFile_Return_CorrectParkData()
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
                ParkData? actualResult = _parkDataManager.ReadOneParkDataFromFile(_testGoodFilePath, "Kitchener Park");

                // Assert
                Assert.IsNotNull(actualResult, "Expected not null Park Data object");
                Assert.AreEqual(expectedResult.parkName, actualResult.parkName, "Park name doesnot match");
                Assert.AreEqual(expectedResult.parkAddress, actualResult.parkAddress, "Park descriptions doesnt match");
                Assert.AreEqual(expectedResult.parkHours, actualResult.parkHours, "Park hours do not match");
            }

            [TestMethod]
            public void UT_SVR_042_ReadOneParkDataFromFile_WithNoMatchingName_ReturnsNull()
            {
                // Act
                ParkData? result = _parkDataManager.ReadOneParkDataFromFile(_testGoodFilePath, "South Park");

                // Assert
                Assert.IsNull(result, "Expected a null result for a nonexistent park name");
            }

            [TestMethod]
            public void UT_SVR_043_ReadAllParkDataFromFile_ReturnsCorrectParkDataArray()
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
            public void UT_SVR_045_ReadOneParkDataFromFile_NonExistentFile_CatchesException()
            {
                // Arrange
                string parkName = "South Park";

                // Act
                ParkData? result;
                result = _parkDataManager.ReadOneParkDataFromFile(_testNotValidFilePath, parkName);
                // Assert within catch to ensure exception is caught and handled
                Assert.IsNull(result, "Expected result to be null when exception is caught.");
            }

            [TestMethod]
            public void UT_SVR_046_AppendParkDataToFile_Returns_ValidData()
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
            public void UT_SVR_047_AppendParkDataToFile_Returns_InValidData()
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
                }
                catch (Exception ex)
                {
                    Assert.IsNotNull(ex, "An exception should be thrown when attempting to write to an invalid file path.");
                }
            }

            [TestMethod]
            public void UT_SVR_048_DeleteParkData_RemoveSpecificParkData()
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
                Assert.AreEqual(12, remainingLines.Length, "Expected number of lines after deletion is incorrect.");
                Assert.IsFalse(remainingLines.Contains(testData.parkName), "Associated Address should be deleted.");
                Assert.IsFalse(remainingLines.Contains(testData.parkAddress), "Associated Description should be deleted.");
            }

            [TestMethod]
            public void UT_SVR_049_EditAParkDataToFile_Return_UpdatedParkData()
            {
                // Arrange

                // Create a temp file and write to it
                string tempFilePath = Path.GetTempFileName();

                List<ParkData> expectedParkData = new List<ParkData>
                {
                    new ParkData
                    {
                        parkName = "Kitchener Park",
                        parkAddress = "123 Fountain St",
                        parkDescription = "This is Loo Park",
                        parkHours = "9:00 AM - 5:00 PM",
                    },
                    new ParkData
                    {
                        parkName = "Toronto Park",
                        parkAddress = "123 Dundas St",
                        parkDescription = "This is the 6IX Park",
                        parkHours = "Open 24 Hours",
                    }
                };

                StringBuilder testData = new StringBuilder();
                for (int i = 0; i < expectedParkData.Count; i++)
                {
                    testData.AppendLine(expectedParkData[i].parkName);
                    testData.AppendLine(expectedParkData[i].parkAddress);
                    testData.AppendLine(expectedParkData[i].parkDescription);
                    testData.AppendLine(expectedParkData[i].parkHours);
                }

                File.WriteAllText(tempFilePath, testData.ToString());


                ParkData requestToUpdateParkData = new ParkData
                {
                    parkName = "Kitchener Park",
                    parkAddress = "123 Fanta St",
                    parkDescription = "This is a Fantastic Park",
                    parkHours = "Open 24 Hours",
                };

                // Act
                _parkDataManager.EditAParkDataToFile(tempFilePath, requestToUpdateParkData);

                ParkData[] updatedParks = _parkDataManager.ReadAllParkDataFromFile(tempFilePath);
                ParkData updatedPark = updatedParks.FirstOrDefault(p => p.parkName == requestToUpdateParkData.parkName);

                // Assert
                Assert.IsNotNull(updatedPark, "The park was not updated or added correctly.");
                Assert.AreEqual(requestToUpdateParkData.parkAddress, updatedPark.parkAddress, "Park address should be updated.");
                Assert.AreEqual(requestToUpdateParkData.parkDescription, updatedPark.parkDescription, "Park description should be updated.");
                Assert.AreEqual(requestToUpdateParkData.parkHours, updatedPark.parkHours, "Park hours should be updated.");
            }


        }

        [TestClass]
        public class ParkReviewsManagerTests
        {
            private string _tempFilePath;
            private Server.Implementations.ParkReviewManager _parkReviewManager;

            [TestInitialize]
            public void TestInitializer()
            {
                _parkReviewManager = new Server.Implementations.ParkReviewManager();

                // Create a temp file for testing
                _tempFilePath = Path.GetTempFileName();

                // Create the file to contain test data 
                var testData = new StringBuilder();
                testData.AppendLine("ParkName: Waterloo Park");
                testData.AppendLine("Username: Katherine Slattery | ParkRating: 4 | DateOfPosting: 03/08/2024 12:43:08 AM | Review: I have many good memories walking through this park. I like the path around the lake and the trails through the woods. There are some nice flowering trees which are so peaceful to sit under in the summer.");
                testData.AppendLine();
                testData.AppendLine("ParkName: Neverland Park");
                testData.AppendLine("Username: Tester123 | DateOfPosting: 03/08/2024 12:43:08 AM | Review: Great park!");
                testData.AppendLine();

                File.WriteAllText(_tempFilePath, testData.ToString());

            }

            [TestMethod]
            public void UT_SVR_050_ReadAllParkReviewsFromFile_ReturnsCorrectReviews()
            {
                // Arrange
                ParkReviewData[] expectedReviews = new ParkReviewData[]
{
                    new ParkReviewData
                    {
                        ParkName = "Waterloo Park",
                        UserName = "Katherine Slattery",
                        Rating = 4,
                        Review = "I have many good memories walking through this park. I like the path around the lake and the trails through the woods. There are some nice flowering trees which are so peaceful to sit under in the summer.",

                    },
                    new ParkReviewData
                    {
                        ParkName = "Clair Lake Park",
                        UserName = "Barry Smylie",
                        Rating = 3,
                        Review = "The trail doesn't follow the banks of the reservoir.  It is a sports park with swimming pool, tennis courts, and field sports.  There is one access to the water",
                    }
                };

                // Act
                List<ParkReviewData> actualReviews = _parkReviewManager.ReadAllParkReviewsFromFile("../../../Database/ParkReview.txt");

                // Assert
                Assert.IsNotNull(actualReviews);
                Assert.AreEqual(7, actualReviews.Count);

                // Correct first review
                Assert.AreEqual(expectedReviews[0].ParkName, actualReviews[0].ParkName);
                Assert.AreEqual(expectedReviews[0].UserName, actualReviews[0].UserName);
                Assert.AreEqual(expectedReviews[0].Rating, actualReviews[0].Rating);
                Assert.AreEqual(DateTime.ParseExact("03/08/2024 12:43:08 AM", "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture), actualReviews[0].DateOfPosting);
                Assert.AreEqual(expectedReviews[0].Review, actualReviews[0].Review);

                // Correct second review
                Assert.AreEqual(expectedReviews[1].ParkName, actualReviews[1].ParkName);
                Assert.AreEqual(expectedReviews[1].UserName, actualReviews[1].UserName);
                Assert.AreEqual(expectedReviews[1].Rating, actualReviews[1].Rating);
                Assert.AreEqual(DateTime.ParseExact("03/08/2024 12:43:08 AM", "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture), actualReviews[1].DateOfPosting);
                Assert.AreEqual(expectedReviews[1].Review, actualReviews[1].Review);
            }

            [TestMethod]
            public void UT_SVR_052_OverwritteAllParkReview_Return_OverwrittenDataInFile()
            {
                string tempFilePath = Path.GetTempFileName();

                // Arrange
                ParkReviewManager parkReviewManager = new ParkReviewManager();
                List<ParkReviewData> expectedreviews = new List<ParkReviewData>
                {
                    new ParkReviewData
                    {
                        ParkName = "Lakeshore Park",
                        UserName = "Trevor Heywood",
                        Rating = 5,
                        DateOfPosting = new DateTime(2021, 7, 4),
                        Review = "Great little neighbourhood link with a dirt trail and a small creek. Lots of mature trees to provide shade in the summer."
                    },
                    new ParkReviewData
                    {
                        ParkName = "Twin Oaks Park",
                        UserName = "Spongbob123",
                        Rating = 5,
                        DateOfPosting = new DateTime(2024, 9, 7),
                        Review = "Great place to take my puppy for a walk."
                    }
                };

                List<string> expectedLines = new List<string>
                {
                    "ParkName: Lakeshore Park",
                    "Username: Trevor Heywood | ParkRating: 5 | DateOfPosting: 07/04/2021 12:00:00 AM | Review: Great little neighbourhood link with a dirt trail and a small creek. Lots of mature trees to provide shade in the summer.",
                    "",
                    "ParkName: Twin Oaks Park",
                    "Username: Spongbob123 | ParkRating: 5 | DateOfPosting: 09/07/2024 12:00:00 AM | Review: Great place to take my puppy for a walk.",
                    ""
                };

                // Act
                parkReviewManager.OverwriteAllParkReviewsToFile(tempFilePath, expectedreviews);

                // Assert
                string[] actualLines = File.ReadAllLines(tempFilePath);
                for (int i = 0; i < expectedLines.Count; i++)
                {
                    Assert.IsTrue(i < actualLines.Length, $"Missing line {i}: {expectedLines[i]}");
                    Assert.AreEqual(expectedLines[i], actualLines[i], $"Line data {i} do not match.");
                }
            }

            [TestMethod]
            public void UT_SVR_053_DeleteAParkReviewTest()
            {
                // Arrange
                string targetParkName = "Waterloo Park";
                ParkReviewManager parkReviewManager = new ParkReviewManager();

                // Act
                parkReviewManager.DeleteParkReviews(targetParkName, _tempFilePath);

                // Assert
                string[] remainingReviews = File.ReadAllLines(_tempFilePath);

                for (int i = 0; i < remainingReviews.Length; i++)
                {
                    Assert.IsFalse(remainingReviews[i].Contains(targetParkName));
                }
            }

            [TestMethod]
            public void UT_SVR_054_AppendParkReviewDataTest()
            {
                // Arrange
                ParkReviewManager parkReviewManager = new ParkReviewManager();
                ParkReviewData parkReviewData = new ParkReviewData
                {
                    ParkName = "Never Park",
                    UserName = "Katherine Slattery",
                    Rating = 4,
                    DateOfPosting = new DateTime(2024, 2, 29, 10, 30, 50),
                    Review = "The trail doesn't follow the banks of the reservoir. It is a sports park with swimming pool, tennis courts, and field sports. There is one access to the water",
                };

                // Convert them to usable format for assertion
                List<string> expectedComponents = new List<string>
                {
                    $"ParkName: {parkReviewData.ParkName}",
                    $"Username: {parkReviewData.UserName}",
                    $"ParkRating: {parkReviewData.Rating}",
                    $"DateOfPosting: {parkReviewData.DateOfPosting.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)}",
                    $"Review: {parkReviewData.Review}"
                };

                // Act
                parkReviewManager.AppendReviewDataToFile(_tempFilePath, parkReviewData);

                // Assert
                string fileContent = File.ReadAllText(_tempFilePath);

                for (int i = 0; i < expectedComponents.Count; i++)
                {
                    Assert.IsTrue(fileContent.Contains(expectedComponents[i]));
                }


            }

        }

        [TestClass]
        public class UserDataManagerTests
        {
            

            [TestMethod]
            public void UT_SVR_032_PerformLogin_ValidUser_ReturnsSuccessMessage()
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
            public void UT_SVR_033_PerformAdminLogin_ValidAdmin_ReturnsSuccessMessage()
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
            public void UT_SVR_034_PerformSignUp_NewUser_ReturnsSuccessMessage()
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

        [TestClass]
        public class ParkReviewsTests
        {
            private string _tempFilePath;
            private Server.Implementations.ParkReviewManager _parkReviewManager;

            [TestInitialize]
            public void TestInitializer()
            {
                _parkReviewManager = new Server.Implementations.ParkReviewManager();

                // Create a temporary file for testing
                _tempFilePath = Path.GetTempFileName();

                // Populate the file with test data
                var testData = new StringBuilder();
                testData.AppendLine("ParkName: Waterloo Park");
                testData.AppendLine("Username: Katherine Slattery | ParkRating: 4 | DateOfPosting: 03/08/2024 12:43:08 AM | Review: I have many good memories walking through this park. I like the path around the lake and the trails through the woods. There are some nice flowering trees which are so peaceful to sit under in the summer.");
                testData.AppendLine();
                testData.AppendLine("ParkName: Neverland Park");
                testData.AppendLine("Username: Tester123 | DateOfPosting: 03/08/2024 12:43:08 AM | Review: Great park!");
                testData.AppendLine();

                File.WriteAllText(_tempFilePath, testData.ToString());

            }

            [TestMethod]
            public void UT_SVR_050_ReadAllParkReviewsFromFile_ReturnsCorrectReviews()
            {
                // Arrange
                ParkReviewData[] expectedReviews = new ParkReviewData[]
{
                    new ParkReviewData
                    {
                        ParkName = "Waterloo Park",
                        UserName = "Katherine Slattery",
                        Rating = 4,
                        Review = "I have many good memories walking through this park. I like the path around the lake and the trails through the woods. There are some nice flowering trees which are so peaceful to sit under in the summer.",

                    },
                    new ParkReviewData
                    {
                        ParkName = "Clair Lake Park",
                        UserName = "Barry Smylie",
                        Rating = 3,
                        Review = "The trail doesn't follow the banks of the reservoir.  It is a sports park with swimming pool, tennis courts, and field sports.  There is one access to the water",
                    }
                };

                // Act
                List<ParkReviewData> actualReviews = _parkReviewManager.ReadAllParkReviewsFromFile("../../../Database/ParkReview.txt");

                // Assert
                Assert.IsNotNull(actualReviews);
                Assert.AreEqual(6, actualReviews.Count);

                // Correct first review
                Assert.AreEqual(expectedReviews[0].ParkName, actualReviews[0].ParkName);
                Assert.AreEqual(expectedReviews[0].UserName, actualReviews[0].UserName);
                Assert.AreEqual(expectedReviews[0].Rating, actualReviews[0].Rating);
                Assert.AreEqual(DateTime.ParseExact("03/08/2024 12:43:08 AM", "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture), actualReviews[0].DateOfPosting);
                Assert.AreEqual(expectedReviews[0].Review, actualReviews[0].Review);

                // Correct second review
                Assert.AreEqual(expectedReviews[1].ParkName, actualReviews[1].ParkName);
                Assert.AreEqual(expectedReviews[1].UserName, actualReviews[1].UserName);
                Assert.AreEqual(expectedReviews[1].Rating, actualReviews[1].Rating);
                Assert.AreEqual(DateTime.ParseExact("03/08/2024 12:43:08 AM", "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture), actualReviews[1].DateOfPosting);
                Assert.AreEqual(expectedReviews[1].Review, actualReviews[1].Review);
            }

            [TestMethod]
            public void UT_SVR_052_OverwritteAllParkReview_Return_OverwrittenDataInFile()
            {
                string tempFilePath = Path.GetTempFileName();

                // Arrange
                ParkReviewManager parkReviewManager = new ParkReviewManager();
                List<ParkReviewData> expectedreviews = new List<ParkReviewData>
                {
                    new ParkReviewData
                    {
                        ParkName = "Lakeshore Park",
                        UserName = "Trevor Heywood",
                        Rating = 5,
                        DateOfPosting = new DateTime(2021, 7, 4),
                        Review = "Great little neighbourhood link with a dirt trail and a small creek. Lots of mature trees to provide shade in the summer."
                    },
                    new ParkReviewData
                    {
                        ParkName = "Twin Oaks Park",
                        UserName = "Spongbob123",
                        Rating = 5,
                        DateOfPosting = new DateTime(2024, 9, 7),
                        Review = "Great place to take my puppy for a walk."
                    }
                };

                List<string> expectedLines = new List<string>
                {
                    "ParkName: Lakeshore Park",
                    "Username: Trevor Heywood | ParkRating: 5 | DateOfPosting: 07/04/2021 12:00:00 AM | Review: Great little neighbourhood link with a dirt trail and a small creek. Lots of mature trees to provide shade in the summer.",
                    "",
                    "ParkName: Twin Oaks Park",
                    "Username: Spongbob123 | ParkRating: 5 | DateOfPosting: 09/07/2024 12:00:00 AM | Review: Great place to take my puppy for a walk.",
                    ""
                };

                // Act
                parkReviewManager.OverwriteAllParkReviewsToFile(tempFilePath, expectedreviews);

                // Assert
                string[] actualLines = File.ReadAllLines(tempFilePath);
                for (int i = 0; i < expectedLines.Count; i++)
                {
                    Assert.IsTrue(i < actualLines.Length, $"Missing line {i}: {expectedLines[i]}");
                    Assert.AreEqual(expectedLines[i], actualLines[i], $"Line data {i} do not match.");
                }
            }

            [TestMethod]
            public void UT_SVR_053_DeleteAParkReviewTest()
            {
                // Arrange
                string targetParkName = "Waterloo Park";
                ParkReviewManager parkReviewManager = new ParkReviewManager();

                // Act
                parkReviewManager.DeleteParkReviews(targetParkName, _tempFilePath);

                // Assert
                string[] remainingReviews = File.ReadAllLines(_tempFilePath);

                for (int i = 0; i < remainingReviews.Length; i++)
                {
                    Assert.IsFalse(remainingReviews[i].Contains(targetParkName));
                }
            }

            [TestMethod]
            public void UT_SVR_054_AppendParkReviewDataTest()
            {
                // Arrange
                ParkReviewManager parkReviewManager = new ParkReviewManager();
                ParkReviewData parkReviewData = new ParkReviewData
                {
                    ParkName = "Never Park",
                    UserName = "Katherine Slattery",
                    Rating = 4,
                    DateOfPosting = new DateTime(2024, 2, 29, 10, 30, 50),
                    Review = "The trail doesn't follow the banks of the reservoir. It is a sports park with swimming pool, tennis courts, and field sports. There is one access to the water",
                };

                // Convert them to usable format for assertion
                List<string> expectedComponents = new List<string>
                {
                    $"ParkName: {parkReviewData.ParkName}",
                    $"Username: {parkReviewData.UserName}",
                    $"ParkRating: {parkReviewData.Rating}",
                    $"DateOfPosting: {parkReviewData.DateOfPosting.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)}",
                    $"Review: {parkReviewData.Review}"
                };

                // Act
                parkReviewManager.AppendReviewDataToFile(_tempFilePath, parkReviewData);

                // Assert
                string fileContent = File.ReadAllText(_tempFilePath);

                for (int i = 0; i < expectedComponents.Count; i++)
                {
                    Assert.IsTrue(fileContent.Contains(expectedComponents[i]));
                }


            }
        }

        [TestClass]
        public class PacketDataTests
        {
            [TestMethod]
            public void UT_SVR_057_SetHeaderSourceID_Return_Valid_SourceID()
            {
                // Arrange
                byte expectedSourceID = 1;
                Header header = new Header();

                // Act
                header.SetHeaderSourceID(expectedSourceID); 
                byte actualSourceID = header.sourceID;

                // Assert
                Assert.AreEqual(expectedSourceID, actualSourceID);

            }

            [TestMethod]
            public void UT_SVR_058_SetDestinationID_Return_Valid_DestinationID()
            {
                // Arrange
                byte expectedDestinationID = 1;
                Header header = new Header();

                // Act
                header.SetHeaderDestinationID(expectedDestinationID);
                byte actualDestinationID = header.destinationID;

                // Assert
                Assert.AreEqual(expectedDestinationID, actualDestinationID);
            }

            [TestMethod]
            public void UT_SVR_059_SetHeaderSequenceNumber_Return_ValidSeqID()
            {
                // Arrange
                uint expectedSequenceNumber = 1;
                Header header = new Header();

                // Act
                header.SetHeaderSequenceNumber(expectedSequenceNumber);
                uint actualSequenceNumber = header.sequenceNumber;

                // Assert
                Assert.AreEqual(expectedSequenceNumber, actualSequenceNumber);
            }

            [TestMethod]
            public void UT_SVR_060_SetHeaderBodyLength_Return_ValidBodyLength()
            {
                // Arrange
                uint expectedBodyLength = 8;
                Header header = new Header();

                // Act
                header.SetHeaderBodyLength(expectedBodyLength);
                uint actualBodyLength = header.GetHeaderBodyLength();

                // Assert
                Assert.AreEqual(expectedBodyLength, actualBodyLength);
            }


            [TestMethod]
            public void UT_SVR_061_GetHeaderSourceID_Return_ValidSourceID()
            {
                // Arrange
                byte expectedSourceID = 2;
                Header header = new Header();

                // Act
                header.SetHeaderSourceID(expectedSourceID);
                uint actualSourceID = header.GetHeaderSourceID();

                // Assert
                Assert.AreEqual(expectedSourceID, actualSourceID);
            }

            [TestMethod]
            public void UT_SVR_062_GetHeaderDestinationID_Return_ValidDestinationID()
            {
                // Arrange
                byte expectedDestinationID = 2;
                Header header = new Header();

                // Act
                header.SetHeaderDestinationID(expectedDestinationID);
                uint actualDestinationID = header.GetHeaderDestinationID();

                // Assert
                Assert.AreEqual(expectedDestinationID, actualDestinationID);
            }

            [TestMethod]
            public void UT_SVR_063_GetHeaderSequenceNumber_Return_ValidSequenceNumber()
            {
                // Arrange
                byte expectedSequenceNumber = 2;
                Header header = new Header();

                // Act
                header.SetHeaderSequenceNumber(expectedSequenceNumber);
                uint actualSequenceNumber = header.GetHeaderSequenceNumber();

                // Assert
                Assert.AreEqual(expectedSequenceNumber, actualSequenceNumber);
            }

            [TestMethod]
            public void UT_SVR_064_GetHeaderBodyLength_Return_ValidBodyLength()
            {
                // Arrange
                uint expectedBodyLength = 10;
                Header header = new Header();

                // Act
                header.SetHeaderBodyLength(expectedBodyLength);
                uint actualBodyLength = header.GetHeaderBodyLength();

                // Assert
                Assert.AreEqual(expectedBodyLength, actualBodyLength);
            }


            [TestMethod]
            public void UT_SVR_065_SetHeaderType_Log_Return_ValidHeaderType_Log()
            {
                // Arrange
                Types expectedHeaderType = Types.log;
                Header header = new Header();

                // Act
                header.SetType(expectedHeaderType);
                Types actualHeaderType = header.type;

                // Assert
                Assert.AreEqual(expectedHeaderType, actualHeaderType);
            }


            [TestMethod]
            public void UT_SVR_066_GetHeaderType_Register_Return_ValidHeaderType_Register()
            {
                // Arrange
                Types expectedHeaderType = Types.register;
                Header header = new Header();

                // Act
                header.SetType(expectedHeaderType);
                Types actualHeaderType = header.GetType();

                // Assert
                Assert.AreEqual(expectedHeaderType, actualHeaderType);
            }

            [TestMethod]
            public void UT_SVR_067_SetBodyBuffer_Return_Valid_BodyBuffer()
            {
                // Arrange
                UserDataManager.LoginData loginData = new UserDataManager.LoginData
                {
                    username = "mino",
                    password = "12345",
                };

                Body body = new Body();
                byte[] expectedBodyBuffer = loginData.SerializeToByteArray();

                // Act 
                body.SetBodyBuffer(expectedBodyBuffer);
                byte[] actualBodyBuffer = body.buffer;

                // Assert
                Assert.AreEqual(expectedBodyBuffer, actualBodyBuffer);
            }

            [TestMethod]
            public void UT_SVR_068_GetBodyBuffer_Return_Valid_BodyBuffer()
            {
                // Arrange
                UserDataManager.LoginData loginData = new UserDataManager.LoginData
                {
                    username = "hang",
                    password = "1234",
                };

                Body body = new Body();
                byte[] expectedBodyBuffer = loginData.SerializeToByteArray();

                // Act 
                body.SetBodyBuffer(expectedBodyBuffer);
                byte[] actualBodyBuffer = body.GetBodyBuffer();

                // Assert
                Assert.AreEqual(expectedBodyBuffer, actualBodyBuffer);
            }

            [TestMethod]
            public void UT_SVR_069_SetPacketHead_Return_Valid_PacketHead()
            {
                // Arrange
                Packet packet = new Packet();
                byte expectedSourceID = 1;
                byte expectedDestinationID = 2;
                Types expectedType = Types.review;

                // Act
                packet.SetPacketHead(expectedSourceID, expectedDestinationID, expectedType);
                byte actualSourceID = packet.GetPacketHeader().sourceID;
                byte actualDestinationID = packet.GetPacketHeader().destinationID;
                Types actualType = packet.GetPacketHeader().type;

                // Assert
                Assert.AreEqual(expectedSourceID, actualSourceID);
                Assert.AreEqual(expectedDestinationID, actualDestinationID);
                Assert.AreEqual(expectedType, actualType);
            }

            [TestMethod]
            public void UT_SVR_070_GetPacketHead_Return_Valid_PacketHead()
            {
                // Arrange
                Packet packet = new Packet();
                byte expectedSourceID = 1;
                byte expectedDestinationID = 2;
                Types expectedType = Types.review;

                // Act
                packet.SetPacketHead(expectedSourceID, expectedDestinationID, expectedType);
                Header actualHeader = packet.GetPacketHeader();
                byte actualSourceID = actualHeader.GetHeaderSourceID();
                byte actualDestinationID = actualHeader.GetHeaderDestinationID();
                Types actualType = actualHeader.GetType();

                // Assert
                Assert.AreEqual(expectedSourceID, actualSourceID);
                Assert.AreEqual(expectedDestinationID, actualDestinationID);
                Assert.AreEqual(expectedType, actualType);
            }

            [TestMethod]
            public void UT_SVR_074_DeserializeFromByteArray_Return_ValidPacket()
            {
                // Arrange
                PacketData.Packet expectedPacket = new PacketData.Packet();
                expectedPacket.SetPacketHead(1, 2, PacketData.Types.login);
                string testMessage = "Good Morning, Vietnam!";
                byte[] testBody = Encoding.UTF8.GetBytes(testMessage);
                expectedPacket.SetPacketBody(testBody, (uint)testBody.Length);
                byte[] serializedPacket = expectedPacket.SerializeToByteArray();

                // Act
                Packet actualPacket = PacketData.Packet.DeserializeFromByteArray(serializedPacket);


                // Arrange

                // Test everything between the two packets 
                Assert.IsNotNull(actualPacket);
                Assert.AreEqual(expectedPacket.GetPacketHeader().sourceID, actualPacket.GetPacketHeader().sourceID);
                Assert.AreEqual(expectedPacket.GetPacketHeader().destinationID, actualPacket.GetPacketHeader().destinationID);
                Assert.AreEqual(expectedPacket.GetPacketHeader().type, actualPacket.GetPacketHeader().type);
                Assert.AreEqual(expectedPacket.GetPacketHeader().bodyLength, actualPacket.GetPacketHeader().bodyLength);
                Assert.AreEqual(testMessage, Encoding.UTF8.GetString(actualPacket.GetBody().GetBodyBuffer()));
            }

            [TestMethod]
            public void UT_SVR_076_SetParkName_Return_Valid_ParkName()
            {
                // Arrange
                ParkData parkData = new ParkData();
                string expectedParkName = "Waterloo Park";


                // Act
                parkData.SetParkName(expectedParkName);
                string actualParkName = parkData.GetParkName();

                // Arrange
                Assert.AreEqual(expectedParkName, actualParkName);   
            }

            [TestMethod]
            public void UT_SVR_078_SetParkAddress_Return_Valid_ParkName()
            {
                // Arrange
                ParkData parkData = new ParkData();
                string expectedParkAddress = "123 Liverpool Street, Manchester, UK";


                // Act
                parkData.SetParkAddress(expectedParkAddress);
                string actualParkAddress = parkData.GetParkAddress();

                // Arrange
                Assert.AreEqual(expectedParkAddress, actualParkAddress);
            }

            [TestMethod]
            public void UT_SVR_080_SetParkDescription_Return_Valid_ParkDescription()
            {
                // Arrange
                ParkData parkData = new ParkData();
                string expectedParkDescription = "This is Liverpool Park, it's full of trash!";


                // Act
                parkData.SetParkDescription(expectedParkDescription);
                string actualParkDescription = parkData.GetParkDescription();

                // Arrange
                Assert.AreEqual(expectedParkDescription, actualParkDescription);
            }

            [TestMethod]
            public void UT_SVR_082_SetParkHours_Return_Valid_ParkHours()
            {
                // Arrange
                ParkData parkData = new ParkData();
                string expectedHours = "Open 24 Hours";


                // Act
                parkData.SetParkHours(expectedHours);
                string actualParkHours = parkData.GetParkHours();

                // Arrange
                Assert.AreEqual(expectedHours, actualParkHours);
            }

            [TestMethod]
            public void UT_SVR_084_DeserializeParkData_Return_Valid_ParkData()
            {
                // Arrange
                ParkData expectedParkData = new ParkData
                {
                    parkName = "Stanley Park",
                    parkAddress = "123 Stan Ave, Vancouver, BC",
                    parkDescription = "Stanley Park is a world renowned park and tourist attraction in Vancouver, Canada. ",
                    parkHours = "Open 24 Hours",
                };


                // Act
                byte[] serializedParkData = expectedParkData.SerializeToByteArray();


                ParkData deserializedParkData = new ParkData().deserializeParkData(serializedParkData);


                // Assert
                Assert.IsNotNull(serializedParkData);
                Assert.IsNotNull(deserializedParkData);

                Assert.AreEqual(expectedParkData.parkName, deserializedParkData.parkName);
                Assert.AreEqual(expectedParkData.parkAddress, deserializedParkData.parkAddress);
                Assert.AreEqual(expectedParkData.parkDescription, deserializedParkData.parkDescription);
                Assert.AreEqual(expectedParkData.parkHours, deserializedParkData.parkHours);
            }


            [TestMethod]
            public void UT_SVR_086_SetUserName_Return_Valid_UserName()
            {
                // Arrange
                ParkReviewData parkReviewData = new ParkReviewData();
                string expectedUserName = "madiera";


                // Act
                parkReviewData.SetUserName(expectedUserName);
                string actualUserName = parkReviewData.GetUserName();

                // Arrange
                Assert.AreEqual(expectedUserName, actualUserName);  
            }

            [TestMethod]
            public void UT_SVR_088_SetParkRating_Return_Valid_UserName()
            {
                // Arrange
                ParkReviewData parkReviewData = new ParkReviewData();
                float expectedParkRating = 4;


                // Act
                parkReviewData.SetParkRating(expectedParkRating);
                float actualParkRating = parkReviewData.GetParkRating();

                // Arrange
                Assert.AreEqual(expectedParkRating, actualParkRating);
            }

            [TestMethod]
            public void UT_SVR_090_SetDateOfPosting_Return_Valid_DateOfPosting()
            {
                // Arrange
                ParkReviewData parkReviewData = new ParkReviewData();
                DateTime expectedDateOfPosting = new DateTime(2024, 2, 29, 10, 30, 50);

                // Act
                parkReviewData.SetDateOfPosting(expectedDateOfPosting);
                DateTime actualDateOfPosting = parkReviewData.GetDateOfPosting();

                // Arrange
                Assert.AreEqual(expectedDateOfPosting, actualDateOfPosting);
            }

            [TestMethod]
            public void UT_SVR_092_SetParkReview_Return_Valid_ParkReview()
            {
                // Arrange
                ParkReviewData parkReviewData = new ParkReviewData();
                string expectedParkReview = "This park is a hidden gem. Highly Recommended!!!";

                // Act
                parkReviewData.SetReview(expectedParkReview);
                string actualParkReview = parkReviewData.GetReview();

                // Arrange
                Assert.AreEqual(expectedParkReview, actualParkReview);
            }

            [TestMethod]
            public void UT_SVR_093_SerializeToByteArray_Return_ValidSerializeData()
            {
                // Arrange
                ParkReviewData expectedReviewData = new ParkReviewData
                {
                    ParkName = "Cornerbrook Park",
                    UserName = "Karen Freiburger",
                    Rating = 5,
                    DateOfPosting = new DateTime(2024, 2, 29, 10, 30, 50),
                    Review = "For the winter it has a great little sledding hill and the skating rink is very well maintained! Also a newer play structure and swings for all seasons complete the fun!",
                };

                // Convert to ByteArray
                byte[] serializeParkReviewData = expectedReviewData.SerializeToByteArray();


                // Act
                ParkReviewData actualReviewData = new ParkReviewData();

                actualReviewData = actualReviewData.deserializeParkReviewData(serializeParkReviewData);

                // Assert
                Assert.IsNotNull(serializeParkReviewData);
                Assert.IsNotNull(actualReviewData);
                Assert.AreEqual(expectedReviewData.ParkName, actualReviewData.ParkName);
                Assert.AreEqual(expectedReviewData.UserName, actualReviewData.UserName);
                Assert.AreEqual(expectedReviewData.Rating, actualReviewData.Rating);
                Assert.AreEqual(expectedReviewData.DateOfPosting, actualReviewData.DateOfPosting);
                Assert.AreEqual(expectedReviewData.Review, actualReviewData.Review);


            }

            [TestMethod]
            public void UT_SVR_094_DeserializeParkReviewData_Return_Valid_ParkReviewData()
            {

                // Arrange
                ParkReviewData expectedReviewData = new ParkReviewData
                {
                    ParkName = "South Park",
                    UserName = "David",
                    Rating = 4,
                    DateOfPosting = new DateTime(2024, 2, 29, 10, 30, 50),
                    Review = "The trail doesn't follow the banks of the reservoir. It is a sports park with swimming pool, tennis courts, and field sports. There is one access to the water",
                };

                // Convert to ByteArray
                byte[] serializeParkReviewData = expectedReviewData.SerializeToByteArray();


                // Act
                ParkReviewData actualReviewData = new ParkReviewData();

                actualReviewData = actualReviewData.deserializeParkReviewData(serializeParkReviewData);

                // Assert
                Assert.AreEqual(expectedReviewData.ParkName, actualReviewData.ParkName);
                Assert.AreEqual(expectedReviewData.UserName, actualReviewData.UserName);
                Assert.AreEqual(expectedReviewData.Rating, actualReviewData.Rating);
                Assert.AreEqual(expectedReviewData.DateOfPosting, actualReviewData.DateOfPosting);
                Assert.AreEqual(expectedReviewData.Review, actualReviewData.Review);


            }


            [TestMethod]
            public void UT_SVR_095_PacketConstructor_Return_DeserializeData()
            {
                // Arrange
                PacketData.Packet expectedPacketData = new PacketData.Packet();
                expectedPacketData.SetPacketHead(1, 2, Types.a_park);
                string expectedBody = "Hello World";

                byte[] expectedBodyArray = Encoding.UTF8.GetBytes(expectedBody);

                expectedPacketData.SetPacketBody(expectedBodyArray, (uint)expectedBody.Length);
                byte[] sendPacket = expectedPacketData.SerializeToByteArray();

                // Act
                PacketData.Packet actualPacketData = new PacketData.Packet(sendPacket);




                // Assert
                Assert.AreEqual(expectedPacketData.GetPacketHeader().sourceID, actualPacketData.GetPacketHeader().sourceID);
                Assert.AreEqual(expectedPacketData.GetPacketHeader().destinationID, actualPacketData.GetPacketHeader().destinationID);
                Assert.AreEqual(expectedPacketData.GetPacketHeader().sequenceNumber, actualPacketData.GetPacketHeader().sequenceNumber);
                Assert.AreEqual(expectedPacketData.GetPacketHeader().bodyLength, actualPacketData.GetPacketHeader().bodyLength);
                Assert.AreEqual(expectedPacketData.GetPacketHeader().type, actualPacketData.GetPacketHeader().type);

                Assert.AreEqual(expectedPacketData.GetBody().buffer.ToString(), actualPacketData.GetBody().buffer.ToString());
            }


        }

        [TestClass]
        public class ServerTests
        {
            [TestMethod]
            public void UT_SVR_056_StartServerTest()
            {
                // Arrange 
                int portNumber = 13000;
                bool expectedState = true;
                bool actualState = false;

                // Act
                Server.Implementations.Server server = new Server.Implementations.Server();
                actualState = server._isRunning;
                server.StartServer(portNumber);

                Thread.Sleep(1000);

                // Assert
                Assert.IsTrue(expectedState);

                server.StopServer();
            }
        }

        [TestClass]
        public class ServerStateManager
        {
            [TestMethod]
            public void UT_SVR_096_SetCurrentState_Starting_Return_StartingState()
            {
                // Arrange
                Server.Implementations.ServerStateManager serverStateManager = new Server.Implementations.ServerStateManager();

                ServerState expectedState = ServerState.Starting;

                ServerState actualState;

                // Act
                serverStateManager.SetCurrentState(expectedState);

                actualState = serverStateManager.GetCurrentState();           
                
                
                // Assert
                Assert.AreEqual(expectedState, actualState);



            }

            [TestMethod]
            public void UT_SVR_097_SetCurrentState_Login_Return_LoginState()
            {
                // Arrange
                Server.Implementations.ServerStateManager serverStateManager = new Server.Implementations.ServerStateManager();

                ServerState expectedState = ServerState.Login;

                ServerState actualState;

                // Act
                serverStateManager.SetCurrentState(ServerState.Login);

                actualState = serverStateManager.GetCurrentState();


                // Assert
                Assert.AreEqual(expectedState, actualState);
            }

            [TestMethod]
            public void UT_SVR_098_SetCurrentState_AllParkData_Return_AllParkDataState()
            {
                // Arrange
                Server.Implementations.ServerStateManager serverStateManager = new Server.Implementations.ServerStateManager();

                ServerState expectedState = ServerState.AllParkData;

                ServerState actualState;

                // Act
                serverStateManager.SetCurrentState(ServerState.AllParkData);

                actualState = serverStateManager.GetCurrentState();


                // Assert
                Assert.AreEqual(expectedState, actualState);
            }

            [TestMethod]
            public void UT_SVR_099_SetCurrentState_Connected_Return_ConnectedState()
            {
                // Arrange
                Server.Implementations.ServerStateManager serverStateManager = new Server.Implementations.ServerStateManager();

                ServerState expectedState = ServerState.Connected;

                ServerState actualState;

                // Act
                serverStateManager.SetCurrentState(expectedState);

                actualState = serverStateManager.GetCurrentState();


                // Assert
                Assert.AreEqual(expectedState, actualState);



            }

            [TestMethod]
            public void UT_SVR_100_SetCurrentState_Idle_Return_IdleState()
            {
                // Arrange
                Server.Implementations.ServerStateManager serverStateManager = new Server.Implementations.ServerStateManager();

                ServerState expectedState = ServerState.Idle;

                ServerState actualState;

                // Act
                serverStateManager.SetCurrentState(expectedState);

                actualState = serverStateManager.GetCurrentState();


                // Assert
                Assert.AreEqual(expectedState, actualState);



            }

            [TestMethod]
            public void UT_SVR_101_SetCurrentState_Idle_OneParkImage_Return_Idle_OneParkImage_State()
            {
                // Arrange
                Server.Implementations.ServerStateManager serverStateManager = new Server.Implementations.ServerStateManager();

                ServerState expectedState_One = ServerState.Idle;

                ServerState actualState_One;

                ServerState expectedState_Two = ServerState.ParkReview;

                ServerState actualState_Two;

                // Act
                serverStateManager.SetCurrentState(expectedState_One);

                actualState_One = serverStateManager.GetCurrentState();

                serverStateManager.SetCurrentState(expectedState_Two);

                actualState_Two = serverStateManager.GetCurrentState();


                // Assert
                Assert.AreEqual(expectedState_One, actualState_One);
                Assert.AreEqual(expectedState_Two, actualState_Two);


            }
        }
    }
}