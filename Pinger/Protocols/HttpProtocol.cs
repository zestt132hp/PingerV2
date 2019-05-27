using System;
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
        private HttpStatusCode _code = HttpStatusCode.OK;

        public Int16 StatusCode => (Int16) _code;
        public string ProtocolType { get; set; }
        public string Message { get; private set; }

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

        public int Interval { get; set; }

        public async Task<RequestStatus> SendRequestAsync(ILogger logger)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(HostName);
                    if (StatusCode == (int) response.StatusCode)
                        return new RequestStatus(true);
                }
                catch (HttpRequestException e)
                {
                    logger?.Write(e);
                }

                return new RequestStatus(false);
            }
        }
    }
}
