using System.Threading.Tasks;

namespace SnkFramework.Connector
{
    public class SnkConnector : ISnkConnector
    {
        private ISnkSocketPipeline _socketPipeline;

        public SnkConnector(ISnkSocketPipeline socketPipeline)
        {
            this._socketPipeline = socketPipeline;
        }

        public async Task Connect()
        {

        }

        public async Task Disconnect()
        {
            //await this.SocketPipeline.Close();
        }
    }
}