using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Logger;
using Pinger.PingerModule;
using Pinger.Protocols;

namespace PingerTest.Pinger
{
    [TestClass]
    public class PingerTest
    {
        [TestMethod]
        public void PinPropertyTest()
        {
            //Arrange
            var moq = new Mock<IPinger>();
            var protocol = new Mock<IProtocol>();
            moq.Setup(x => x.Protocol).Returns(protocol.Object);
            moq.Setup(x => x.Interval).Returns(200);
            HttpProtocol httpProtocol = new HttpProtocol() {Interval = 200};
            global::Pinger.PingerModule.Pinger pinger =
                new global::Pinger.PingerModule.Pinger(httpProtocol, new Mock<ILogger>().Object);

            //Act
            var resultProtocol = moq.Object.Protocol;
            var resultInterval = moq.Object.Interval;

            //Assert
            Assert.AreEqual(pinger.Interval, resultInterval);
            Assert.IsNotNull(resultProtocol);
        }

        [TestMethod]
        public void PingerMethodsTest()
        {
            //Arrange
            var moq = new Mock<IPinger>();
            moq.SetupSequence(x => x.StartWork()).Pass();
            moq.SetupSequence(x => x.StopWork()).Pass();

            //Act
            moq.Object.StartWork();
            moq.Object.StopWork();

            //Assert
            moq.VerifyAll();
        }

        [TestMethod]
        public void PingerObjTest()
        {
            //Arrange
            int defaultInterval = 5;
            HttpProtocol protocol = new HttpProtocol(){Interval = -10};
            var logger = new Mock<ILogger>();
            var mock = new Mock<global::Pinger.PingerModule.Pinger>(protocol, logger.Object);

            //Act
            var realProtocol = mock.Object.Protocol;
            mock.Object.StartWork();
            mock.Object.StopWork();

            //Assert
            Assert.AreSame(protocol, realProtocol);
            Assert.AreEqual(defaultInterval, mock.Object.Interval);
        }
    }
}
