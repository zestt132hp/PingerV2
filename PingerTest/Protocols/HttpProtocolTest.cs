using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pinger.Protocols;
using Pinger.Logger;
using Moq;
using System.Threading.Tasks;

namespace PingerTest.Protocols
{
    [TestClass]
    public class HttpProtocolTest
    {
        [TestMethod]
        public void HttpProotocolPropertyTest()
        {
            //Arrange
            int intervalExp = 5;
            string hostExp = "http://ya.ru";
            var moq = new Mock<IProtocol>();
            moq.SetupGet(x => x.HostName).Returns("ya.ru");
            moq.SetupGet(x => x.Interval).Returns(0);
            moq.SetupGet(x => x.ProtocolType).Returns("http/https");

            //Act
            HttpProtocol protocol = new HttpProtocol()
            {
                HostName = moq.Object.HostName,
                Interval = moq.Object.Interval,
                ProtocolType = moq.Object.ProtocolType
            };

            //Assert
            Assert.IsNotNull(protocol);
            Assert.AreEqual(intervalExp, protocol.Interval);
            Assert.AreEqual(hostExp, protocol.HostName);
        }
        [TestMethod]
        public void HttpProtocolMethodSyncTest()
        {
            //Arrange
            HttpProtocol protocol = new HttpProtocol();
            var log = new Mock<ILogger>();

            //Act
            var result = protocol.SendRequestAsync(log.Object).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.GetStatus);
        }
        
        [TestMethod]
        public async Task HttpProtocolMethodASyncTest()
        {
            //Arrange
            HttpProtocol protocol = new HttpProtocol(){HostName = "ya.ru"};
            var log = new Mock<ILogger>();

            //Act
            var result = await protocol.SendRequestAsync(log.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetStatus);
        }
    }
}
