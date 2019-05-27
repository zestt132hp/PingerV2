namespace Pinger.Protocols
{
    public class RequestStatus
    {
        private readonly bool _isSuccess;
        public bool GetStatus => _isSuccess;

        public RequestStatus(bool isSuccess)
        {
            _isSuccess = isSuccess;
        }
    }
}
