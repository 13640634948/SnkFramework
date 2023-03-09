using System.IO;

namespace SnkFramework.Connector
{
    public abstract class SnkNetworkBinaryStream
    {
        private Stream _stream;
        private bool _bigEndian;

        protected SnkNetworkBinaryStream(Stream stream, bool bigEndian)
        {
            this._stream = stream;
            this._bigEndian = bigEndian;
        }
    }
}