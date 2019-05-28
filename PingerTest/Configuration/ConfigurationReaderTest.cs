using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Configuration;
using Pinger.Protocols;

namespace PingerTest.Configuration
{
    [TestClass]
    public class ConfigurationReaderTest
    {
        [TestMethod]
        public void ConfigurationReaderTst()
        {
            //Arrange
            var mock = new Mock<IConfigurationReader>();
            var protocol = new Mock<IProtocol>();
            mock.Setup(x => x.GetReadsProtocols()).Returns(new Dictionary<int, IProtocol>() {{1, protocol.Object}});
            IEnumerable<IProtocol> protocols = new[] {protocol.Object};
            mock.Setup(x => x.Read<IProtocol>()).Returns(protocols);

            //Act
            var result = mock.Object.GetReadsProtocols();
            IProtocol resultProtocol = mock.Object.GetReadsProtocols()[1];

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultProtocol);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ConfigReaderTest()
        {
            //Arrange
            var mock = new Mock<IConfigurationBuilder>();
            ConfigurationReader reader = new ConfigurationReader("tmp", "value", mock.Object);
            
            //Act
            var resDictionary = reader.GetReadsProtocols();
            var protoc = reader.Read<HttpProtocol>();

            //Assert
            Assert.IsNull(protoc);
            Assert.IsNotNull(resDictionary);
        }
    }
}
