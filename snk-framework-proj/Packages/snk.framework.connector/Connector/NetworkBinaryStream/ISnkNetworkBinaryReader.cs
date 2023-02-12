using System.Threading.Tasks;

namespace SnkFramework.Connector
{
    public interface ISnkNetworkBinaryReader
    {
        Task<byte[]> Reader();
    }
}