using Microsoft.Extensions.CommandLineUtils;
using System;

namespace Pinger.Commands
{
    public class ShowCommand:ICommand
    {
        private CommandLineApplication _app;
        private Configuration.IConfigurationReader _reader;
        public ShowCommand(Configuration.IConfigurationReader reader)
        {
            _reader = reader ?? throw new NullReferenceException(nameof(reader));
        }
        public void ConfigCommand(CommandLineApplication app, string[] args)
        {
            app.OnExecute(() => Run(args));
        }

        public int Run(string[]args)
        {
            foreach (var pair in _reader.GetReadsProtocols())
            {
                Console.WriteLine($"[{pair.Key}] - [{pair.Value.HostName} {pair.Value.Interval} {pair.Value.ProtocolType}]");
            }
            return 1;
        }
    }
}
