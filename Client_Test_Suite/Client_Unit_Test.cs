//using Client;
using LogiPark.MVVM.View;
using LogiPark.MVVM.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Net.Mime.MediaTypeNames.Application;
using System.Windows.Controls;
using LogiPark.MVVM.Model;
using System.Windows.Media.Imaging;
using System.Net.Sockets;
using NUnit.Framework;

namespace Client_Test_Suite
{
    [TestClass]
    public class Client_Unit_Test
    {
        [TestClass]
        public class  Park_Data_Manager_Tests
        {

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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expectedName, parkData.parkName);
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expectedAddress, parkData.parkAddress);
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expectedDescription, parkData.parkDescription);
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expectedHours, parkData.parkHours);
            }

            [TestMethod]
            public void UT_CL_PDM_009() // true negative
            {
                //Arrange
                string expected = "Apple Park";

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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }


            [TestMethod]
            public void UT_CL_PDM_010() // true negative 
            {
                //Arrange
                string expected = "191 king street";

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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }


            [TestMethod]
            public void UT_CL_PDM_011() // true negative
            {
                //Arrange
                string expected = "descripton";

                //Act
                ParkDataManager.ParkData park = new ParkDataManager.ParkData
                {
                    parkName = "Waterloo Park",
                    parkAddress = "101 king street ",
                    parkDescription = "Description",
                    parkHours = "7am - 12am"
                };

                string result = park.GetParkDescription();

                //Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }


            [TestMethod]
            public void UT_CL_PDM_012() // true negative
            {
                //Arrange
                string expected = "7am-11pm";

                //Act
                ParkDataManager.ParkData park = new ParkDataManager.ParkData
                {
                    parkName = "Waterloo Park",
                    parkAddress = "101 king street ",
                    parkDescription = "Description",
                    parkHours = "7am - 12am"
                };

                string result = park.GetParkHours();

                //Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }
        }

        [TestClass]
        public class Park_Review_Manager_Tests
        {

            [TestMethod]
            public void UT_CL_PRM_001() // true positive
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void UT_CL_PRM_002() // true negative
            {
                //Arrange
                var expected = "Frank smith";

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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }



            [TestMethod]
            public void UT_CL_PRM_003() // true positive
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void UT_CL_PRM_004() // true negative
            {
                //Arrange
                var expected = 1f;

                //Act
                ParkReviewManager.ParkReviewData reviewData = new ParkReviewManager.ParkReviewData
                {
                    ParkName = "Waterloo Park",
                    UserName = "bob smith",
                    Rating = 4f,
                    DateOfPosting = new DateTime(2020, 02, 20),
                    Review = "nice park, clean."
                };

                var result = reviewData.GetParkRating();

                //Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void UT_CL_PRM_005() // true positive
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void UT_CL_PRM_006() // true negative
            {
                //Arrange
                var expected = new DateTime(2020, 02, 29);

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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void UT_CL_PRM_007() // true positive
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void UT_CL_PRM_008() // true negative
            {
                //Arrange
                var expected = "clean.";

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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void UT_CL_PRM_009() // true positive
            {
                //Arrange
                var review = new ParkReviewManager.ParkReviewData();
                string expectedName = "jown dow";

                review.SetUserName(expectedName);

                //Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expectedName, review.UserName);
            }

            [TestMethod]
            public void UT_CL_PRM_010() // true positive
            {
                //Arrange
                var review = new ParkReviewManager.ParkReviewData();
                var expectedRating = 3f;

                review.SetParkRating(expectedRating);

                //Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expectedRating, review.Rating);
            }

            [TestMethod]
            public void UT_CL_PRM_011() // true positive
            {
                //Arrange
                var review = new ParkReviewManager.ParkReviewData();
                var expectedTime = new DateTime(2024, 01, 07);

                review.SetDateOfPosting(expectedTime);

                //Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expectedTime, review.DateOfPosting);
            }

            [TestMethod]
            public void UT_CL_PRM_012() // true positive
            {
                //Arrange
                var review = new ParkReviewManager.ParkReviewData();
                string expectedReview = "decent park cant complain";

                review.SetReview(expectedReview);

                //Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expectedReview, review.Review);
            }
        }

        [TestClass]
        public class User_Data_Manager_Tests
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void UT_CL_UDM_005()
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void UT_CL_UDM_006()
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void UT_CL_UDM_007()
            {
                // Arrange

                // Act

                // Assert
            }

            [TestMethod]
            public void UT_CL_UDM_008()
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void UT_CL_UDM_009()
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void UT_CL_UDM_010()
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void UT_CL_UDM_011()
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void UT_CL_UDM_012()
            {
                // Arrange

                // Act

                // Assert
            }

            [TestMethod]
            public void UT_CL_UDM_013()
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }

            [TestMethod]
            public void UT_CL_UDM_014()
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
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expected, result);
            }
        }

        [TestClass]
        public class Image_Manager_Tests
        {
            [TestMethod]
            public void UT_CN_IT_001()
            {
               
                // Arrange
                string fileName = "background.png";
                byte[] data = File.ReadAllBytes("background.png");

                // Act
                ImageManager imageManager = new ImageManager()
                {
                    FileName = fileName,
                    Data = data
                };

                // Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(fileName, imageManager.FileName);
                CollectionAssert.AreEqual(data, imageManager.Data);
            
            }

            [TestMethod]
            public void UT_CN_IT_002()
            {
                // Arrange
                string fileName = "background.png";
                byte[] data = File.ReadAllBytes("background.png");
                BitmapImage image = new BitmapImage();

                // Act
                ImageManager imageManager = new ImageManager()
                {
                    FileName = fileName,
                    Data = data,
                    Image = image
                };

                // Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(imageManager.Image);
                //Assert.AreEqual(image, imageManager.Image);
            }

            [TestMethod]
            public void UT_CN_IM_003()
            {
                // Arrange
                string fileName = "background.png";
                byte[] data = File.ReadAllBytes("background.png");

                // Act
                ImageManager imageManager = new ImageManager()
                {
                    FileName = fileName,
                    Data = data,
                    Image = null
                };

                // Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNull(imageManager.Image);
            }
        }

        [TestClass]
        public class TCP_Connection_Manager_Tests
        {
            
            [TestMethod]
            public void UT_CN_TCPCM_001()
            {
                // Arrange
                var intance = TcpConnectionManager.Instance;

                // Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(intance);
            }

            [TestMethod]
            public void UT_CN_TCPCM_002()
            {
                // Arrange
                var TCPmanager = TcpConnectionManager.Instance;

                // Act
                //TCPmanager();

                // Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(TCPmanager.client);
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(TCPmanager.client.Connected);
            }

            [TestMethod]
            public void UT_CN_TCPCM_003()
            {
                // Arrange
                var TCPmanager = TcpConnectionManager.Instance;

                // Act
                TCPmanager.CloseConnection();

                // Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(TCPmanager.client.Connected);

            }
        }

        [TestClass]
        public class Admin_Add_Park_View_Model_Tests
        {
            private bool IsPropertyRaised(AdminAddParkViewModel viewModel, string propertyName)
            {
                var eventRaised = false;
                viewModel.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == propertyName)
                    {
                        eventRaised = true;
                    }
                };
                return eventRaised;
            }

            [TestMethod]
            public void UT_CN_AAPVM_001()
            {
                var viewModel = new AdminAddParkViewModel();

                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(viewModel.CurrentView);
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType<LogiPark.MVVM.View.AdminAddParkViewPage>(viewModel.CurrentView);
            }

            [TestMethod]
            public void UT_CN_AAPVM_002()
            {
                var viewModel = new AdminAddParkViewModel();
                var newParkVm = new AdminAddParkViewModel();
                var propertyRaised = IsPropertyRaised(viewModel, nameof(AdminAddParkViewModel.ParkVm));

                viewModel.ParkVm = newParkVm;

                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(propertyRaised);
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(newParkVm, viewModel.ParkVm);
            }

            [TestMethod]
            public void UT_CN_AAPVM_003()
            {
                var viewModel = new AdminAddParkViewModel();
                var newView = new object();
                var propertyRaised = IsPropertyRaised(viewModel, nameof(AdminAddParkViewModel.CurrentView));

                viewModel.CurrentView = newView;

                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(propertyRaised);
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(newView, viewModel.CurrentView);
            }
        }

        [TestClass]
        public class Admin_Home_View_Model_Tests
        {
            [SetUp]
            public void SetUp()
            {
                Assembly.Load("PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

                
                // Assembly.Load("AnotherAssemblyName, Version=x.x.x.x, Culture=neutral, PublicKeyToken=yourPublicKeyTokenHere");
            }
            [TestMethod]
            public void UT_CN_AHVM_001()
            {
                var viewModel = new AdminHomeViewModel();

                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(viewModel.CurrentView);
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType<LogiPark.MVVM.View.AdminHomeViewPage>(viewModel.CurrentView);
            }

            [TestMethod]
            public void UT_CN_AHNM_002()
            {
                //  var viewModel = new AdminHomeViewModel();
                //  var testParkVm = new AdminHomeViewModel(); 

                //  viewModel.ParkVm = testParkVm;

                //  Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(testParkVm, viewModel.ParkVm);
                // Arrange
                var model = new AdminHomeViewModel();
                bool propertyChangedWasRaised = false;

               
                model.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(model.CurrentView))
                        propertyChangedWasRaised = true;
                };

                // Act
                model.CurrentView = new object(); 

                // Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(propertyChangedWasRaised);
            }
        }
    }
}