using System;
using LogiPark.MVVM.Model;
using Server.Interfaces;

namespace Server_Testing
{
    // Do run thissss -> this is the presentationframework issue :( 
        [TestClass]
        public class System_Testing
        {
            [TestMethod]
            public void TestMethod1()
            {
                Thread serverThread = new Thread(() =>
                {
                    IServer server = new Server.Implementations.Server();
                    server.StartServer(13000);
                });

                serverThread.Start();

                Thread.Sleep(1000);

                ProgramClient client = new ProgramClient();

                UserDataManager.LoginData loginData = new UserDataManager.LoginData
                {
                    username = "hang",
                    password = "1234",
                };

                client.SendLoginRequest(loginData);

                string response = client.ReceiveServerResponse();
                Console.WriteLine(response);
            }
        }
    
}