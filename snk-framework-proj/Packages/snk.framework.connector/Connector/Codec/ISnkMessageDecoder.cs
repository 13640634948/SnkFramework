using System.Threading.Tasks;

namespace SnkFramework.Connector
{
    public interface ISnkMessageDecoder
    {
        Task<ISnkMessage> Decode(byte[] bytes);
    }
}