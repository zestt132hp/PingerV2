using System;

namespace Pinger.Logger
{
    public interface ILogger
    {
        void Write(string value);
        void Write(Exception exc);
    }
}
