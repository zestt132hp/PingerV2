using Pinger.Protocols;

namespace Pinger.PingerModule
{
    public interface IPingerProperty
    {
        IProtocol Protocol { get; }
        int Interval { get; }
    }
}
