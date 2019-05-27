using System;
using System.Timers;
using Pinger.Logger;
using Pinger.Protocols;

namespace Pinger.PingerModule
{
    public class Pinger : IPinger
    {
        private Timer _timer;
        private int _interval;
        public IProtocol Protocol { get; }

        private readonly ILogger _logger;

        public Pinger(IProtocol protocol, ILogger logger)
        {
            Protocol = protocol ?? throw new NullReferenceException(nameof(Pinger));
            _interval = protocol.Interval;
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
            _timer.Dispose();
        }

        private void StartTimer()
        {
            if (_timer == null)
            {
                _timer = new Timer(Protocol.Interval);
                _timer.Elapsed += OnTimedEvent;
            }

            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
        }

        private async void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            String message = $"HostName: {Protocol.HostName}\n Result: ";
            var result = await Protocol.SendRequestAsync(_logger);
            message += result.GetStatus ? "OK" : "FAILED";
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
