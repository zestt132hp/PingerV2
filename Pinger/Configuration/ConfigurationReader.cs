using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Pinger.Protocols;

namespace Pinger.Configuration
{
    public class ConfigurationReader : IConfigurationReader
    {
        private readonly string _sectionFormat;
        private readonly Dictionary<int, IProtocol> _dictionaryHosts = new Dictionary<int, IProtocol>();
        private IConfigurationRoot Configuration { get; }

        public ConfigurationReader(string hostFileName, string sectionFormat, IConfigurationBuilder builder)
        {
            var hostFile = string.IsNullOrEmpty(hostFileName)
                ? throw new ArgumentNullException(nameof(hostFileName))
                : hostFileName;
            _sectionFormat = string.IsNullOrEmpty(sectionFormat)
                ? throw new ArgumentNullException(nameof(sectionFormat))
                : sectionFormat;
            builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(hostFile, true, true);
            Configuration = builder.Build();
        }

        public IEnumerable<T> Read<T>()
        {
            StringBuilder strFormatter = new StringBuilder();
            strFormatter.AppendFormat(_sectionFormat, ReadProtocolAttribute(typeof(T)));
            var hosts = Configuration.GetSection(strFormatter.ToString()).GetChildren().Select(x => x.Get<T>())
                .ToArray();
            WriteInDictionary(hosts);
            return hosts;
        }

        private void WriteInDictionary(Array hosts)
        {
            foreach (IProtocol tmp in hosts)
            {
                if (!_dictionaryHosts.ContainsValue(tmp))
                    _dictionaryHosts.Add(_dictionaryHosts.Count + 1, tmp);
            }
        }

        private string ReadProtocolAttribute(Type typeProtocol)
        {
            return typeProtocol.CustomAttributes?.ToArray()[0].ConstructorArguments?[0].Value.ToString();
        }

        public Dictionary<int, IProtocol> GetReadsProtocols()
        {
            if (_dictionaryHosts.Count == 0)
            {
                Read<HttpProtocol>();
                Read<TcpProtocol>();
                Read<EchoProtocol>();
            }

            return _dictionaryHosts;
        }
    }
}
