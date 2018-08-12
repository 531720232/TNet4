using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using TNet.Client.Log;
using TNet.RPC.Sockets;


/// <summary>
/// Remote EventArgs
/// </summary>
public class RemoteEventArgs : EventArgs
{
    /// <summary>
    /// data
    /// </summary>
    public byte[] Data { get; set; }

    /// <summary>
    /// user data
    /// </summary>
    public object UserData { get; set; }
}


/// <summary>
/// RemoteCallback delegate
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public delegate void RemoteCallback(object sender, RemoteEventArgs e);

/// <summary>
/// Remote client
/// </summary>
public abstract class RemoteClient
{
    /// <summary>
    /// Remote Target
    /// </summary>
    public object RemoteTarget { get; set; }

    /// <summary>
    /// callback event.
    /// </summary>
    public event RemoteCallback Callback;

    /// <summary>
    /// Is socket client
    /// </summary>
    public bool IsSocket { get; protected set; }
    /// <summary>
    /// 
    /// </summary>
    public string LocalAddress { get; protected set; }

    /// <summary>
    /// Send
    /// </summary>
    /// <param name="data"></param>
    public abstract void Send(string data);

    /// <summary>
    /// Send
    /// </summary>
    /// <param name="data"></param>
    public abstract void Send(byte[] data);

    /// <summary>
    /// 
    /// </summary>
    public abstract void Close();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnCallback(RemoteEventArgs e)
    {
        RemoteCallback handler = Callback;
        if (handler != null) handler(RemoteTarget, e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    protected byte[] ReadStream(Stream stream, Encoding encoding)
    {
        List<byte> data = new List<byte>();
        BinaryReader readStream;
        using (readStream = new BinaryReader(stream, encoding))
        {
            int size = 0;
            while (true)
            {
                var buffer = new byte[1024];
                size = readStream.Read(buffer, 0, buffer.Length);
                if (size == 0)
                {
                    break;
                }
                byte[] temp = new byte[size];
                Buffer.BlockCopy(buffer, 0, temp, 0, size);
                data.AddRange(temp);
            }
            return data.ToArray();
        }
    }

}

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
    public override  void Send(string data)
    {
        byte[] buffer = (_encoding ?? Encoding.UTF8).GetBytes(data);
         Send(buffer);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public override void Send(byte[] data)
    {
        if (!Connected)
        {
            Connect();
        }
        _client.PostSend(data, 0, data.Length);
    }

    public static Guid SSID=Guid.NewGuid();
    public static int UserId;
    public static string sid="";
    public static string pid = "";
    public static int ptcl = 0;//1->extend head
     

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public  void SendAction(TNet.Client.GameAction action,TNet.RPC.IO.MessageHead head,TNet.RPC.IO.RequestParam param)
    {
        var writer = new TNet.IO.EndianBinaryWriter(TNet.IO.EndianBitConverter.Little, new MemoryStream());
        // writer.WriteObj(SSID);//ssid

        writer.Write(param.ToPostString());
        //writer.Write(action.Id);
        //writer.Write(head.MsgId);
        //writer.Write(UserId);//uid
        //writer.Write(sid);
        //writer.Write(pid);
        //writer.Write(ptcl);//ptcl
        //writer.WriteObj(param);
        writer.Write(action.Reader.To7ZBytes());
     var s=   writer.To7ZBytes();
        Send(s);
    }


    private void OnDataReceived(object sender, SocketEventArgs e)
    {
        try
        {
            var remoteArgs = new RemoteEventArgs() { Data = e.Data };
            OnCallback(remoteArgs);
            GameLoger.Write(e.Source.Message);
            var str = e.Source.Message;
            var buff = LZMA.Decompress(e.Data);
            var buffw = new TNet.IO.EndianBinaryReader(TNet.IO.EndianBitConverter.Little, new MemoryStream(buff));

        var id=    buffw.ReadInt32();
            var errc = buffw.ReadInt32();//error_code

            var msg = buffw.ReadInt32();//msgid
  
            var info = buffw.ReadString2();//error_info
            if (errc != 0&&string.IsNullOrEmpty(info))
            {

                throw new Exception($"{errc}:{info}");
            }

      
            var aid2 = buffw.ReadInt32();//action_id
        
            var st = buffw.ReadString2();//st
          
       
        

            var messh = new TNet.RPC.IO.MessageHead(msg, aid2, st, errc, info);


            //var bit = MessagePack.MessagePackSerializer.Serialize(messh);
            // var json = MessagePack.MessagePackSerializer.ToJson(bit);
            var action = TNet.Client.ActionFactory.Create(aid2,buffw);
            if(action!=null)
            {

            }
            else
            {
                throw new Exception($"Action{aid2}不存在");
            }

          
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