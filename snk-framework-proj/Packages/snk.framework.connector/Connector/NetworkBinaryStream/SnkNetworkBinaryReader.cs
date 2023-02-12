using System.IO;
using System.Threading.Tasks;

namespace SnkFramework.Connector
{
    public class SnkNetworkBinaryReader : SnkNetworkBinaryStream, ISnkNetworkBinaryReader
    {
        public SnkNetworkBinaryReader(Stream stream, bool bigEndian) : base(stream, bigEndian)
        {
        }

        public async Task<byte[]> Reader()
        {
            return default;
        }
    }
}