
using ProtoBuf;
using System;

namespace TNet.Com.Model
{
    /// <summary>
    /// 排行榜数据项
    /// </summary>
    [Serializable, ProtoContract]
    public class RankingItem
    {
        /// <summary>
        /// 排名,从1开始
        /// </summary>
        public virtual int RankId
        {
            get;
            set;
        }
    }
}