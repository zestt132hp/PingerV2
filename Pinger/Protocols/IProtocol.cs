using System;
using Pinger.Logger;

namespace Pinger.Protocols
{
    public interface IProtocol
    {        
        string HostName { get; }     
        string ProtocolType { get; }
        int Interval { get; }
        string Message { get; }
        RequestStatus SendRequest<T>(ILogger<Exception> logger);
        RequestStatus SendRequestAsync(ILogger<Exception> logger);
    }
}
