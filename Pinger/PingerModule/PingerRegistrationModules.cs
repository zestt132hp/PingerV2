using System;
using Microsoft.Extensions.Configuration;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using Pinger.Commands;
using Pinger.Configuration;
using Pinger.Logger;
using Pinger.Protocols;

namespace Pinger.PingerModule
{
   public class PingerRegistrationModules:NinjectModule
    {
        public override void Load()
        {
            Bind<IProtocolInfo>().To<ProtocolInfo>();
            Bind<IConfigurationNlog>().To<NlogConfiguration>();
            Bind<IPingerProcessor>().To<PingerProcessor>();
            Bind<ILogger<String>>().To<Logger.Logger>().WithConstructorArgument(DateTime.Now.ToShortDateString());
            Bind<ILogger<Exception>>().To<ExceptionLogger>()
                .WithConstructorArgument(DateTime.Now.ToShortDateString());
            Bind<IConfigurationReader>().To<ConfigurationReader>();
            Bind<IConfigurationBuilder>().To<ConfigurationBuilder>();
            Bind<IConfigurationWriter>().To<ConfigurationWriter>();
            Bind<IPingerFactory>().ToFactory(() => new TypeMatchingArgumentInheritanceInstanceProvider());
            Bind<ICommandFactory>().ToFactory(() => new TypeMatchingArgumentInheritanceInstanceProvider());
        }
    }
}
