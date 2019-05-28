using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pinger.Logger;

namespace Pinger.Protocols
{
    [JsonObject("icmp")]
    public class EchoProtocol : IProtocol
    {
        public string HostName
        {
            get => _ipHostEntry.HostName;
            set => _ipHostEntry = TryHost(value);
        }

        public string ProtocolType
        {
            get => _protocolType;
            set => TryProtocolType(value);
        }

        private static string _message = "DataTest";
        public string Message { get; set; } = _message;

        public int Interval
        {
            get => _interval;
            set => _interval = value > 0 ? value : _interval;
        }

        private PingOptions _options;
        private IPHostEntry _ipHostEntry = Dns.GetHostEntry("localhost");
        private IPAddress _ipAddress;
        private string _protocolType = "icmp";
        const int Timeout = 120;
        private int _interval = 5;

        private void TryProtocolType(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new NullReferenceException("тип протокола не может быть пустым");
            _protocolType = value.Equals(_protocolType) ? value : _protocolType;
        }

        private IPHostEntry TryHost(string value)
        {
            try
            {
                var host = Dns.GetHostEntry(value);
                return host;
            }
            catch (Exception)
            {
                throw new ArgumentException("Неверный формат хоста");
            }
        }

        public async Task<RequestStatus> SendRequestAsync(ILogger logger)
        {
            _ipAddress = _ipHostEntry.AddressList.FirstOrDefault();
            _options = new PingOptions()
            {
                DontFragment = true
            };
            using (Ping ping = new Ping())
            {
                var dataBytes = Encoding.UTF8.GetBytes(_message);
                if (_ipAddress != null)
                {
                    try
                    {
                        var reply = await ping.SendPingAsync(_ipAddress, Timeout, dataBytes, _options);
                        if (reply.Status == IPStatus.Success)
                            return new RequestStatus(true);
                    }
                    catch (InvalidOperationException e)
                    {
                        logger?.Write(e);
                    }
                    catch (SocketException e)
                    {
                        logger?.Write(e);
                    }
                }

                return new RequestStatus(false);
            }
        }
    }
}
