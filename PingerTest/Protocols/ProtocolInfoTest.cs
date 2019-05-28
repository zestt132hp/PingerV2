using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Protocols;

namespace PingerTest.Protocols
{
    [TestClass]
    public class ProtocolInfoTest
    {
        [TestMethod]
        public void GetHttpProtocolAttributeTest()
        {
            //Arrange
            var moq = new Mock<IProtocolInfo>();
            moq.Setup(x => x.GetJsonAttribute<HttpProtocol>()).Returns("http/https");
            var expectInfo = new ProtocolInfo().GetJsonAttribute<HttpProtocol>();

            //Act
            string result = moq.Object.GetJsonAttribute<HttpProtocol>();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectInfo, result);
        }

        [TestMethod]
        public void GetEchoProtocolAttributeTest()
        {
            //Arrange
            var moq = new Mock<IProtocolInfo>();
            moq.Setup(x => x.GetJsonAttribute<EchoProtocol>()).Returns("icmp");
            var expectInfo = new ProtocolInfo().GetJsonAttribute<EchoProtocol>();

            //Act
            string result = moq.Object.GetJsonAttribute<EchoProtocol>();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectInfo, result);
        }

        [TestMethod]
        public void GetTcpProtocolAttributeTest()
        {
            //Arrange
            var moq = new Mock<IProtocolInfo>();
            moq.Setup(x => x.GetJsonAttribute<TcpProtocol>()).Returns("tcp/ip");
            var expectInfo = new ProtocolInfo().GetJsonAttribute<TcpProtocol>();

            //Act
            string result = moq.Object.GetJsonAttribute<TcpProtocol>();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectInfo, result);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ErrorWithOtherType()
        {
            //Arrange
            var moq = new Mock<IProtocolInfo>();
            moq.Setup(x => x.GetJsonAttribute<object>());
            var expectInfo = new ProtocolInfo().GetJsonAttribute<object>();

            //Act
            string result = moq.Object.GetJsonAttribute<object>();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectInfo, result);
        }
    }
}
