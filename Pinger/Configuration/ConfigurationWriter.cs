using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Pinger.Protocols;

namespace Pinger.Configuration
{
    class ConfigurationWriter:IConfigurationWriter
    {
        private readonly Dictionary<string, string> _commandMap = new Dictionary<string, string>()
        {
            { "--host", "HostName" },
            { "--protocol", "ProtocolType" },
            { "--interval", "Interval" },
            { "--option", "StatusCode" }
        };
        private IConfigurationRoot _config;
        private readonly IConfigurationBuilder _builder;
        private readonly IProtocolInfo _protocolInfo;
        private readonly IConfigurationReader _reader;
        private IProtocol _protocol;
        private string _hostFile;
        public ConfigurationWriter(string hostFileName, IConfigurationBuilder builder, IProtocolInfo protocolInfo, IConfigurationReader reader)
        {
            if(string.IsNullOrEmpty(hostFileName))
                throw new NullReferenceException(hostFileName);
            _hostFile = hostFileName;
            _protocolInfo = protocolInfo ?? throw new NullReferenceException(nameof(protocolInfo));
            _builder = builder ?? throw new NullReferenceException(nameof(builder));
            _reader = reader ?? throw new NullReferenceException(nameof(reader));
        }
        private bool ParseInputsArgs()
        {
            string tmp = _config["ProtocolType"];
            if (_protocolInfo.GetJsonAttribute<HttpProtocol>().Contains(tmp))
            {
                _protocol = _config.Get<HttpProtocol>();
                return true;
            }

            if (_protocolInfo.GetJsonAttribute<TcpProtocol>().Contains(tmp))
            {
                _protocol = _config.Get<TcpProtocol>();
                return true;
            }

            if (_protocolInfo.GetJsonAttribute<EchoProtocol>().Contains(tmp))
            {
                _protocol = _config.Get<EchoProtocol>();
                return true;
            }
            return false;
        }

        public bool SaveInConfig(string[] args)
        {
            if (args == null)
                throw new NullReferenceException(nameof(args));
            _builder.AddCommandLine(args, _commandMap);
            _config = _builder.Build();
            if (ParseInputsArgs())
            {
                var dictionary = _reader.GetReadsProtocols();
                dictionary.Add(dictionary.Count + 1, _protocol);
                SaveInConfig(dictionary.Select(x => x.Value));
                return true;
            }

            return false;
        }

        private void SaveInConfig(IEnumerable<IProtocol> protocols)
        {
            var enumerable = protocols as IProtocol[] ?? protocols.ToArray();
            JsonConvert.SerializeObject(
                enumerable.GroupBy(x => $"Section({x.ProtocolType})")
                    .ToDictionary(x => x.Key, grouping => grouping.Select(x => x)), Formatting.Indented);
            var root = new Dictionary<string, Dictionary<string, IEnumerable<IProtocol>>>()
            {
                {
                    "hosts",
                    enumerable.GroupBy(x => $"Section({x.ProtocolType})")
                        .ToDictionary(x => x.Key, group => group.Select(x => x))
                }
            };
            File.WriteAllText(Directory.GetCurrentDirectory() + $"\\{_hostFile}",
                JsonConvert.SerializeObject(root, Formatting.Indented));
            using (StreamWriter sw = File.CreateText(Directory.GetCurrentDirectory() + $"\\{_hostFile}"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, root);
            }
        }

        public bool RemoveFromConfig(int index)
        {
            if (_reader.GetReadsProtocols().ContainsKey(index))
            {
                if (_reader.GetReadsProtocols().Remove(index))
                {
                    var hosts = _reader.GetReadsProtocols().Select(x => x.Value);
                    SaveInConfig(hosts);
                    return true;
                }
            }

            return false;
        }

        public void CreateConfig()
        {
            throw new NotImplementedException();
        }
    }
}
