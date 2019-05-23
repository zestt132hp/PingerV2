using Microsoft.Extensions.CommandLineUtils;

namespace Pinger.Commands
{
    interface ICommand
    {
        void ConfigCommand(CommandLineApplication app, string[] args);
        int Run(string[] args);
    }
}
