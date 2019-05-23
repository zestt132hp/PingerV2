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
    public class EchoProtocol:IProtocol
    {
        public string HostName { get; set; }
        public string ProtocolType { get; set; }
        private static string _message = "DataTest";
        public string Message { get; set; } = _message;

        public int Interval { get; set; }

        private readonly PingOptions _options;
        private readonly IPHostEntry _ipHostEntry;
        private readonly IPAddress _ipAddress;
        const int Timeout = 120;
        public EchoProtocol(string host)
        {
            HostName = string.IsNullOrEmpty(host) ? host : "localhost";
            _options = new PingOptions();
            if (HostName != null) _ipHostEntry = Dns.GetHostEntry(HostName);
            _ipAddress = _ipHostEntry.AddressList.FirstOrDefault();
            _options.DontFragment = true;
        }

        public EchoProtocol()
        {
        }

        public RequestStatus SendRequest<T>(ILogger<Exception> logger)
        {
            var reply = default(PingReply);
            try
            {
                var pingSender = new Ping();
                var dataBytes = Encoding.UTF8.GetBytes(_message);
                if (_ipAddress != null)
                {
                    reply = pingSender.Send(_ipAddress, Timeout, dataBytes, _options);
                }
                if (reply != null)
                {
                    var bytes = reply.Buffer;
                    var responseData = Encoding.UTF8.GetString(dataBytes, 0, bytes.Length);
                    Message = $"Получены данные {responseData}";
                    return new RequestStatus(reply.Status == IPStatus.Success);
                }
                else
                {
                    Message = $"Полученные даннные отсутвуют {nameof(reply)}";
                    return new RequestStatus(false);
                }
            }
            catch (Exception ex)
            {
                logger.Write(ex);
                Message = $"Ошибка при получении данных";
                return new RequestStatus(false);
            }
        }
        RequestStatus IProtocol.SendRequestAsync(ILogger<Exception> logger)
        {
            return Task.WhenAll(SendRequestAsync(logger)).Result.FirstOrDefault();
        }

        private async Task<RequestStatus> SendRequestAsync(ILogger<Exception> logger)
        {
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
