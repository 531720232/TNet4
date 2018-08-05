using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Redis
{
    /// <summary>
    /// Server redis info
    /// </summary>
    public class RedisInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public RedisInfo()
        {
            ClientVersion = RedisStorageVersion.Hash;
            SlaveSet = new Dictionary<string, RedisInfo>();
        }
        /// <summary>
        /// Server info hash
        /// </summary>
        public string HashCode { get; set; }

        /// <summary>
        /// Redis client version
        /// </summary>
        public RedisStorageVersion ClientVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServerPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SerializerType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StarTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, RedisInfo> SlaveSet { get; set; }

    }
}
