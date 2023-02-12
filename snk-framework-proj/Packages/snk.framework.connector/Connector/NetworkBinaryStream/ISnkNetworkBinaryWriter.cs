using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnkFramework.Connector
{
    public interface ISnkNetworkBinaryWriter
    {
        Task Writer(IEnumerable<byte> bytes);
    }
}