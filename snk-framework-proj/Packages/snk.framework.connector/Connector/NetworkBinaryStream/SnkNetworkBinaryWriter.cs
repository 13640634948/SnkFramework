using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SnkFramework.Connector
{
    public class SnkNetworkBinaryWriter : SnkNetworkBinaryStream, ISnkNetworkBinaryWriter
    {
        public SnkNetworkBinaryWriter(Stream stream, bool bigEndian) : base(stream, bigEndian)
        {
        }

        public async Task Writer(IEnumerable<byte> bytes)
        {
        }
    }
}