using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Coms
{
   public class TNServer_Socket:EcsRx.Components.IComponent
    {

        public TNet.RPC.Sockets.SocketListener socket;


    public TNServer_Socket()
        {
            Init();
        }
        private void Init()
        {
            socket = new RPC.Sockets.SocketListener(new RPC.Sockets.SocketSettings(1000, 1, 1, 1024, 9001));
            socket.StartListen();
            Log.TraceLog.WriteInfo("监听在9001端口");
            socket.Connected += Socket_Connected;
            socket.DataReceived += Socket_DataReceived;
            socket.Disconnected += Socket_Disconnected;
        }

        private void Socket_Disconnected(RPC.Sockets.ISocket socket, RPC.Sockets.Events.ConnectionEventArgs e)
        {
            Log.TraceLog.WriteInfo("断开");
        }

        private void Socket_Connected(RPC.Sockets.ISocket socket, RPC.Sockets.Events.ConnectionEventArgs e)
        {
            Log.TraceLog.WriteInfo("链接");
        }

        private void Socket_DataReceived(RPC.Sockets.ISocket socket, RPC.Sockets.Events.ConnectionEventArgs e)
        {
            var ms = new RPC.IO.MessageStructure(e.Data);
    
            socket.PostSend(e.Socket, e.Data, 0, e.Data.Length);
          //  throw new NotImplementedException();
        }
    }
}
