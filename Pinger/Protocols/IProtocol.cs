using System.Threading.Tasks;
using Pinger.Logger;

namespace Pinger.Protocols
{
    public interface IProtocol
    {
        string HostName { get; }
        string ProtocolType { get; }
        int Interval { get; }
        string Message { get; }
        Task<RequestStatus> SendRequestAsync(ILogger logger);
    }
}
