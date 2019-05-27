namespace Pinger.PingerModule
{
    public interface IPinger : IPingerProperty
    {
        void StartWork();
        void StopWork();
    }
}
