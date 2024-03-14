using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;

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
        public void GUI_Test_ClientSuccussfulLogin()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\Hangsihak Sin\Pictures\LogiPark\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("hang");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(500);

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);


            // if the ClientHomeView window name is found, that means the login was successful
            string actual = winDriver.FindElementByName("ClientHomeView").Text;

            Assert.AreEqual("ClientHomeView", actual);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void GUI_Test_ClientFailureLogin()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("abcd");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(500);

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);


            // if the ClientHomeView window name isnt found, that means the login failed
            string actual = winDriver.FindElementByName("Login").Text;

            Assert.AreNotEqual("ClientHomeView", actual);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void GUI_Test_AdminSuccussfulLogin()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(500);

            allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);


            // if the AdminHomeView window name is found, that means the login was successful
            string actual = winDriver.FindElementByName("AdminHomeView").Text;

            Assert.AreEqual("AdminHomeView", actual);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void GUI_Test_AdminFailureLogin()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("abc");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(500);

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
        public void GUI_Test_ClientSuccussfulSignup()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Sign Up").Click();

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("abcd");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Sign Up").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(500);

            allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the Login button name is found, that means the signup was successful
            string actual = winDriver.FindElementByName("Login").Text;

            Assert.AreEqual("Login", actual);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void GUI_Test_ClientFailureSignup()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Sign Up").Click();

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // not typing anything into username should cuase a fail
            //winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("");

            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Sign Up").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(500);

            allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the Login button name isnt found, that means the signup failed
            string actual = winDriver.FindElementByName("Validation Error").Text;

            Assert.AreNotEqual("Login", actual);

            winDriver.FindElementByName("OK").Click();

            winDriver.CloseApp();
        }

        [TestMethod]
        public void GUI_Test_AdminSuccussfulSignup()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Sign Up").Click();

            allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("ADMIN");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Sign Up").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(500);

            allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the Login button name is found, that means the signup was successful
            string actual = winDriver.FindElementByName("Login").Text;

            Assert.AreEqual("Login", actual);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void GUI_Test_AdminFailureSignup()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Sign Up").Click();

            allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("abc");

            // not typing a password should fail the signup
            //winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("");

            winDriver.FindElementByName("Sign Up").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(500);

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
        public void GUI_Test_ClientParkCard()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("hang");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(800);

            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Waterloo Park").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the write review button is found, the park view page opened up
            string actual = winDriver.FindElementByName("Write Review").Text;

            Assert.AreEqual("Write Review", actual);

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Close").Click();

            System.Threading.Thread.Sleep(500);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void GUI_Test_ClientMapView()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("hang");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(500);

            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Map").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the write review button is found, the park view page opened up
            string actual = winDriver.FindElementByName("Map View").Text;

            Assert.AreEqual("Map View", actual);

            winDriver.CloseApp();

        }

        [TestMethod]
        public void GUI_Test_LogoutButton()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("hang");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(500);

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
        public void GUI_Test_AdminLogoutButton()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(500);

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
        public void GUI_Test_AdminParkCard()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\Hangsihak Sin\Pictures\LogiPark\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(800);

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Waterloo Park").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the write review button is found, the park view page opened up
            string actual = winDriver.FindElementByName("Edit Park Info").Text;

            Assert.AreEqual("Edit Park Info", actual);

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Close").Click();

            System.Threading.Thread.Sleep(500);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.CloseApp();
        }

        [TestMethod]
        public void GUI_Test_AdminAddPark()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(800);

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Add Park").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the write review button is found, the park view page opened up
            string actual = winDriver.FindElementByName("Upload Photo").Text;

            Assert.AreEqual("Upload Photo", actual);

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Close").Click();

            System.Threading.Thread.Sleep(500);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.CloseApp();
        }
        //*********************** HOME PAGE TESTS ***********************


        //*********************** PARK VIEW TESTS ***********************
        
        [TestMethod]
        public void GUI_Test_ParkViewWriteReview()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("hang");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("1234");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(1000);

            var allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Waterloo Park").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Write Review").Click();

            winDriver.FindElementByAccessibilityId("RatingComboBox").Click();

            winDriver.FindElementByName("1 Star").Click();

            winDriver.FindElementByAccessibilityId("userReviewTextBox").SendKeys("My review");

            winDriver.FindElementByName("Leave Review").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the review username is found, the review was successfully made
            string actual = winDriver.FindElementByName("ClientHomeView").Text;

            System.Threading.Thread.Sleep(500);

            Assert.AreEqual("ClientHomeView", actual);

            winDriver.CloseApp();
        }
       
        [TestMethod]
        public void GUI_Test_AdminEditParkInfo()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(800);

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Waterloo Park").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Edit Park Info").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the write Change Photo button is found, the park edit page opened up
            string actual = winDriver.FindElementByName("Change Photo").Text;

            Assert.AreEqual("Change Photo", actual);

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Close").Click();

            System.Threading.Thread.Sleep(500);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.CloseApp();
        }
        
        [TestMethod]
        public void GUI_Test_AdminDeletePark()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(800);

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Waterloo Park").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Delete Park").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the OK button is found, the delete confirmation window popped up
            string actual = winDriver.FindElementByName("OK").Text;
        
            Assert.AreEqual("OK", actual);

            winDriver.FindElementByName("OK").Click();

            System.Threading.Thread.Sleep(500);
            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.CloseApp();
        }
        
        
        [TestMethod]
        public void GUI_Test_AdminDeleteReview()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", @"C:\Users\OwenA\source\repos\2nd Year 2nd SEMESTER\COMP72070 - Project IV\COMP72070_Sec3_Group9_Winter_2024\Logi-Park-Login\bin\Debug\LogiPark.exe");
            var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            winDriver.FindElementByName("Admin Mode").Click();

            var allWindowHandles = winDriver.WindowHandles;

            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
            winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
            winDriver.FindElementByName("Login").Click();

            // allows the windows to actually open before trying to access them
            System.Threading.Thread.Sleep(800);

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Waterloo Park").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("Delete Review").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            // if the Change Photo button is found, the park edit page opened up
            string actual = winDriver.FindElementByName("Review deleted successfully.").Text;

            Assert.AreEqual("Review deleted successfully.", actual);

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);

            winDriver.FindElementByName("OK").Click();

            allWindowHandles = winDriver.WindowHandles;
            winDriver.SwitchTo().Window(allWindowHandles[0]);
        
            winDriver.CloseApp();
        }
        //*********************** PARK VIEW TESTS ***********************
    }
}
