namespace Pinger.Configuration
{
    public interface IConfigurationWriter
    {
        bool SaveInConfig(string[] host);
        bool RemoveFromConfig(int index);
    }
}
