using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Logger;
using Pinger.Protocols;

namespace PingerTest.Protocols
{
    [TestClass]
    public class EchoProtocolTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EchoProotocolPropertyTest()
        {
            //Arrange
            int intervalExp = 5;
            string hostExp = "localhost";
            EchoProtocol echoProtocol = new EchoProtocol()
            {
                HostName = "klhdgf",
                Interval = 0,
                ProtocolType = "icmp"
            };

            //Act
            var protocol = (IProtocol) echoProtocol;

            //Assert
            Assert.IsNotNull(protocol);
            Assert.AreEqual(intervalExp, protocol.Interval);
            Assert.AreEqual(hostExp, protocol.HostName);
        }

        [TestMethod]
        public void EchoProtocolMethodSyncTest()
        {
            //Arrange
            var log = new Mock<ILogger>();
            EchoProtocol protocol = new EchoProtocol();

            //Act
            var result = protocol.SendRequestAsync(log.Object).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.GetStatus);
        }

        [TestMethod]
        public async Task EchoProtocolMethodASyncTest()
        {
            //Arrange
            var log = new Mock<ILogger>();
            EchoProtocol protocol = new EchoProtocol() {HostName = "online.gameroom.ru"};

            //Act
            var result = await protocol.SendRequestAsync(log.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.GetStatus);
        }
    }
}
