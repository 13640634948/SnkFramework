using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SnkFramework.Connector
{
    public class SnkTcpSocketPipeline<TDecoder, TEncoder> : ISnkSocketPipeline
        where TDecoder : class, ISnkMessageDecoder, new()
        where TEncoder : class, ISnkMessageEncoder, new()
    {
        private readonly TcpClient _client;
        private readonly bool _bigEndian;

        private ISnkNetworkBinaryReader _reader;
        private ISnkNetworkBinaryWriter _writer;

        private TDecoder _decoder;
        private TEncoder _encoder;

        private List<ISnkMessage> _requestList = new();

        private ConcurrentDictionary<uint, ISnkMessage> _requestDict = new();

        public SnkTcpSocketPipeline(bool bigEndian = true)
        {
            this._client = new TcpClient();
            this._bigEndian = bigEndian;
        }

        public async Task Connect(string host, int port)
        {
            await _client.ConnectAsync(host, port);
            var networkStream = this._client.GetStream();
            this._writer = new SnkNetworkBinaryWriter(networkStream, this._bigEndian);
            this._reader = new SnkNetworkBinaryReader(networkStream, this._bigEndian);
        }

        private async Task sendMessage(ISnkMessage messageRequest)
        {
            var bytes = await this._encoder.Encode(messageRequest);
            await this._writer.Writer(bytes);
        }

        protected virtual void onReceiveMessage(ISnkMessage message)
        {

        }

        private async Task startReceiveMessage()
        {
            while (true)
            {
                var bytes = await this._reader.Reader();
                var message = await this._decoder.Decode(bytes);


                this.onReceiveMessage(message);
            }
        }

        public async Task Disconnect()
        {

        }


        public async Task<ISnkMessage> Request(ISnkMessage request)
        {
            this._requestList.Add(request);
            await this.sendMessage(request);
            return default;
        }

        public async Task Notify(ISnkMessage notification)
        {
            await this.sendMessage(notification);
        }
    }
}