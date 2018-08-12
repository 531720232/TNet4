using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.IO
{
    [MessagePack.MessagePackObject]
  public class ListWR
    {
        [MessagePack.Key(1)]
        public List<byte[]> buffer { get; set; } = new List<byte[]>();
        [MessagePack.Key(2)]
        public int offset { get; set; }
        public void Write(object obj)
        {
          var bytes= MessagePackSerializer.Serialize(obj);
            Write(bytes);
        }
        public void Write<T>(T obj)
        {
            var bytes = MessagePackSerializer.Serialize(obj);
            Write(bytes);
        }
        public void Write(byte[] obj)
        {
            buffer.Add(obj);

            offset++;
        }
        public byte[] ToBytes()
        {
            var bytes = MessagePackSerializer.Serialize(this);
            return bytes;
        }
     
        public T Read<T>()
        {
            if (buffer.Count < 1)
            {
                return default(T);
            }
            if (offset == 0)
            {
                offset = 1;
                //   return null;
            }
            var obj = MessagePackSerializer.Deserialize<T>(buffer[offset - 1]);
            offset++;
            return obj;
        }
        //public int             ReadInt()
        //{
        //    if (buffer.Count < 1)
        //    {
        //        return 1;
        //    }
        //    if (offset == 0)
        //    {
        //        offset = 1;
        //        //   return null;
        //    }
        //    var obj= MessagePackSerializer.Deserialize<int>(buffer[offset-1]);
        //    offset++;
        //    return obj;
        //}
        //public string ReadString()
        //{
        //    if(buffer.Count<1)
        //    {
        //        return null;
        //    }
        //    if (offset==0)
        //    {
        //        offset =1;
        //     //   return null;
        //    }
        //    var obj = MessagePackSerializer.Deserialize<string>(buffer[offset - 1]);
        //    offset++;
        //    return obj;
        //}
    }

}
