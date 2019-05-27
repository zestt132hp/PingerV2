using System;
using System.Collections.Generic;
using System.Linq;
using Pinger.Configuration;
using Pinger.Logger;


namespace Pinger.PingerModule
{
    public class PingerProcessor : IPingerProcessor
    {
        private readonly IConfigurationReader _configWorker;
        private readonly ILogger _logger;
        private readonly Dictionary<int, IPinger> _pooling = new Dictionary<int, IPinger>();

        public PingerProcessor(IConfigurationReader confWorker, ILogger log)
        {
            _logger = log;
            if (confWorker != null)
            {
                _configWorker = confWorker;
                LoadPingerstoPing();
            }
            else
                throw new NullReferenceException(nameof(PingerProcessor));
        }

        private void LoadPingerstoPing()
        {
            foreach (var keyValuePair in _configWorker.GetReadsProtocols())
            {
                _pooling.Add(keyValuePair.Key, new Pinger(keyValuePair.Value, _logger));
            }
        }

        public void Ping()
        {
            _pooling.Values.AsParallel().ForAll(x => x.StartWork());
        }

        public void StopPing()
        {
            _pooling.Values.AsParallel().ForAll(x => x.StopWork());
        }

        public void StopPing(int index)
        {
            if (_pooling.ContainsKey(index))
                _pooling[index].StopWork();
            else
                throw new IndexOutOfRangeException(nameof(index));
        }

        public void Ping(int index)
        {
            if (_pooling.ContainsKey(index))
                _pooling[index].StartWork();
            else
                throw new IndexOutOfRangeException(nameof(Ping));
        }
    }
}
