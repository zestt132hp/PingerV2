using System;
using Microsoft.Extensions.CommandLineUtils;
using Pinger.Commands;

namespace Pinger
{
    public class Program
    {
        // ReSharper disable once FunctionRecursiveOnAllPaths
        public static void Main(string[] args)
        {           
            CommandLineApplication app = new CommandLineApplication();
            var root = new RootCommand(app);
            root.Configure(args);
            Main(Console.ReadLine()?.Split(' '));
        }       
    }   
}
