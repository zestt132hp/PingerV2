using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Configuration;
using Pinger.Protocols;

namespace PingerTest.Configuration
{
    [TestClass]
    public class ConfigurationWriterTest
    {
        [TestMethod]
        public void ConfigWriterMethods()
        {
            //Arrange
            var moq = new Mock<IConfigurationWriter>();
            moq.Setup(x => x.SaveInConfig(new []{ "noMatch" })).Returns(false);
            moq.Setup(x => x.RemoveFromConfig(-1)).Returns(false);

            //Act
            bool save = moq.Object.SaveInConfig(new []{"noMatch"});
            bool remove = moq.Object.RemoveFromConfig(-1);

            //Assert
            Assert.AreEqual(false, remove);
            Assert.AreEqual(false, save);
        }

        [TestMethod]
        public void ConfigWriterTest()
        {
            //Arrange
            var builder = new Mock<IConfigurationBuilder>();
            var protocolInfo = new Mock<IProtocolInfo>();
            var confReader = new Mock<IConfigurationReader>();
            ConfigurationWriter writer = new ConfigurationWriter("hosts.json", builder.Object, protocolInfo.Object, confReader.Object);

            //Act
            var saveResult = writer.RemoveFromConfig(-1);
            var removeResult = writer.SaveInConfig(new []{""});

            //Assert
            Assert.IsFalse(saveResult);
            Assert.IsFalse(removeResult);
        }
    }
}
