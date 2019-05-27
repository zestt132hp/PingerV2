using System;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Ninject;
using Ninject.Parameters;
using Pinger.Logger;
using Pinger.PingerModule;
using Pinger.Protocols;

namespace Pinger.Commands
{
    public class RootCommand
    {
        private readonly CommandLineApplication _app;
        private readonly Configuration.IConfigurationWriter _writer;
        private readonly Configuration.IConfigurationReader _reader;
        private readonly IPingerProcessor _processor;
        private readonly IKernel _kernel = new StandardKernel(new PingerRegistrationModules());
        private static readonly string HostFileName = "hosts.json";
        private static readonly string SectionName = "hosts:Section({0})";

        public RootCommand(CommandLineApplication app)
        {
            var configurationBuilder = _kernel.Get<IConfigurationBuilder>();
            var protocolInfo = _kernel.Get<IProtocolInfo>();
            var hostFileName = new ConstructorArgument("hostFileName", HostFileName);
            var sectionName = new ConstructorArgument("sectionFormat", SectionName);
            var builder = new ConstructorArgument("_builder", configurationBuilder);
            var info = new ConstructorArgument("protocolInfo", protocolInfo);
            _reader = _kernel.Get<Configuration.IConfigurationReader>(hostFileName, sectionName, builder);
            var read = new ConstructorArgument("reader", _reader);
            _writer = _kernel.Get<Configuration.IConfigurationWriter>(hostFileName, builder, info, read);
            _processor = _kernel.Get<IPingerProcessor>(new ConstructorArgument("confWorker", _reader),
                new ConstructorArgument("log", _kernel.Get<ILogger>()));
            _app = app;
            _app.HelpOption("-?|-h|--help");
        }

        public void Configure(string[] args)
        {
            _app.Command("remove", (x) => new DeleteCommand(_writer).ConfigCommand(x, args));
            _app.Command("add", (x) => new AddCommand(_writer).ConfigCommand(x, args));
            var commandShow = _app.Command("show", (x) => new ShowCommand(_reader).ConfigCommand(x, args));
            CommandOption remove = _app.Option("-r|--remove", "remove host from file with hosts",
                CommandOptionType.NoValue);
            CommandOption addOption = _app.Option("-a|--add", "add host in hosts file", CommandOptionType.NoValue);
            CommandOption show = _app.Option("-s|--show", "Display the hosts from collection",
                CommandOptionType.NoValue);
            CommandOption start = _app.Option("-p|--ping", "Begin ping process", CommandOptionType.NoValue);
            _app.OnExecute(() =>
            {
                if (addOption.HasValue())
                    return _app.Execute(addOption.Value());
                if (remove.HasValue())
                    return _app.Execute(remove.Value());
                if (show.HasValue())
                    return commandShow.Invoke();
                if (start.HasValue())
                {
                    var z = _app.Command("ping",
                        (x) => { new StartCommand(_processor).ConfigCommand(x, start.Value()); });
                    z.Invoke();
                }

                return 0;
            });
            if (args.Length == 0)
                _app.ShowHelp();
            else
            {
                Run(args);
            }
        }

        private void Run(string[] args)
        {
            try
            {
                _app.Execute(args);
            }
            catch (Exception e)
            {
                Console.WriteLine("Введено неверное значение");
                _app.ShowHelp();
            }
        }
    }
}
