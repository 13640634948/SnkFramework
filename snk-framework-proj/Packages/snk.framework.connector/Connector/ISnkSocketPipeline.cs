using System.Threading.Tasks;

namespace SnkFramework.Connector
{
    public interface ISnkSocketPipeline
    {
        Task Connect(string host, int port);

        Task Disconnect();

        Task<ISnkMessage> Request(ISnkMessage request);

        Task Notify(ISnkMessage notification);

    }
}