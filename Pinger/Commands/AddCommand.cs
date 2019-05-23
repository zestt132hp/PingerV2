using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;
using Pinger.Configuration;

namespace Pinger.Commands
{
    public class AddCommand : ICommand
    {
        private readonly List<string> _optionsCommand = new List<string>() {"--host", "--protocol", "--interval", "--option"};
        private readonly IConfigurationWriter _config;
        private CommandLineApplication _app;
        public AddCommand(IConfigurationWriter writer)
        {
            _config = writer;
        }

        public void ConfigCommand(CommandLineApplication app, string[] args)
        {
            _app = app;
            _app.Option(_optionsCommand[0], "Add Host Address", CommandOptionType.SingleValue);
            _app.Option(_optionsCommand[1], "Add using protocol type", CommandOptionType.SingleValue);
            _app.Option(_optionsCommand[2], "Add interval of pinging host",
                CommandOptionType.SingleValue);
            _app.Option(_optionsCommand[3],
                "Add status code for web request | port for tcp/ip request", CommandOptionType.SingleValue);
            _app.HelpOption("-?|-h|--help");
            _app.OnExecute(() =>
            {
                if (args.Length > _optionsCommand.Count)
                    args = args.Skip(1).ToArray();
                /*
                if (option.HasValue())
                {
                    Array.Resize(ref args, args.Length + 1);
                    args[args.Length - 1] = option.Value();
                }*/
                if (!_config.SaveInConfig(args))
                {
                    Console.WriteLine("Попытка добавления протокола завершилась неудачей");
                    return 0;
                }
                Console.WriteLine("Протокол добавлен успешно");
                return 1;
            });
        }
        public int Run(string[] args)
        {
            return 0;
        }
    }
}
