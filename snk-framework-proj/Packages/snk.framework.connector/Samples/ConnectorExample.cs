using System;
using UnityEngine;

namespace SnkFramework.Connector.Sample
{
    public class ConnectorExample : MonoBehaviour
    {
        private string host = "127.0.0.1";
       
        private int port = 8090;

        private bool ServiceStarted;
        
        private bool Connected;

        private Service _service;

        private SnkTcpSocketPipeline<SnkMessageDecoder, SnkMessageEncoder> _socketPipeline;
        
        async void Connect()
        {
            try
            {
                await _socketPipeline.Connect(host, port);
                Debug.LogFormat("连接成功");
            }
            catch (Exception e)
            {
                Debug.LogFormat("连接异常：{0}", e);
            }
        }
        
        public void Start()
        {
            this._service = new Service(port);
            this._socketPipeline = new SnkTcpSocketPipeline<SnkMessageDecoder, SnkMessageEncoder>();

        }

        public void OnGUI()
        {
            if (GUILayout.Button((this.ServiceStarted ? "Stop" : "Start") + "Service"))
            {
                if (this.ServiceStarted)
                    this._service.StopService();
                else
                    this._service.StartService();
            }

            if (GUILayout.Button((this.Connected ? "Disconnect" : "Connect") + "Service"))
            {
                if (Connected)
                    _ = _socketPipeline.Disconnect();
                else
                    Connect();
            }
        }
        
        private void OnDestroy()
        {
            if (this._service != null)
            {
                this._service.StopService();
                this._service = null;
            }
        }
    }
}