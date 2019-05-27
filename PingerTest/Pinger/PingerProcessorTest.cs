using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Logger;
using Pinger.PingerModule;

namespace PingerTest.Pinger
{
    [TestClass]
    public class PingerProcessorTest
    {
        [TestMethod]
        public void PingerProcessorModules()
        {
            //Arrange
            var mock = new Mock<IPingerProcessor>();
            mock.SetupSequence(x => x.Ping(1)).Pass();
            mock.Setup(x => x.Ping());
            mock.Setup(x => x.StopPing(1));
            mock.SetupSequence(x => x.StopPing());

            //Act
            mock.Object.Ping();
            mock.Object.Ping(1);
            mock.Object.StopPing();
            mock.Object.StopPing(1);

            //Assert
            mock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void PingerProcessorCreationTest()
        {
            //Arrange
            var mockLog = new Mock<ILogger>();
            PingerProcessor process = new PingerProcessor(null, mockLog.Object);

            //Act
            process.Ping(1);

            //Assert
            Assert.Fail("Ошибка при создании процессора");
        }
    }
}
