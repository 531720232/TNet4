using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNet.Extend
{
  public  static class PackageHelper
    {
        public const bool use7z = false;

        private static string m_guid="null";
        private static string guid
        {
            get
            {
                if(m_guid== "null")
                {
                    m_guid = Guid.NewGuid().ToString();
                }
                return m_guid;

            }

        }
        [Obsolete]
        public static TNet.IO.EndianBinaryWriter Start(int msgid,string sid,int act_id,int user_id)
        {
            var new_m = new System.IO.MemoryStream();
            var wr = new TNet.IO.EndianBinaryWriter(IO.EndianBitConverter.Little, new_m);
            wr.Write(msgid);
            wr.Write(guid);
            wr.Write(sid);
            wr.Write(act_id);
            wr.Write(user_id);

            return wr;
        }
        public static RPC.IO.MessageStructure Create_A_Pack(int msg_id,int action,int errorCode=0,string error="",bool m_7z=true)
        {
            var pack = new RPC.IO.MessageStructure();
            pack.WriteBuffer(new RPC.IO.MessageHead(msg_id, action, "st", errorCode, error) { Has7zip=m_7z});
       
            //  pack.WriteByte("六道的灵魂");
            return pack;
        }
        public static byte[] ToBytes(this TNet.IO.EndianBinaryWriter w)
        {
        

            var bytes = new byte[w.BaseStream.Length];
            w.BaseStream.Position = 0;
                  w.BaseStream.ReadAsync(bytes,0,bytes.Length).Wait();

            return bytes;//bytes;
        }
       
    }
}
