using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using LogiPark.MVVM.Model;
using LogiPark.MVVM.View;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography.X509Certificates;

namespace Client_Test_Suite
{
    [TestClass]
    public class Client_Integration_Tests
    {
        [TestMethod]
        public void IT_PC_001()
        {
            //Arrange

            //Act
            ProgramClient client = new ProgramClient();

            //Assert
            Assert.IsNotNull(client);
        }
    }
}
