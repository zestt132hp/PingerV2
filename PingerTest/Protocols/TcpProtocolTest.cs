using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Protocols;
using Pinger.Logger;
using System.Threading.Tasks;

namespace PingerTest.Protocols
{
    [TestClass]
    public class TcpProtocolTest
    {
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TcpProtocolPropertyTest()
        {
            //Arrange
            int intervalExp = 5;
            string hostExp = "localhost";
            int portExp = 80;
            TcpProtocol tcpProtocol = new TcpProtocol()
            {
                HostName =  "gwfdgsdgh",
                Interval = -1,
                Port = -90,
                ProtocolType = "tcp/ip"
            };

            //Act
            var portResult = tcpProtocol.Port;
            var protocol =(IProtocol) tcpProtocol;

            //Assert
            Assert.IsNotNull(protocol);
            Assert.AreEqual(intervalExp, protocol.Interval);
            Assert.AreEqual(hostExp, protocol.HostName);
            Assert.AreEqual(portExp, portResult);
        }
        [TestMethod]
        public void TcpProtocolMethodSyncTest()
        {
            //Arrange
            var log = new Mock<ILogger>();
            TcpProtocol protocl = new TcpProtocol();
            //Act
            var result = protocl.SendRequestAsync(log.Object).Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.GetStatus);
        }

        [TestMethod]
        public async Task TcpProtocolMethodASyncTestAsync()
        {
            //Arrange
            var log = new Mock<ILogger>();
            TcpProtocol protocol = new TcpProtocol(){HostName = "8.8.8.8", Port = 111};

            //Act
            var result = await protocol.SendRequestAsync(log.Object);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.GetStatus);
        }
    }
}
