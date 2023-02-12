using System.Threading;

namespace SnkFramework.Connector
{
    public class RequestTaskTimeoutOrCompletionSource : TaskTimeoutOrCompletionSource<ISnkResponse>
    {
        private ISnkRequest request;

        public RequestTaskTimeoutOrCompletionSource(ISnkRequest request, int timeoutMilliseconds,
            CancellationToken cancellationToken) : base(timeoutMilliseconds, cancellationToken)
        {
            this.request = request;
        }

        public ISnkRequest Request
        {
            get { return this.request; }
        }

        public uint Sequence
        {
            get { return this.request.Sequence; }
        }
    }
}