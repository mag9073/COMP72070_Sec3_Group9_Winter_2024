using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Specialized;

namespace Client_Test_Suite
{
    [TestClass]
    public class Client_Robot_Tests
    {
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        //******************************************************************************
        //*********************** RUN THE SERVER WHEN TESTING!!! *********************** 
        //******************************************************************************



        //************************ LOGIN TESTS ************************
        [TestMethod]
        public void Robot_Test_ClientSuccussfulLogin()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path);
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("hang");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);


            // if the ClientHomeView window name is found, that means the login was successful
            string actual = winDriver.FindElementByName("ClientHomeView (hang)").Text;

            Assert.AreEqual("ClientHomeView (hang)", actual);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void Robot_Test_ClientFailureLogin()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("abcd");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the ClientHomeView window name isnt found, that means the login failed
            string actual = winDriver.FindElementByName("Login").Text;

            Assert.AreNotEqual("ClientHomeView", actual);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void Robot_Test_AdminSuccussfulLogin()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the AdminHomeView window name is found, that means the login was successful
            string actual = winDriver.FindElementByName("AdminHomeView").Text;

            Assert.AreEqual("AdminHomeView", actual);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void Robot_Test_AdminFailureLogin()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("abc");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the AdminHomeView window name isnt found, that means the login failed
            string actual = winDriver.FindElementByName("Login").Text;

            Assert.AreNotEqual("AdminHomeView", actual);

            winDriver.CloseApp();
        }

        //************************ LOGIN TESTS ************************




        //************************ SIGNUP TESTS ************************
        [TestMethod]
        public void Robot_Test_ClientSuccussfulSignup()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Sign Up").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("abcde");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("12345");
            winDriver.FindElementByName("Sign Up").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the Login button name is found, that means the signup was successful
            string actual = winDriver.FindElementByName("Login").Text;

            Assert.AreEqual("Login", actual);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void Robot_Test_ClientFailureSignup()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Sign Up").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // not typing anything into username should cuase a fail
            //winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("");

            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Sign Up").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the Login button name isnt found, that means the signup failed
            string actual = winDriver.FindElementByName("Validation Error").Text;

            Assert.AreNotEqual("Login", actual);

            winDriver.FindElementByName("OK").Click();

            winDriver.CloseApp();
        }

        [TestMethod]
        public void Robot_Test_AdminSuccussfulSignup()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Sign Up").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("ADMIN");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Sign Up").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the Login button name is found, that means the signup was successful
            string actual = winDriver.FindElementByName("Login").Text;

            Assert.AreEqual("Login", actual);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void Robot_Test_AdminFailureSignup()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Sign Up").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("abc");

            // not typing a password should fail the signup
            //winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("");

            winDriver.FindElementByName("Sign Up").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the Login button name isnt found, that means the signup failed
            string actual = winDriver.FindElementByName("Validation Error").Text;

            Assert.AreNotEqual("Login", actual);

            winDriver.FindElementByName("OK").Click();

            winDriver.CloseApp();
        }

        //************************ SIGN UP TESTS ************************


        //*********************** HOME PAGE TESTS ***********************
        [TestMethod]
        public void Robot_Test_ClientParkCard()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("hang");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Waterloo Park").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the write review button is found, the park view page opened up
            string actual = winDriver.FindElementByName("Write Review").Text;

            Assert.AreEqual("Write Review", actual);

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Close").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void Robot_Test_ClientMapView()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("hang");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Map").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the write review button is found, the park view page opened up
            string actual = winDriver.FindElementByName("MapView").Text;

            Assert.AreEqual("MapView", actual);

            winDriver.CloseApp();

        }

        [TestMethod]
        public void Robot_Test_LogoutButton()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("hang");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);

            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Logout").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the Login button is found, the logout was successful
            string actual = winDriver.FindElementByName("Login").Text;

            Assert.AreEqual("Login", actual);

            winDriver.CloseApp();

        }

        [TestMethod]
        public void Robot_Test_AdminLogoutButton()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Logout").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the Login button is found, the logout was successful
            string actual = winDriver.FindElementByName("Login").Text;

            Assert.AreEqual("Login", actual);

            winDriver.CloseApp();

        }

        [TestMethod]
        public void Robot_Test_AdminParkCard()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Waterloo Park").Click();

            System.Threading.Thread.Sleep(50);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the write review button is found, the park view page opened up
            string actual = winDriver.FindElementByName("Edit Park Info").Text;

            Assert.AreEqual("Edit Park Info", actual);

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Close").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void Robot_Test_AdminAddPark()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Add Park").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Upload Photo").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("PFP").Click();
            winDriver.FindElementByName("Open").Click();

            winDriver.FindElementByAccessibilityId("ParkNameTextBox").SendKeys("Test");
            winDriver.FindElementByAccessibilityId("ParkAddressTextBox").SendKeys("Test");
            winDriver.FindElementByAccessibilityId("ParkDescriptionsTextBox").SendKeys("Test");
            winDriver.FindElementByAccessibilityId("ParkHoursTextBox").SendKeys("Test");

            winDriver.FindElementByName("Save").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("OK").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Refresh").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the write review button is found, the park view page opened up
            string actual = winDriver.FindElementByName("Test").Text;

            Assert.AreEqual("Test", actual);

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.CloseApp();
        }
        //*********************** HOME PAGE TESTS ***********************


        //*********************** PARK VIEW TESTS ***********************
        
        [TestMethod]
        public void Robot_Test_ParkViewWriteReview()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("hang");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Maximize").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Test").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Write Review").Click();

            winDriver.FindElementByAccessibilityId("RatingComboBox").Click();

            winDriver.FindElementByName("1 Star").Click();

            winDriver.FindElementByAccessibilityId("userReviewTextBox").SendKeys("My review");

            winDriver.FindElementByName("Leave Review").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the review username is found, the review was successfully made
            string actual = winDriver.FindElementByName("ClientHomeView (hang)").Text;

            Assert.AreEqual("ClientHomeView (hang)", actual);

            winDriver.CloseApp();
        }
       
        [TestMethod]
        public void Robot_Test_AdminEditParkInfo()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Maximize").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Test").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Edit Park Info").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("ParkAddressTextBox").SendKeys("Updated - ");
            winDriver.FindElementByAccessibilityId("ParkDescriptionsTextBox").SendKeys("Updated - ");
            winDriver.FindElementByAccessibilityId("ParkHoursTextBox").SendKeys("Updated - ");

            winDriver.FindElementByName("Save").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("OK").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the write Change Photo button is found, the park edit page opened up
            string actual = winDriver.FindElementByName("AdminHomeView").Text;

            Assert.AreEqual("AdminHomeView", actual);

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Logout").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.CloseApp();
        }
        
        [TestMethod]
        public void Robot_Test_AdminDeletePark()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Maximize").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Test").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Delete Park").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the OK button is found, the delete confirmation window popped up
            string actual = winDriver.FindElementByName("Test has been deleted -> (park data, park image, park reviews)").Text;
        
            Assert.AreEqual("Test has been deleted -> (park data, park image, park reviews)", actual);

            winDriver.FindElementByName("OK").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.CloseApp();
        }
        
        [TestMethod]
        public void Robot_Test_AdminDeleteReview()
        {
            var appiumOptions = new AppiumOptions();
            string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../Logi-Park-Login/bin/Debug/LogiPark.exe";
            appiumOptions.AddAdditionalCapability("app", path); 
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            System.Threading.Thread.Sleep(300);
            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Maximize").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Test").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Delete Review").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the Change Photo button is found, the park edit page opened up
            string actual = winDriver.FindElementByName("Review deleted successfully.").Text;

            Assert.AreEqual("Review deleted successfully.", actual);

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("OK").Click();

            System.Threading.Thread.Sleep(300);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);
        
            winDriver.CloseApp();
        }
        //*********************** PARK VIEW TESTS ***********************
    }
}
