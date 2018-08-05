using System;
using System.Collections.Generic;
using System.Text;
using TNet.RPC.IO;

namespace TNet.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class SocketGameResponse : BaseGameResponse
    {
        static SocketGameResponse()
        {
            var setting = Runtime.GameZone.Setting;
            if (setting != null)
            {
                MessageStructure.Enable7zip = setting.ActionEnable7Zip;
                MessageStructure.Enable7zipMinByte = setting.Action7ZipOutLength;
            }
        }

        private MessageStructure _buffers;
        /// <summary>
        /// 
        /// </summary>
        public SocketGameResponse()
        {
            _buffers = new MessageStructure();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public override void BinaryWrite(byte[] buffer)
        {
          
            DoWrite(buffer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public override void Write(byte[] buffer)
        {
            DoWrite(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] ReadByte()
        {
            return _buffers.PosGzipBuffer();
        }

        private void DoWrite(byte[] buffer)
        {
            _buffers.WriteByte(buffer);
        }

    }
}
