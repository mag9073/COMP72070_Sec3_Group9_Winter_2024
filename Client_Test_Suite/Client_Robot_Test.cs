using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Specialized;
using System.Xml.Linq;


namespace Client_Test_Suite
{
    [TestClass]
    public class Client_Robot_Tests
    {
        const int OPENED_WINDOW = 0;

        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        //******************************************************************************
        //*********************** RUN THE SERVER WHEN TESTING!!! *********************** 
        //******************************************************************************



        //************************ LOGIN TESTS ************************
        [TestMethod]
        public void Robot_Test_ClientSuccussfulLogin()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path);
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("hang");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);


            // if the ClientHomeView window name is found, that means the login was successful
            string actual = winDriver.FindElementByName("ClientHomeView (hang)").Text;



            // Assert
            Assert.AreEqual("ClientHomeView (hang)", actual);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void Robot_Test_ClientFailureLogin()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("abcd");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // if the ClientHomeView window name isnt found, that means the login failed
            string actual = winDriver.FindElementByName("Login").Text;



            // Assert
            Assert.AreNotEqual("ClientHomeView", actual);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void Robot_Test_AdminSuccussfulLogin()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // if the AdminHomeView window name is found, that means the login was successful
            string actual = winDriver.FindElementByName("AdminHomeView").Text;



            // Assert
            Assert.AreEqual("AdminHomeView", actual);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void Robot_Test_AdminFailureLogin()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("abc");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // if the AdminHomeView window name isnt found, that means the login failed
            string actual = winDriver.FindElementByName("Login").Text;



            // Assert
            Assert.AreNotEqual("AdminHomeView", actual);

            winDriver.CloseApp();
        }

        //************************ LOGIN TESTS ************************




        //************************ SIGNUP TESTS ************************
        [TestMethod]
        public void Robot_Test_ClientSuccussfulSignup()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);


            
            // Act
            winDriver.FindElementByName("Sign Up").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("abcde");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("12345");
            winDriver.FindElementByName("Sign Up").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // if the Login button name is found, that means the signup was successful
            string actual = winDriver.FindElementByName("Login").Text;



            // Assert
            Assert.AreEqual("Login", actual);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void Robot_Test_ClientFailureSignup()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByName("Sign Up").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // not typing anything into username should cuase a fail
            //winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("");

            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Sign Up").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // if the Login button name isnt found, that means the signup failed
            string actual = winDriver.FindElementByName("Validation Error").Text;


            // Assert
            Assert.AreNotEqual("Login", actual);

            winDriver.FindElementByName("OK").Click();

            winDriver.CloseApp();
        }

        // This test makes sure there is no signup for admin,s we dont want anyone to sign up as admin, only people with access to the admin user database
        [TestMethod]
        public void Robot_Test_AdminSuccussfulSignup()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            try
            {
                winDriver.FindElementByName("Sign Up").Click();
            }
            catch(Exception ex)
            {
                winDriver.CloseApp();
            }
        }

        // This test makes sure there is no signup for admin,s we dont want anyone to sign up as admin, only people with access to the admin user database
        [TestMethod]
        public void Robot_Test_AdminFailureSignup()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            try
            {
                winDriver.FindElementByName("Sign Up").Click();
            }
            catch (Exception ex)
            {
                winDriver.CloseApp();
            }
        }

        //************************ SIGN UP TESTS ************************


        //*********************** HOME PAGE TESTS ***********************
        [TestMethod]
        public void Robot_Test_ClientParkCard()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("hang");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            Actions clicker = new Actions(winDriver);
            clicker.MoveByOffset(100, -150).Click().Perform();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // if the write review button is found, the park view page opened up
            string actual = winDriver.FindElementByName("Write Review").Text;

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);



            // Assert
            Assert.AreEqual("Write Review", actual);

            winDriver.FindElementByName("Close").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void Robot_Test_ClientMapView()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("hang");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("Map").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // if the write review button is found, the park view page opened up
            string actual = winDriver.FindElementByName("MapView").Text;



            // Assert
            Assert.AreEqual("MapView", actual);

            winDriver.CloseApp();

        }

        [TestMethod]
        public void Robot_Test_LogoutButton()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("hang");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);

            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("Logout").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // if the Login button is found, the logout was successful
            string actual = winDriver.FindElementByName("Login").Text;



            // Assert
            Assert.AreEqual("Login", actual);

            winDriver.CloseApp();

        }

        [TestMethod]
        public void Robot_Test_AdminLogoutButton()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByName("Admin Mode").Click();

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("Logout").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // if the Login button is found, the logout was successful
            string actual = winDriver.FindElementByName("Login").Text;



            // Assert
            Assert.AreEqual("Login", actual);

            winDriver.CloseApp();

        }

        [TestMethod]
        public void Robot_Test_AdminParkCard()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            Actions clicker = new Actions(winDriver);
            clicker.MoveByOffset(100, -150).Click().Perform();

            System.Threading.Thread.Sleep(50);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // if the write review button is found, the park view page opened up
            string actual = winDriver.FindElementByName("Edit Park Info").Text;

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);



            // Assert
            Assert.AreEqual("Edit Park Info", actual);

            winDriver.FindElementByName("Close").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void Robot_Test_AdminAddPark()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("Add Park").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("Upload Photo").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("PFP").Click();
            winDriver.FindElementByName("Open").Click();

            winDriver.FindElementByAccessibilityId("ParkNameTextBox").SendKeys("Test");
            winDriver.FindElementByAccessibilityId("ParkAddressTextBox").SendKeys("Test");
            winDriver.FindElementByAccessibilityId("ParkDescriptionsTextBox").SendKeys("Test");
            winDriver.FindElementByAccessibilityId("ParkHoursTextBox").SendKeys("Test");

            winDriver.FindElementByName("Save").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("OK").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("Refresh").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // if the write review button is found, the park view page opened up
            string actual = winDriver.FindElementByName("Test").Text;



            // Assert
            Assert.AreEqual("Test", actual);

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.CloseApp();
        }
        //*********************** HOME PAGE TESTS ***********************


        //*********************** PARK VIEW TESTS ***********************
        
        [TestMethod]
        public void Robot_Test_ParkCardCreateReview()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("hang");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("Maximize").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            Actions clicker = new Actions(winDriver);
            clicker.MoveByOffset(0, 780).Click().Perform();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("Write Review").Click();

            winDriver.FindElementByAccessibilityId("RatingComboBox").Click();

            winDriver.FindElementByName("1 Star").Click();

            winDriver.FindElementByAccessibilityId("userReviewTextBox").SendKeys("My review");

            winDriver.FindElementByName("Leave Review").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // if the review username is found, the review was successfully made
            string actual = winDriver.FindElementByName("ClientHomeView (hang)").Text;



            // Assert
            Assert.AreEqual("ClientHomeView (hang)", actual);

            winDriver.CloseApp();
        }
       
        [TestMethod]
        public void Robot_Test_AdminEditParkInfo()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("Maximize").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            Actions clicker = new Actions(winDriver);
            clicker.MoveByOffset(0, 780).Click().Perform();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("Edit Park Info").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByAccessibilityId("ParkAddressTextBox").SendKeys("Updated - ");
            winDriver.FindElementByAccessibilityId("ParkDescriptionsTextBox").SendKeys("Updated - ");
            winDriver.FindElementByAccessibilityId("ParkHoursTextBox").SendKeys("Updated - ");

            winDriver.FindElementByName("Save").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("OK").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            Actions clicker2 = new Actions(winDriver);
            clicker2.MoveByOffset(620, 100).Click().Perform();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // if the write Change Photo button is found, the park edit page opened up
            string actual = winDriver.FindElementByName("Updated - Test").Text;

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("Close").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // Assert
            Assert.AreEqual("Updated - Test", actual);

            winDriver.FindElementByName("Logout").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.CloseApp();
        }
        
        [TestMethod]
        public void Robot_Test_ParkDelete()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("Maximize").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            Actions clicker = new Actions(winDriver);
            clicker.MoveByOffset(0, 780).Click().Perform();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("Delete Park").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // if the OK button is found, the delete confirmation window popped up
            string actual = winDriver.FindElementByName("Test has been deleted -> (park data, park image, park reviews)").Text;
        


            // Assert
            Assert.AreEqual("Test has been deleted -> (park data, park image, park reviews)", actual);

            winDriver.FindElementByName("OK").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.CloseApp();
        }
        
        [TestMethod]
        public void Robot_Test_ParkCardDeleteReview()
        {
            // Arrange
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);



            // Act
            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("Maximize").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW  ]);

            Actions clicker = new Actions(winDriver);
            clicker.MoveByOffset(0, 780).Click().Perform();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            winDriver.FindElementByName("Delete Review").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);

            // if the Change Photo button is found, the park edit page opened up
            string actual = winDriver.FindElementByName("Review deleted successfully.").Text;

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW]);



            // Assert
            Assert.AreEqual("Review deleted successfully.", actual);

            winDriver.FindElementByName("OK").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[OPENED_WINDOW ]);
        
            winDriver.CloseApp();
        }
        //*********************** PARK VIEW TESTS ***********************
    }
}
