using System.Threading.Tasks;

namespace SnkFramework.Connector
{
    public class SnkMessageDecoder : ISnkMessageDecoder
    {
        public async Task<ISnkMessage> Decode(byte[] bytes)
        {
            return default;
        }
    }
}