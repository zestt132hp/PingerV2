using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Protocols;
using ILogger = Pinger.Logger.ILogger;

namespace PingerTest.Protocols
{
    [TestClass]
    public class ProtocolTests
    {
        [TestMethod]
        public void ProtocolSendRequestSyncTest()
        {
            //Arrange
            var moq = new Mock<IProtocol>();
            var log = new Mock<ILogger>();
            moq.SetupSequence((x) => x.SendRequestAsync(log.Object)).ReturnsAsync(new RequestStatus(false));
            global::Pinger.PingerModule.Pinger pinger = new global::Pinger.PingerModule.Pinger(
                moq.Object, log.Object);

            //Act
            var status = pinger.Protocol.SendRequestAsync(log.Object).Result;

            //Assert
            Assert.IsNotNull(pinger.Protocol);
            Assert.IsFalse(status.GetStatus);
        }

        [TestMethod]
        public async Task ProtocolSendRequestASyncTestAsync()
        {
            //Arrange
            var moq = new Mock<IProtocol>();
            var log = new Mock<ILogger>();
            moq.SetupSequence((x) => x.SendRequestAsync(log.Object)).Returns(async () =>
            {
                await Task.Yield();
                return new RequestStatus(true);
            });
            global::Pinger.PingerModule.Pinger pinger = new global::Pinger.PingerModule.Pinger(moq.Object, log.Object);

            //Act
            var result = await pinger.Protocol.SendRequestAsync(log.Object);

            //Assert
            Assert.IsNotNull(pinger.Protocol);
            Assert.IsTrue(result.GetStatus);
        }
        [TestMethod]
        public void ProtocolPropertyTest()
        {
            //Arrange
            var moq = new Mock<IProtocol>();
            var log = new Mock<ILogger>();
            string expectedHost = "http://ya.ru";
            int expectedInterval = 200;
            string expectedProtocolType = "http/https";
            moq.SetupGet(x => x.HostName).Returns(expectedHost);
            moq.SetupGet(x => x.Interval).Returns(expectedInterval);
            moq.SetupSequence((x) => x.SendRequestAsync(log.Object)).ReturnsAsync(new RequestStatus(true));
            moq.Setup(x => x.ProtocolType).Returns(expectedProtocolType);
            global::Pinger.PingerModule.Pinger ping = new global::Pinger.PingerModule.Pinger(moq.Object, log.Object);

            //Act
            var result = ping.Protocol.SendRequestAsync(log.Object).Result;
            int interval = ping.Interval;

            //Assert
            moq.Verify(x=>x.SendRequestAsync(log.Object));
            Assert.IsNotNull(result);
            Assert.AreEqual(result.GetStatus, true);
            Assert.AreEqual(interval, 200);
            Assert.AreEqual(expectedProtocolType, ping.Protocol.ProtocolType);

        }
    }
}
