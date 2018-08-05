using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TNet.RPC.Service
{
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
        public abstract Task Send(string data);

        /// <summary>
        /// Send
        /// </summary>
        /// <param name="data"></param>
        public abstract Task Send(byte[] data);

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
}
