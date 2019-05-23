
namespace Pinger.Logger
{
    public interface ILogger<T>
    {
        void Write(T value);
    }
}
