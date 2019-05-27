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
        public string HostName { get; set; }
        public string ProtocolType { get; set; }
        private static string _message = "DataTest";
        public string Message { get; set; } = _message;

        public int Interval { get; set; }

        private PingOptions _options;
        private IPHostEntry _ipHostEntry;
        private IPAddress _ipAddress;
        const int Timeout = 120;

        public async Task<RequestStatus> SendRequestAsync(ILogger logger)
        {
            _ipHostEntry = Dns.GetHostEntry(HostName);
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
