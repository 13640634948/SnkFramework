using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SnkFramework.Connector.Sample
{
    public class Service
    {
        private readonly int _port;
        
        [SerializeField] 
        private bool started = false;
        
        private TcpListener _tcpListener;
        
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;

        private readonly List<TcpClient> _clientList = new List<TcpClient>();

        private const bool secure = false;

        private X509Certificate _cert;
        
        private RemoteCertificateValidationCallback _remoteCertificateValidationCallback;

        public Service(int port)
        {
            this._port = port;
        }

        public void StartService()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            _tcpListener = TcpListener.Create(_port);
            _tcpListener.Start();
            started = true;
            Task.Run(() =>
            {
                while (true)
                {
                    if (_cancellationToken.IsCancellationRequested)
                        _cancellationToken.ThrowIfCancellationRequested();

                    var client = _tcpListener.AcceptTcpClient();
                    Work(client, _cancellationToken);
                }
            }, _cancellationToken);
        }


        private bool ValidateClientCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (_remoteCertificateValidationCallback != null)
                return _remoteCertificateValidationCallback(sender, certificate, chain, sslPolicyErrors);

            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            return certificate == null;
        }

        private void Work(TcpClient client, CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                try
                {
                    _clientList.Add(client);
                    Stream stream;
                    if (secure)
                    {
                        try
                        {
                            var sslStream = new SslStream(client.GetStream(), false, ValidateClientCertificate);
                            await sslStream.AuthenticateAsServerAsync(_cert, false, SslProtocols.Tls | SslProtocols.Ssl3 | SslProtocols.Ssl2, false);
                            stream = sslStream;
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogErrorFormat("AuthenticateAsServerAsync,Exception:{0}", e);
                            throw;
                        }
                    }
                    else
                    {
                        stream = client.GetStream();
                    }

                    ISnkNetworkBinaryReader reader = new SnkNetworkBinaryReader(stream, true);// new BinaryReader(stream, false);
                    ISnkNetworkBinaryWriter writer = new SnkNetworkBinaryWriter(stream, true);

                    //这里服务器共用了客户端的编码解码器
                    ISnkMessageDecoder decoder = new SnkMessageDecoder();
                    ISnkMessageEncoder encoder = new SnkMessageEncoder();

                    while (true)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            cancellationToken.ThrowIfCancellationRequested();

                        var readBytes = await reader.Reader();
                        //读取一条消息
                        var message = await decoder.Decode(readBytes);

                        switch (message)
                        {
                            case SnkRequest request:
                            {
                                //收到请求，返回一个响应消息
                                Debug.LogFormat("Server Received:{0}", request);

                                var response = new SnkResponse
                                {
                                    Status = 200,
                                    Sequence = request.Sequence, //必须与请求配对
                                    ContentType = 0,
                                    Content = Encoding.UTF8.GetBytes(request.CommandID==0 ? "pong" : "The server responds to the client")
                                };

                                //写入一条消息
                                var writeBytes = await encoder.Encode(response);
                                await writer.Writer(writeBytes);
                            
                                Debug.LogFormat("Server Sent:{0}", response);
                                continue;
                            }
                            case SnkNotification notification:
                            {
                                //收到通知，返回一个通知消息
                                Debug.LogFormat("Server Received:{0}", notification);

                                var response = new SnkNotification
                                {
                                    CommandID = 11,
                                    ContentType = 0,
                                    Content = Encoding.UTF8.GetBytes("The server sends a notification to the client")
                                };

                                //写入一条消息
                                var writeBytes = await encoder.Encode(response);
                                await writer.Writer(writeBytes);
                                
                                Debug.LogFormat("Server Sent:{0}", response);
                                continue;
                            }
                        }
                    }
                }
                finally
                {
                    if (client != null)
                    {
                        _clientList.Remove(client);
                        client.Close();
                    }
                }

            }, cancellationToken); 
        }
        
        
        public void StopService()
        {
            started = false;
            if (this._tcpListener != null)
            {
                this._tcpListener.Stop();
                this._tcpListener = null;
            }
            if (this._cancellationTokenSource != null)
            {
                this._cancellationTokenSource.Cancel();
                this._cancellationTokenSource = null;
            }

            if (this._clientList != null)
            {
                foreach (var client in this._clientList)
                {
                    client.Close();
                }
            }
        }
    }
}