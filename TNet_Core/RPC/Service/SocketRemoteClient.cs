using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TNet.Log;
using TNet.RPC.Sockets;
using TNet.RPC.Sockets.Events;

namespace TNet.RPC.Service
{
    /// <summary>
    /// Remote client for socket
    /// </summary>
    public class SocketRemoteClient : RemoteClient
    {
        private const int BufferSize = 1024;
        private ClientSocket _client;
        private Encoding _encoding;
        private Timer _timer;

        /// <summary>
        /// init
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="heartInterval">ms</param>
        public SocketRemoteClient(string host, int port, int heartInterval)
        {
            IsSocket = true;
            var remoteEndPoint = new IPEndPoint(Dns.GetHostAddresses(host)[0], port);
            var settings = new ClientSocketSettings(BufferSize, remoteEndPoint);
            _client = new ClientSocket(settings);
            _client.DataReceived += OnDataReceived;
            _client.Disconnected += OnDisconnected;
            _timer = new Timer(DoCheckHeartbeat, null, 1000, heartInterval);

        }

        /// <summary>
        /// 
        /// </summary>
        public Encoding Encoding
        {
            set { _encoding = value; }
        }

        /// <summary>
        /// Heartbeat packet data.
        /// </summary>
        public byte[] HeartPacket { get; set; }

        /// <summary>
        /// Connect
        /// </summary>
        public void Connect()
        {
            _client.Connect();
            LocalAddress = _client.LocalEndPoint.ToString();
            Connected = true;
        }
        /// <summary>
        /// Close
        /// </summary>
        public override void Close()
        {
            _client.Close();
            Connected = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public bool Wait(int millisecondsTimeout = 0)
        {
            return _client.Wait(millisecondsTimeout);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public override async Task Send(string data)
        {
            byte[] buffer = (_encoding ?? Encoding.ASCII).GetBytes(data);
            await Send(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public override async Task Send(byte[] data)
        {
            if (!Connected)
            {
                Connect();
            }
            await _client.PostSend(data, 0, data.Length);
        }


        private void OnDataReceived(object sender, SocketEventArgs e)
        {
            try
            {
                var remoteArgs = new RemoteEventArgs() { Data = e.Data };
                OnCallback(remoteArgs);

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Socket remote received error:{0}", ex);
            }
        }

        private void OnDisconnected(object sender, SocketEventArgs e)
        {
            try
            {
                Connected = false;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Socket remote disconnected error:{0}", ex);
            }
        }

        private void DoCheckHeartbeat(object state)
        {
            try
            {
                if (HeartPacket != null && HeartPacket.Length > 0)
                {
                    Send(HeartPacket);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Socket remote heartbeat error:{0}", ex);
            }
        }

    }
}
