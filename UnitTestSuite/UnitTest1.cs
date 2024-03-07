using Server;
using Logi_Park_Login;

namespace UnitTestSuite
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
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
    public class LoggerTests
    {
        [TestMethod]
        public void UT_LOG_001()
        {
            //Arrange

            //Act

            //Assert
            Assert.AreEqual(1, 1);
        }
    }
}