using System;
using Pinger.Logger;
using Pinger.Protocols;

namespace Pinger.PingerModule
{
    public interface IPingerFactory
    {        
        IPinger CreatePinger(IProtocol protocol, ILogger<Exception> excLogger, ILogger<string> logger);
    }
}
