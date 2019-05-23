using System.Collections.Generic;
using Pinger.Protocols;

namespace Pinger.Configuration
{
    public interface IConfigurationReader
    {
        Dictionary<int, IProtocol> GetReadsProtocols();
        IEnumerable<T> Read<T>();
    }
}
