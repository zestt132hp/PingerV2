using System;
using Microsoft.Extensions.CommandLineUtils;
using Pinger.PingerModule;

namespace Pinger.Commands
{
    public class StartCommand : ICommand
    {
        private IPingerProcessor _processor;
        private CommandLineApplication _app;
        public StartCommand(IPingerProcessor processor)
        {
            _processor = processor;
        }

        public void ConfigCommand(CommandLineApplication app, params string[] args)
        {
            _app = app;
            _app.OnExecute(() => Run(args));
        }

        public int Run(string[] args)
        {
            try
            {
                if (!(args.Length > 1))
                {
                    if (Int32.TryParse(args[0], out var tmp))
                        _processor.Ping(tmp);
                    else
                        _processor.Ping();
                }
                else
                {
                    throw new ArgumentNullException(nameof(args));
                }
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine("Неудачная попытка старта пинга");
                return 0;
            }
        }
    }
}
