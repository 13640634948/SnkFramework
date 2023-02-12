using System.Threading.Tasks;

namespace SnkFramework.Connector
{
    public class SnkConnector<TSocketPipeline> : ISnkConnector
        where TSocketPipeline : class, ISnkSocketPipeline, new()
    {
        public ISnkSocketPipeline SocketPipeline { get; protected set; }

        public async Task Connect()
        {

        }

        public async Task Disconnect()
        {
            //await this.SocketPipeline.Close();
        }
    }
}