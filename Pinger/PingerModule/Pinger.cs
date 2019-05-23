using System;
using System.Timers;
using Pinger.Logger;
using Pinger.Protocols;

namespace Pinger.PingerModule
{
    class Pinger : IPinger
    {
        private Timer _timer;
        private int _interval = 5;
        public IProtocol Protocol { get; private set; }

        private readonly ILogger<Exception> _excLogger; 
        private readonly ILogger<String> _logger;

        public Pinger(IProtocol protocol, ILogger<Exception> excLogger, ILogger<string> logger)
        {
            Protocol = protocol ?? throw new NullReferenceException(nameof(Pinger));
            _excLogger = excLogger;
            _logger = logger;
        }

        public int Interval
        {
            get => _interval;
            set
            {
                if (value > 0)
                    _interval = value;
            }
        }

        public void StopWork()
        {
            _timer.Elapsed -= OnTimedEvent;
            StopTimer();
        }

        public void SetInterval(string value)
        {
            if (Int32.TryParse(value, out var tmp))
                _interval = tmp;
        }

        private void StartTimer()
        {
            if (_timer == null)
            {
                _timer = new Timer(Interval);
                _timer.Elapsed += OnTimedEvent;
            }
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            String message = $"HostName: {Protocol.HostName}\n Result: ";
            message += Protocol.SendRequestAsync(_excLogger).GetStatus ? "OK" : "FAILED";
            message += "\n Message From Process: " + Protocol.Message;
            _logger.Write(message);
        }

        private void StopTimer()
        {
            if (_timer == null)
                throw new NullReferenceException(nameof(_timer));
            _timer.Stop();
        }

        public void StartWork()
        {
            StartTimer();
        }
    }
}
