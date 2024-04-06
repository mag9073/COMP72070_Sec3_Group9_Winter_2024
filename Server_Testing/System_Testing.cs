using System;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using static LogiPark.MVVM.Model.ParkDataManager;

namespace Server_Testing
{
    // Do run thissss -> this is the presentationframework issue :( 
        //[TestClass]
        //public class System_Testing
        //{
        //    [TestMethod]
        //    public void TestMethod1()
        //    {
        //        Thread serverThread = new Thread(() =>
        //        {
        //            IServer server = new Server.Implementations.Server();
        //            server.StartServer(13000);
        //        });

        //        serverThread.Start();

        //        Thread.Sleep(1000);

        //        ProgramClient client = new ProgramClient();

        //        UserDataManager.LoginData loginData = new UserDataManager.LoginData
        //        {
        //            username = "hang",
        //            password = "1234",
        //        };

        //        client.SendLoginRequest(loginData);

        //        string response = client.ReceiveServerResponse();
        //        Console.WriteLine(response);
        //    }
        //}

    [TestClass]
    public class Performance_Testing
    {
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        [TestMethod]
        public void Performance_Test_001_WriteAReview()
        {

            for(int i = 0; i < 10; i++)
            {
                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", @"C:\Users\Hangsihak Sin\Documents\Project-04\Logi-Park-Login\bin\Debug\LogiPark.exe");
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
                winDriver.FindElementByName("Write Review").Click();

                winDriver.FindElementByClassName("ComboBox").Click();



                var listBoxItems = winDriver.FindElementsByClassName("ListBoxItem");
                if (listBoxItems.Any())
                {
                    var lastItem = listBoxItems.LastOrDefault();
                    lastItem?.Click();
                }
                else
                {
                    Console.WriteLine("No ListBoxItem elements were found.");
                }

                var ReviewTextBox = winDriver.FindElementByClassName("TextBox");
                ReviewTextBox?.Click();
                ReviewTextBox?.SendKeys("This is my first post :D");

                var LeaveReviewButton = winDriver.FindElementByAccessibilityId("submitReviewButton");
                LeaveReviewButton?.Click();

                System.Threading.Thread.Sleep(500);
                allWindowHandles = winDriver.WindowHandles;
                winDriver.SwitchTo().Window(allWindowHandles[0]);

                winDriver.CloseApp();
            }
        }


        [TestMethod]
        public void Performance_Test_002_Add_A_Park()
        {

            List<ParkData> parks = new List<ParkData>
            {
                new ParkData
                {
                    parkName = "Toronto Safari Park",
                    parkAddress = "123 Safari Road, Toronto",
                    parkDescription = "A vast park with diverse wildlife and safari tours.",
                    parkHours = "9 AM - 5 PM"

                },
                new ParkData
                {
                    parkName = "Vancouver Nature Reserve",
                    parkAddress = "789 Wilderness Ave, Vancouver",
                    parkDescription = "A protected area showcasing Vancouver's natural beauty",
                    parkHours = "8 AM - 6 PM"
                }
            };

            List<string> parkImageFilePaths = new List<string>
            {
                @"C:\Users\Hangsihak Sin\Documents\Project-04\Server_Testing\Assets\ParkImages\Toronto-Safari-Park.jpg",
                @"C:\Users\Hangsihak Sin\Documents\Project-04\Server_Testing\Assets\ParkImages\Vancouver-Nature-Reserve.jpg"
            };


            for (int i = 0; i < parks.Count; i++)
            {
                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", @"C:\Users\Hangsihak Sin\Documents\Project-04\Logi-Park-Login\bin\Debug\LogiPark.exe");
                var winDriver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);


                // ADMIN
                var adminButton = winDriver.FindElementByAccessibilityId("adminModeButton");
                adminButton?.Click();

                var allWindowHandles = winDriver.WindowHandles;
                winDriver.SwitchTo().Window(allWindowHandles[0]);

                System.Threading.Thread.Sleep(800);

                winDriver.FindElementByAccessibilityId("usernameTextBox").SendKeys("admin");
                winDriver.FindElementByAccessibilityId("passwordTextBox").SendKeys("123");
                winDriver.FindElementByAccessibilityId("Login").Click();

                // allows the windows to actually open before trying to access them
                System.Threading.Thread.Sleep(800);

                allWindowHandles = winDriver.WindowHandles;
                winDriver.SwitchTo().Window(allWindowHandles[0]);

                var addParkButton = winDriver.FindElementByAccessibilityId("addParkButton");
                addParkButton?.Click();

                allWindowHandles = winDriver.WindowHandles;
                winDriver.SwitchTo().Window(allWindowHandles[0]);

                // Filling Add Park Info
                winDriver.FindElementByAccessibilityId("ParkNameTextBox").SendKeys(parks[i].parkName);

                winDriver.FindElementByAccessibilityId("ParkAddressTextBox").SendKeys(parks[i].parkAddress);

                winDriver.FindElementByAccessibilityId("ParkDescriptionsTextBox").SendKeys(parks[i].parkDescription);

                winDriver.FindElementByAccessibilityId("ParkHoursTextBox").SendKeys(parks[i].parkHours);

                winDriver.FindElementByAccessibilityId("UploadButton").Click();

                Thread.Sleep(1000);

                

                Actions actions = new Actions(winDriver);
                actions.SendKeys(parkImageFilePaths[i] + Keys.Enter).Perform();

                winDriver.FindElementByAccessibilityId("SaveButton").Click();

                winDriver.FindElementByAccessibilityId("2").Click();

                System.Threading.Thread.Sleep(500);
                allWindowHandles = winDriver.WindowHandles;
                winDriver.SwitchTo().Window(allWindowHandles[0]);

                winDriver.CloseApp();

            }
        }
    }
    
}