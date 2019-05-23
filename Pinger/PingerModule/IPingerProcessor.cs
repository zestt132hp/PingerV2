using System;

namespace Pinger.PingerModule
{
    public interface IPingerProcessor
    {
        void Ping(Int32 index);
        void Ping();
        void StopPing();
        void StopPing(Int32 index);
    }
}
