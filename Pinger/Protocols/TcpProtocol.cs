using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pinger.Logger;

namespace Pinger.Protocols
{
    [JsonObject("tcp/ip")]
    public class TcpProtocol : IProtocol
    {
        private static readonly string RegExpressionForAddress =
            @"^(25[0-5]|2[0-4][0-9]|[0-1][0-9]{2}|[0-9]{2}|[0-9])(\.(25[0-5]|2[0-4][0-9]|[0-1][0-9]{2}|[0-9]{2}|[0-9])){3}";

        private static readonly string RegExForPort = "|([:][0-9]{2,5})";
        private Regex _reg = new Regex(RegExpressionForAddress);
        private Int32 _port;
        private static String _message = "DataTest";
        private String _host;

        public TcpProtocol()
        {
        }

        public int Port
        {
            get => _port;
            set => _port = value < 0 ? 80 : value;
        }        
        public string ProtocolType { get; set; }
        public string Message { get; private set; } = _message;
        public string HostName
        {
            get => _host;
            set => TryHost(value);
        }
        public int Interval { get; set; }

        public TcpProtocol(string hostName)
        {
            TryHost(hostName);
        }

        private void TryHost(string hostName)
        {
            if(string.IsNullOrEmpty(hostName))
                throw new NullReferenceException(nameof(hostName));
            if (hostName.ToLowerInvariant().Equals("localhost"))
            {
                _host = hostName;
                return;
            }
            if (hostName.Contains(":") && new Regex(RegExpressionForAddress + RegExForPort).IsMatch(hostName))
            {
                string[] array = hostName.Split(':');
                _host = array[0];
                Int32.TryParse(array[1], out _port);
            }
            else
            {
                if (!_reg.IsMatch(hostName))
                    throw new FormatException("Некорретно введён адрес хоста");
                _host = hostName;
            }
        }

        public RequestStatus SendRequest<T>(ILogger<Exception> logger)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(hostname: HostName, port: Port);
                    var bytes = Encoding.ASCII.GetBytes(Message);
                    var networkStream = client.GetStream();
                    networkStream.Write(bytes, 0, bytes.Length);
                    bytes = new byte[8];
                    var readBytes = networkStream.Read(bytes, 0, bytes.Length);
                    var responseData = Encoding.ASCII.GetString(bytes, 0, networkStream.Read(bytes, 0, readBytes));
                    Message = responseData;
                    if (readBytes > 0)
                    {
                        return new RequestStatus(isSuccess: true);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Write(new Exception($"{HostName}:{Port} || {e.Message}"));
                Message = "Fail";
            }
            return new RequestStatus(isSuccess: false);
        }
        private async Task<RequestStatus> SendRequestAsync(ILogger<Exception> logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            var data = Encoding.UTF8.GetBytes(_message);
            using (var client = new TcpClient())
            {
                try
                {
                    var stream = await GetStream(client, data);
                    var buffer = new byte[8];
                    var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead != 0)
                    {
                        var charBuffer = new char[8];
                        Message =
                            $"Received message {string.Concat(Encoding.UTF8.GetDecoder().GetChars(buffer, 0, bytesRead, charBuffer, 0))}";
                        return new RequestStatus(true);
                    }
                    else
                        return new RequestStatus(false);
                }
                catch (Exception e)
                {
                    logger.Write(e);
                    return new RequestStatus(false);
                }
            }
        }

        private async Task<NetworkStream> GetStream(TcpClient client, byte[] data)
        {
            await client.ConnectAsync(_host, _port).ConfigureAwait(false);
            var stream = client.GetStream();
            await
                Task.WhenAll(stream.WriteAsync(BitConverter.GetBytes(data.Length), 0, 4),
                    stream.WriteAsync(data, 0, data.Length));
            return stream;
        }

        RequestStatus IProtocol.SendRequestAsync(ILogger<Exception> logger)
        {
            Task.WaitAll(Task.WhenAll(SendRequestAsync(logger)));
            return Task.WhenAll(SendRequestAsync(logger)).Result.FirstOrDefault();
        }
    }
}
