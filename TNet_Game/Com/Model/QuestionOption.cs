using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using TNet.Event;

namespace TNet.Com.Model
{
    /// <summary>
    /// 问题选项
    /// </summary>
    [Serializable, ProtoContract]
    public class QuestionOption : CacheItemChangeEvent
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string Name { get; set; }
    }
}
