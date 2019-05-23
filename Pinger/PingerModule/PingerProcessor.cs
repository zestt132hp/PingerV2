using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Pinger.Configuration;
using Pinger.Logger;


namespace Pinger.PingerModule
{
    class PingerProcessor:IPingerProcessor
    {
       private readonly IConfigurationReader _configWorker;
       //factory don't use
       //private readonly IPingerFactory _factory;
       private readonly ILogger<Exception> _excLogger;
       private readonly ILogger<string> _logger;
       private readonly Dictionary<int, IPinger> _pooling = new Dictionary<int, IPinger>();
        public PingerProcessor(IConfigurationReader confWorker, ILogger<Exception> eLog, ILogger<string> sLog) //IPingerFactory factory,
        {
            _excLogger = eLog??throw new NullReferenceException(nameof(eLog));
            _logger = sLog??throw new NullReferenceException(nameof(sLog));
            //_factory = factory??throw new NullReferenceException(nameof(factory));
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
                _pooling.Add(keyValuePair.Key, new Pinger(keyValuePair.Value, _excLogger, _logger));
                //does not work
                //IProtocol protocol = keyValuePair.Value;
                //_pooling.Add(keyValuePair.Key, _factory.CreatePinger(protocol, excLogger, logger));
            }
        }

        public void Ping()
        {
            _pooling.Values.AsParallel().ForAll(x=>x.StartWork());
        }

        public void StopPing()
        {
            _pooling.Values.AsParallel().ForAll(x => x.StopWork());
            Thread.Sleep(2000);
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
            if(_pooling.ContainsKey(index))
                _pooling[index].StartWork();
            else
                throw new IndexOutOfRangeException(nameof(Ping));
        }
    }
}
