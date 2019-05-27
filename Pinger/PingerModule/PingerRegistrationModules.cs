using System;
using Microsoft.Extensions.Configuration;
using Ninject.Modules;
using Pinger.Configuration;
using Pinger.Logger;
using Pinger.Protocols;

namespace Pinger.PingerModule
{
    public class PingerRegistrationModules : NinjectModule
    {
        public override void Load()
        {
            Bind<IProtocolInfo>().To<ProtocolInfo>();
            Bind<IConfigurationNlog>().To<NlogConfiguration>();
            Bind<IPingerProcessor>().To<PingerProcessor>();
            Bind<ILogger>().To<Logger.Logger>().WithConstructorArgument(DateTime.Now.ToShortDateString());
            Bind<IConfigurationReader>().To<ConfigurationReader>();
            Bind<IConfigurationBuilder>().To<ConfigurationBuilder>();
            Bind<IConfigurationWriter>().To<ConfigurationWriter>();
        }
    }
}
