using LogiPark.MVVM.Model;
using LogiPark.MVVM.View;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Net.Mime.MediaTypeNames.Application;
using System.Windows.Controls;
//using Client;

namespace Client_Test_Suite
{
    [TestClass]
    public class Client_Unit_Test
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
            Assert.AreEqual(expected, result);
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
            Assert.AreEqual(expected, result);
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
            Assert.AreEqual(expected, result);
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
            Assert.AreEqual(expected, result);
        }

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
            Assert.AreEqual(expected, result);
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
            Assert.AreEqual(expected, result);
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
            Assert.AreEqual(expected, result);
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
                DateOfPosting = new DateTime(2020,02,20),
                Review = "nice park, clean."
            };

            var result = reviewData.GetParkRating();

            //Assert
            Assert.AreEqual(expected, result);
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
            Assert.AreEqual(expected, result);
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
            Assert.AreEqual(expected, result);
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
            Assert.AreEqual(expected, result);
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
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UT_CL_PRM_009() // true positive
        {
            //Arrange
            var review = new ParkReviewManager.ParkReviewData();
            string expectedName = "jown dow";

            review.SetUserName(expectedName);

            //Assert
            Assert.AreEqual(expectedName, review.UserName);
        }

        [TestMethod]
        public void UT_CL_PRM_010() // true positive
        {
            //Arrange
            var review = new ParkReviewManager.ParkReviewData();
            var expectedRating = 3f;

            review.SetParkRating(expectedRating);

            //Assert
            Assert.AreEqual(expectedRating, review.Rating);
        }

        [TestMethod]
        public void UT_CL_PRM_011() // true positive
        {
            //Arrange
            var review = new ParkReviewManager.ParkReviewData();
            var expectedTime = new DateTime(2024, 01, 07);

            review.SetDateOfPosting(expectedTime);

            //Assert
            Assert.AreEqual(expectedTime, review.DateOfPosting);
        }

        [TestMethod]
        public void UT_CL_PRM_012() // true positive
        {
            //Arrange
            var review = new ParkReviewManager.ParkReviewData();
            string expectedReview = "decent park cant complain";

            review.SetReview(expectedReview);

            //Assert
            Assert.AreEqual(expectedReview, review.Review);
        }
    }
}