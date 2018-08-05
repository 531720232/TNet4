using System;
using System.Collections.Generic;
using System.Text;
using TNet.Configuration;
using TNet.Config;
namespace TNet.Redis
{
    /// <summary>
    /// Redis Pool Setting
    /// </summary>
    public class RedisPoolSetting
    {
        private RedisSection _redisSection;

        /// <summary>
        /// 
        /// </summary>
        public RedisPoolSetting(bool useConfig = true)
        {
            _redisSection = useConfig ? ConfigManager.Configger.GetFirstOrAddConfig<RedisSection>() : new RedisSection(false);
        }

        /// <summary>
        /// Host, format:password@ip:port
        /// </summary>
        public string Host
        {
            get { return _redisSection.Host; }
            set { _redisSection.Host = value; }
        }
        /// <summary>
        /// ReadOnlyHost
        /// </summary>
        public string ReadOnlyHost
        {
            get { return _redisSection.ReadOnlyHost; }
            set { _redisSection.Host = value; }
        }
        /// <summary>
        /// MaxWritePoolSize
        /// </summary>
        public int MaxWritePoolSize
        {
            get { return _redisSection.MaxWritePoolSize; }
            set { _redisSection.MaxWritePoolSize = value; }
        }
        /// <summary>
        /// MaxReadPoolSize
        /// </summary>
        public int MaxReadPoolSize
        {
            get { return _redisSection.MaxReadPoolSize; }
            set { _redisSection.MaxReadPoolSize = value; }
        }
        /// <summary>
        /// ConnectTimeout(ms)
        /// </summary>
        public int ConnectTimeout
        {
            get { return _redisSection.ConnectTimeout; }
            set { _redisSection.ConnectTimeout = value; }
        }
        /// <summary>
        /// PoolTimeOut(ms), default 2000ms
        /// </summary>
        public int PoolTimeOut
        {
            get { return _redisSection.PoolTimeOut; }
            set { _redisSection.PoolTimeOut = value; }
        }
        /// <summary>
        /// DbIndex
        /// </summary>
        public int DbIndex
        {
            get { return _redisSection.DbIndex; }
            set { _redisSection.DbIndex = value; }
        }

        /// <summary>
        /// ClientVersion
        /// </summary>
        public RedisStorageVersion ClientVersion
        {
            get { return _redisSection.ClientVersion; }
            set { _redisSection.ClientVersion = value; }
        }
    }
}
