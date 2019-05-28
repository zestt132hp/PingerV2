using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pinger.Logger;

namespace Pinger.Protocols
{
    [JsonObject("http/https")]
    public class HttpProtocol : IProtocol
    {
        private readonly Regex _regex = new Regex("^(http|https)://");
        private string _host;
        private string _protocolType = "http/https";
        private HttpStatusCode _code = HttpStatusCode.OK;
        private int _interval = 5;

        public Int16 StatusCode => (Int16) _code;

        public string ProtocolType
        {
            get => _protocolType;
            set => TryProtocolType(value);
        }

        public string Message { get; private set; }

        private void TryProtocolType(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new NullReferenceException("Тип протокола не может быть пустым");
            if (!_protocolType.Equals(value))
            {
                var constType = _protocolType.Split("/");
                var tmp = value.Split("/");
                if (!tmp.Contains(constType.First()) || !tmp.Contains(constType.Last()))
                    throw new ArgumentException("Неверно указан тип протокола");
            }

            _protocolType = value;
        }

        private void TryHost(string hostName)
        {
            if (string.IsNullOrEmpty(hostName))
                throw new ArgumentNullException(nameof(hostName));
            if (!_regex.IsMatch(hostName))
            {
                _host = "http://" + hostName;
                return;
            }

            _host = hostName;
        }

        public string HostName
        {
            get => _host;
            set => TryHost(value);
        }

        public int Interval
        {
            get => _interval;
            set => _interval = value > 0 ? value : _interval;
        }

        public async Task<RequestStatus> SendRequestAsync(ILogger logger)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(_host);
                    if (StatusCode == (int) response.StatusCode)
                        return new RequestStatus(true);
                }
                catch (Exception e)
                {
                    logger?.Write(e);
                }

                return new RequestStatus(false);
            }
        }
    }
}
