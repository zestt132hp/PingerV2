using Pinger.Configuration;
using Pinger.PingerModule;

namespace Pinger.Commands
{
    public interface ICommandFactory
    {
         AddCommand CreateAddCommand(IConfigurationWriter writer);
         ShowCommand CreateShowCommand(IConfigurationReader reader);
         DeleteCommand CreateDeleteCommand(IConfigurationWriter writer);
         RootCommand CreateRootCommand();
         StartCommand CreateStartCommand(IPingerProcessor processor);
       /* AddCommand CreateAddCommand();
        ShowCommand CreateShowCommand();
        DeleteCommand CreateDeleteCommand();
        StartCommand CreateStartCommand();*/
    }
}
