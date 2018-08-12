using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    public class ProtobufCacheSerializer : ICacheSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public byte[] Serialize(object obj)
        {
            return ProtoBufUtils.Serialize(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Deserialize(byte[] data, Type type)
        {
            return ProtoBufUtils.Deserialize(data, type);
        }
    }
}
