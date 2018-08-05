using System;
using System.Collections.Generic;
using System.Text;
using TNet.RPC.Sockets.ws;

namespace TNet.RPC.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public class DataToken
    {
        /// <summary>
        /// Receive meeage data
        /// </summary>
        public byte[] byteArrayForMessage;
        /// <summary>
        /// Receive message head data
        /// </summary>
        public byte[] byteArrayForPrefix;
        /// <summary>
        /// received message length
        /// </summary>
        public int messageBytesDone;
        /// <summary>
        /// received message head length
        /// </summary>
        public int prefixBytesDone;
        /// <summary>
        /// Wait has be received message length
        /// </summary>
        public int messageLength;
        /// <summary>
        /// in buffer pool offset
        /// </summary>
        public int bufferOffset;
        /// <summary>
        /// in buffer pool read offset
        /// </summary>
        public int bufferSkip;
        /// <summary>
        /// socket
        /// </summary>
        public TNSocket Socket;

        /// <summary>
        /// websocket handshake data.
        /// </summary>
        public List<byte> byteArrayForHandshake;
        /// <summary>
        /// Receive message head2 data
        /// </summary>
        public byte[] byteArrayForPrefix2;
        /// <summary>
        /// received message head2 length
        /// </summary>
        public int prefixBytesDone2;

        /// <summary>
        /// received mask
        /// </summary>
        public byte[] byteArrayMask;

        /// <summary>
        /// received mask length
        /// </summary>
        public int maskBytesDone;
        /// <summary>
        /// Web socket head
        /// </summary>
        public MessageHeadFrame HeadFrame { get; set; }

        /// <summary>
        /// Web socket data
        /// </summary>
        public List<DataSegmentFrame> DataFrames { get; set; }

        /// <summary>
        /// Sync recieve data.
        /// </summary>
        public Queue<ArraySegment<byte>> SyncSegments { get; set; }

        /// <summary>
        /// offset
        /// </summary>
        public int DataOffset
        {
            get { return bufferOffset + bufferSkip; }
        }

        /// <summary>
        /// Remain byte length
        /// </summary>
        public int RemainByte
        {
            get { return messageLength - messageBytesDone; }
        }

        /// <summary>
        /// message data is receive complated
        /// </summary>
        public bool IsMessageReady
        {
            get { return messageBytesDone == messageLength; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SocketAsyncResult AsyncResult { get; set; }

        /// <summary>
        /// init
        /// </summary>
        public DataToken()
        {
            byteArrayForPrefix = new byte[4];
            DataFrames = new List<DataSegmentFrame>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResultCallback(ResultCode result, Exception error = null)
        {
            if (AsyncResult != null)
            {
                AsyncResult.Result = result;
                AsyncResult.Error = error;
                AsyncResult.Callback();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        public virtual void Reset(bool skip)
        {
            byteArrayForMessage = null;
            Array.Clear(byteArrayForPrefix, 0, byteArrayForPrefix.Length);
            messageBytesDone = 0;
            prefixBytesDone = 0;
            messageLength = 0;
            byteArrayForPrefix2 = null;
            prefixBytesDone2 = 0;
            byteArrayMask = null;
            maskBytesDone = 0;
            HeadFrame = null;
            AsyncResult = null;
            if (skip)
            {
                bufferSkip = 0;
            }
        }
    }
}
