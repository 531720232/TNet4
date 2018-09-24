using System;
using System.Collections.Generic;
using System.Text;
using TNet.Configuration;

namespace TNet.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheSection : ConfigSection
    {
        /// <summary>
        /// 
        /// </summary>
        public CacheSection()
        {
            UpdateInterval = ConfigUtils.GetSetting("Cache.update.interval", 6); //10 Minute 600
            ExpiredInterval = ConfigUtils.GetSetting("Cache.expired.interval", 600);
            IsStorageToDb = ConfigUtils.GetSetting("Cache.IsStorageToDb", false);
            SerializerType = ConfigUtils.GetSetting("Cache.Serializer", "Protobuf");
            ShareExpirePeriod = ConfigUtils.GetSetting("Cache.global.period", 3 * 86400); //72 hour
            PersonalExpirePeriod = ConfigUtils.GetSetting("Cache.user.period", 6); //24 hour 86400
        }

        /// <summary>
        /// The cache expiry interval.
        /// </summary>
        public int ExpiredInterval { get; set; }

        /// <summary>
        /// The cache update interval.
        /// </summary>
        public int UpdateInterval { get; set; }


        /// <summary>
        /// Redis data is storage to Db.
        /// </summary>
        public bool IsStorageToDb { get; set; }

        /// <summary>
        /// cache serialize to redis's type, protobuf or json
        /// </summary>
        public string SerializerType { get; set; }

        /// <summary>
        /// Personal cache expire period, default 24h
        /// </summary>
        public int PersonalExpirePeriod { get; set; }
        /// <summary>
        /// cache expire period
        /// </summary>
        public int ShareExpirePeriod { get; set; }

    }
}
