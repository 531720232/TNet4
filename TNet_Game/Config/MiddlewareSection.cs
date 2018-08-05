using System;
using System.Collections.Generic;
using System.Text;
using TNet.Configuration;

namespace TNet.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class MiddlewareSection : ConfigSection
    {
        /// <summary>
        /// init
        /// </summary>
        public MiddlewareSection()
        {
            EnableGM = ConfigUtils.GetSetting("EnableGM", false);
            BroadcastMaxCount = ConfigUtils.GetSetting("broadcastcache_maxcount", 1000);
            BroadcastTimeout = ConfigUtils.GetSetting("broadcastcache_timeout", 1800);

            ChatMaxCount = ConfigUtils.GetSetting("chatcache_maxcount", 3000);
            ChatTimeout = ConfigUtils.GetSetting("chatcache_timeout", 1800); //30分钟

            PreAccount = string.Format("{0}", ConfigUtils.GetSetting("Sns.PreAccount", "Z"));
        }

        /// <summary>
        /// 
        /// </summary>
        public string PreAccount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool EnableGM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BroadcastMaxCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BroadcastTimeout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ChatMaxCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ChatTimeout { get; set; }
    }
}
