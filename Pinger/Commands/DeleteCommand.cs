using System;
using Microsoft.Extensions.CommandLineUtils;

namespace Pinger.Commands
{
    public class DeleteCommand : ICommand
    {
        private CommandLineApplication _app;
        readonly Configuration.IConfigurationWriter _writer;

        public DeleteCommand(Configuration.IConfigurationWriter writer)
        {
            _writer = writer;
        }

        public void ConfigCommand(CommandLineApplication app, string[] args)
        {
            app.Option("-value", "delete host with value", CommandOptionType.SingleValue);
            app.HelpOption("--help|-h|-?");
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            app.OnExecute(() => Run(new[] {args[args.Length - 1]}));
        }

        public int Run(string[] args)
        {
            if (args.Length != 1)
                return 0;
            if (!Int32.TryParse(args[0].Split('=')[1], out var tmp))
                return 0;
            if (_writer.RemoveFromConfig(tmp))
            {
                Console.WriteLine("Remove success");
                return 1;
            }

            return 0;
        }
    }
}
